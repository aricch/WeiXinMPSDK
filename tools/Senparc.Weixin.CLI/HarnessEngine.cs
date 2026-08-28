using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Senparc.Weixin.Cli;

internal sealed class HarnessEngine
{
    private readonly WorkspacePolicy _policy;

    internal HarnessEngine(WorkspacePolicy policy)
    {
        _policy = policy;
    }

    internal IReadOnlyList<PreparedChange> Prepare(HarnessPlan plan, bool allowDelete)
    {
        ArgumentNullException.ThrowIfNull(plan);
        if (plan.SchemaVersion != 1)
        {
            throw new InvalidOperationException($"Unsupported plan schema: {plan.SchemaVersion}");
        }

        if (plan.Operations.Count > _policy.Manifest.MaxOperations)
        {
            throw new InvalidOperationException(
                $"Plan contains {plan.Operations.Count} operations; limit is {_policy.Manifest.MaxOperations}.");
        }

        var prepared = new List<PreparedChange>(plan.Operations.Count);
        var seenPaths = new HashSet<string>(OperatingSystem.IsWindows()
            ? StringComparer.OrdinalIgnoreCase
            : StringComparer.Ordinal);

        foreach (var operation in plan.Operations)
        {
            var absolutePath = _policy.ResolveForWrite(operation.Path);
            var relativePath = _policy.ToRelative(absolutePath);
            if (!seenPaths.Add(relativePath))
            {
                throw new InvalidOperationException($"A plan may modify each path only once: {relativePath}");
            }

            var original = File.Exists(absolutePath) ? File.ReadAllBytes(absolutePath) : null;
            var originalUnixMode = GetUnixFileMode(absolutePath);
            var kind = operation.Kind.Trim().ToLowerInvariant();
            PreparedChange change = kind switch
            {
                "write" => PrepareWrite(operation, relativePath, absolutePath, original, originalUnixMode),
                "replace" => PrepareReplace(operation, relativePath, absolutePath, original, originalUnixMode),
                "delete" when allowDelete => PrepareDelete(operation, relativePath, absolutePath, original, originalUnixMode),
                "delete" => throw new InvalidOperationException(
                    $"Delete requires the explicit --allow-delete option: {relativePath}"),
                _ => throw new InvalidOperationException($"Unsupported operation kind '{operation.Kind}'.")
            };
            prepared.Add(change);
        }

        return prepared;
    }

    internal async Task<HarnessReport> ExecuteAsync(
        HarnessPlan plan,
        string planner,
        HarnessExecutionOptions options,
        CancellationToken cancellationToken)
    {
        var report = new HarnessReport
        {
            Goal = plan.Goal,
            Workspace = _policy.Workspace,
            Planner = planner,
            Summary = plan.Summary
        };

        try
        {
            var changes = Prepare(plan, options.AllowDelete);
            report.ChangedFiles.AddRange(changes.Select(change => change.RelativePath));

            if (!options.Apply)
            {
                report.Status = "planned";
                report.Diagnostics.Add("Dry-run completed. No workspace files were changed.");
                return await FinishAsync(report, options.ReportPath, cancellationToken).ConfigureAwait(false);
            }

            if (!options.AllowDirty)
            {
                await RejectDirtyTargetsAsync(changes, cancellationToken).ConfigureAwait(false);
            }

            Apply(changes);
            try
            {
                foreach (var step in _policy.Manifest.Verification)
                {
                    var result = await VerifyAsync(step, cancellationToken).ConfigureAwait(false);
                    report.Verification.Add(result);
                    if (result.ExitCode != 0)
                    {
                        throw new VerificationFailedException(
                            $"{step.Kind} failed for {step.Project} with exit code {result.ExitCode}.");
                    }
                }

                report.Status = "applied";
            }
            catch
            {
                if (!options.KeepFailed)
                {
                    Rollback(changes);
                    report.Status = "rolled-back";
                    report.Diagnostics.Add("All file changes were rolled back after verification failed.");
                }
                else
                {
                    report.Status = "failed-kept";
                    report.Diagnostics.Add("Verification failed; --keep-failed preserved the changes.");
                }

                throw;
            }

            return await FinishAsync(report, options.ReportPath, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            if (report.Status == "started")
            {
                report.Status = "failed";
            }

            report.Diagnostics.Add(exception.Message);
            await FinishAsync(report, options.ReportPath, cancellationToken).ConfigureAwait(false);
            throw;
        }
    }

    private PreparedChange PrepareWrite(
        FileOperation operation,
        string relativePath,
        string absolutePath,
        byte[]? original,
        UnixFileMode? originalUnixMode)
    {
        if (operation.Content is null)
        {
            throw new InvalidOperationException($"write requires content: {relativePath}");
        }

        if (original is not null)
        {
            throw new InvalidOperationException(
                $"write can only create a new file; use replace for an existing file: {relativePath}");
        }

        ValidateExpectedHash(operation, relativePath, original);
        var newContent = EncodeAndValidate(operation.Content, relativePath, HasUtf8Preamble(original));
        return new PreparedChange(relativePath, absolutePath, original, newContent, Delete: false, originalUnixMode);
    }

    private PreparedChange PrepareReplace(
        FileOperation operation,
        string relativePath,
        string absolutePath,
        byte[]? original,
        UnixFileMode? originalUnixMode)
    {
        if (original is null)
        {
            throw new InvalidOperationException($"replace target does not exist: {relativePath}");
        }

        var replacements = GetReplacements(operation, relativePath);
        ValidateExpectedHash(operation, relativePath, original);
        var updated = DecodeUtf8(original, relativePath);
        foreach (var replacement in replacements)
        {
            if (string.IsNullOrEmpty(replacement.OldText))
            {
                throw new InvalidOperationException($"replace requires non-empty oldText: {relativePath}");
            }

            var occurrenceCount = CountOccurrences(updated, replacement.OldText);
            var expectedOccurrences = Math.Max(replacement.ExpectedOccurrences, 1);
            if (occurrenceCount != expectedOccurrences)
            {
                throw new InvalidOperationException(
                    $"replace expected {expectedOccurrences} occurrence(s), found {occurrenceCount}: {relativePath}");
            }

            updated = updated.Replace(replacement.OldText, replacement.NewText, StringComparison.Ordinal);
        }

        return new PreparedChange(
            relativePath,
            absolutePath,
            original,
            EncodeAndValidate(updated, relativePath, HasUtf8Preamble(original)),
            Delete: false,
            originalUnixMode);
    }

    private static IReadOnlyList<TextReplacement> GetReplacements(
        FileOperation operation,
        string relativePath)
    {
        if (operation.Replacements.Count > 0)
        {
            if (operation.OldText is not null || operation.NewText is not null)
            {
                throw new InvalidOperationException(
                    $"replace must use either replacements or oldText/newText, not both: {relativePath}");
            }

            return operation.Replacements;
        }

        if (operation.OldText is null || operation.NewText is null)
        {
            throw new InvalidOperationException(
                $"replace requires replacements or oldText/newText: {relativePath}");
        }

        return
        [
            new TextReplacement
            {
                OldText = operation.OldText,
                NewText = operation.NewText,
                ExpectedOccurrences = operation.ExpectedOccurrences
            }
        ];
    }

    private PreparedChange PrepareDelete(
        FileOperation operation,
        string relativePath,
        string absolutePath,
        byte[]? original,
        UnixFileMode? originalUnixMode)
    {
        if (original is null)
        {
            throw new InvalidOperationException($"delete target does not exist: {relativePath}");
        }

        ValidateExpectedHash(operation, relativePath, original);
        return new PreparedChange(relativePath, absolutePath, original, null, Delete: true, originalUnixMode);
    }

    private void ValidateExpectedHash(FileOperation operation, string relativePath, byte[]? original)
    {
        if (original is null)
        {
            if (!string.IsNullOrWhiteSpace(operation.ExpectedSha256))
            {
                throw new InvalidOperationException($"New file must not specify expectedSha256: {relativePath}");
            }

            return;
        }

        if (string.IsNullOrWhiteSpace(operation.ExpectedSha256))
        {
            throw new InvalidOperationException($"Existing file requires expectedSha256: {relativePath}");
        }

        var actual = WorkspacePolicy.ComputeSha256(original);
        if (!actual.Equals(operation.ExpectedSha256, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Content changed since the file was read (SHA-256 mismatch): {relativePath}");
        }
    }

    private byte[] EncodeAndValidate(string content, string relativePath, bool preserveUtf8Preamble)
    {
        var payload = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true)
            .GetBytes(content);
        var bytes = payload;
        if (preserveUtf8Preamble)
        {
            var preamble = Encoding.UTF8.Preamble;
            bytes = new byte[preamble.Length + payload.Length];
            preamble.CopyTo(bytes);
            payload.CopyTo(bytes.AsSpan(preamble.Length));
        }
        if (bytes.Length > _policy.Manifest.MaxFileBytes)
        {
            throw new InvalidOperationException(
                $"Generated file exceeds {_policy.Manifest.MaxFileBytes} bytes: {relativePath}");
        }

        return bytes;
    }

    private static string DecodeUtf8(byte[] content, string relativePath)
    {
        try
        {
            var offset = content.AsSpan().StartsWith(Encoding.UTF8.Preamble) ? Encoding.UTF8.Preamble.Length : 0;
            return new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true)
                .GetString(content, offset, content.Length - offset);
        }
        catch (DecoderFallbackException exception)
        {
            throw new InvalidOperationException($"Only UTF-8 text files can be modified: {relativePath}", exception);
        }
    }

    private static bool HasUtf8Preamble(byte[]? content) =>
        content is not null && content.AsSpan().StartsWith(Encoding.UTF8.Preamble);

    private static UnixFileMode? GetUnixFileMode(string path) =>
        !OperatingSystem.IsWindows() && File.Exists(path) ? File.GetUnixFileMode(path) : null;

    private static int CountOccurrences(string content, string value)
    {
        var count = 0;
        var offset = 0;
        while ((offset = content.IndexOf(value, offset, StringComparison.Ordinal)) >= 0)
        {
            count++;
            offset += value.Length;
        }

        return count;
    }

    private static void Apply(IReadOnlyList<PreparedChange> changes)
    {
        var applied = new List<PreparedChange>(changes.Count);
        try
        {
            foreach (var change in changes)
            {
                WriteChange(change, useNewContent: true);
                applied.Add(change);
            }
        }
        catch
        {
            Rollback(applied);
            throw;
        }
    }

    private static void Rollback(IEnumerable<PreparedChange> changes)
    {
        foreach (var change in changes.Reverse())
        {
            WriteBytesAtomically(change.AbsolutePath, change.OriginalContent, change.OriginalUnixMode);
        }
    }

    private static void WriteChange(PreparedChange change, bool useNewContent)
    {
        var content = useNewContent ? change.NewContent : change.OriginalContent;
        WriteBytesAtomically(
            change.AbsolutePath,
            change.Delete && useNewContent ? null : content,
            change.OriginalUnixMode);
    }

    private static void WriteBytesAtomically(string path, byte[]? content, UnixFileMode? unixFileMode)
    {
        if (content is null)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            return;
        }

        var directory = Path.GetDirectoryName(path)
            ?? throw new InvalidOperationException($"Cannot determine parent directory for {path}.");
        Directory.CreateDirectory(directory);
        var temporaryPath = Path.Combine(directory, $".{Path.GetFileName(path)}.{Guid.NewGuid():N}.tmp");
        try
        {
            File.WriteAllBytes(temporaryPath, content);
            if (unixFileMode.HasValue && !OperatingSystem.IsWindows())
            {
                File.SetUnixFileMode(temporaryPath, unixFileMode.Value);
            }

            File.Move(temporaryPath, path, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }
    }

    private async Task RejectDirtyTargetsAsync(
        IReadOnlyList<PreparedChange> changes,
        CancellationToken cancellationToken)
    {
        foreach (var change in changes.Where(change => change.OriginalContent is not null))
        {
            var result = await ProcessRunner.CaptureAsync(
                "git",
                ["-C", _policy.Workspace, "status", "--porcelain", "--", change.RelativePath],
                _policy.Workspace,
                15,
                cancellationToken).ConfigureAwait(false);
            if (result.ExitCode != 0)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(result.Output))
            {
                throw new InvalidOperationException(
                    $"Refusing to overwrite a dirty Git path without --allow-dirty: {change.RelativePath}");
            }
        }
    }

    private async Task<VerificationResult> VerifyAsync(
        VerificationStep step,
        CancellationToken cancellationToken)
    {
        var kind = step.Kind.Trim().ToLowerInvariant();
        if (kind is not ("build" or "test"))
        {
            throw new InvalidOperationException($"Unsupported verification kind '{step.Kind}'.");
        }

        var project = _policy.ResolveForRead(step.Project);
        var extension = Path.GetExtension(project);
        if (!extension.Equals(".csproj", StringComparison.OrdinalIgnoreCase)
            && !extension.Equals(".sln", StringComparison.OrdinalIgnoreCase)
            && !extension.Equals(".slnx", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Verification target must be a .csproj, .sln, or .slnx: {step.Project}");
        }

        var arguments = new List<string> { kind, project, "--no-restore", "--nologo" };
        if (!string.IsNullOrWhiteSpace(step.Framework))
        {
            arguments.Add("--framework");
            arguments.Add(step.Framework);
        }

        var stopwatch = Stopwatch.StartNew();
        var process = await ProcessRunner.CaptureAsync(
            "dotnet",
            arguments,
            _policy.Workspace,
            step.TimeoutSeconds,
            cancellationToken).ConfigureAwait(false);
        stopwatch.Stop();
        return new VerificationResult
        {
            Kind = kind,
            Project = step.Project,
            ExitCode = process.ExitCode,
            DurationSeconds = stopwatch.Elapsed.TotalSeconds,
            Output = LimitOutput(process.CombinedOutput, 32_000)
        };
    }

    private static string LimitOutput(string value, int maxCharacters) =>
        value.Length <= maxCharacters ? value : value[^maxCharacters..];

    private static async Task<HarnessReport> FinishAsync(
        HarnessReport report,
        string? reportPath,
        CancellationToken cancellationToken)
    {
        report.FinishedAtUtc = DateTimeOffset.UtcNow;
        if (!string.IsNullOrWhiteSpace(reportPath))
        {
            var absolutePath = Path.GetFullPath(reportPath);
            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)
                ?? throw new InvalidOperationException("Report path has no parent directory."));
            var json = JsonSerializer.Serialize(report, JsonDefaults.Options);
            await File.WriteAllTextAsync(absolutePath, json, new UTF8Encoding(false), cancellationToken)
                .ConfigureAwait(false);
        }

        return report;
    }

    private sealed class VerificationFailedException : Exception
    {
        internal VerificationFailedException(string message)
            : base(message)
        {
        }
    }
}
