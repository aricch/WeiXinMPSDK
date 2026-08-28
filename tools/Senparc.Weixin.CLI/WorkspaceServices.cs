using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Senparc.Weixin.Cli;

internal sealed class WorkspacePolicy
{
    private static readonly HashSet<string> ProtectedFileNames = new(StringComparer.OrdinalIgnoreCase)
    {
        ".env", ".env.local", "id_rsa", "id_ed25519", "secrets.json",
        "appsettings.production.json", "appsettings.development.json", "appsettings.local.json",
        "credentials", "credentials.json"
    };

    private static readonly HashSet<string> ProtectedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pfx", ".p12", ".pem", ".key", ".snk"
    };

    private readonly string _rootWithSeparator;
    private readonly StringComparison _pathComparison;

    internal WorkspacePolicy(string workspace, HarnessManifest manifest)
    {
        Workspace = Path.GetFullPath(workspace);
        Manifest = manifest;
        _rootWithSeparator = Workspace.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            + Path.DirectorySeparatorChar;
        _pathComparison = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
    }

    internal string Workspace { get; }

    internal HarnessManifest Manifest { get; }

    internal string ResolveForRead(string relativePath)
    {
        var absolutePath = Resolve(relativePath);
        if (!File.Exists(absolutePath))
        {
            throw new InvalidOperationException($"File does not exist: {relativePath}");
        }

        RejectLinks(absolutePath);
        return absolutePath;
    }

    internal string ResolveForWrite(string relativePath)
    {
        var absolutePath = Resolve(relativePath);
        RejectLinks(absolutePath);
        return absolutePath;
    }

    internal string ToRelative(string absolutePath) =>
        NormalizeRelative(Path.GetRelativePath(Workspace, absolutePath));

    internal static string ComputeSha256(byte[] content) =>
        Convert.ToHexStringLower(SHA256.HashData(content));

    private string Resolve(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath) || Path.IsPathRooted(relativePath))
        {
            throw new InvalidOperationException($"Path must be workspace-relative: {relativePath}");
        }

        var normalized = NormalizeRelative(relativePath);
        var segments = normalized.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0 || segments.Any(segment => segment is "." or ".."))
        {
            throw new InvalidOperationException($"Path traversal is not allowed: {relativePath}");
        }

        if (segments.Any(segment => Manifest.ExcludedPathSegments.Contains(segment, StringComparer.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"Path is excluded by policy: {relativePath}");
        }

        var fileName = segments[^1];
        if (ProtectedFileNames.Contains(fileName) || ProtectedExtensions.Contains(Path.GetExtension(fileName)))
        {
            throw new InvalidOperationException($"Secret or key material is protected: {relativePath}");
        }

        if (!IsUnderAllowedRoot(normalized))
        {
            throw new InvalidOperationException($"Path is outside allowed roots: {relativePath}");
        }

        var absolutePath = Path.GetFullPath(Path.Combine(Workspace, normalized));
        if (!absolutePath.StartsWith(_rootWithSeparator, _pathComparison))
        {
            throw new InvalidOperationException($"Path escapes the workspace: {relativePath}");
        }

        return absolutePath;
    }

    private bool IsUnderAllowedRoot(string relativePath)
    {
        foreach (var root in Manifest.AllowedRoots)
        {
            var normalizedRoot = NormalizeRelative(root).Trim('/');
            if (normalizedRoot is "" or ".")
            {
                return true;
            }

            if (relativePath.Equals(normalizedRoot, _pathComparison)
                || relativePath.StartsWith(normalizedRoot + "/", _pathComparison))
            {
                return true;
            }
        }

        return false;
    }

    private void RejectLinks(string absolutePath)
    {
        var currentPath = File.Exists(absolutePath)
            ? Path.GetDirectoryName(absolutePath)
            : Path.GetDirectoryName(absolutePath) ?? Workspace;

        while (currentPath is not null && !Directory.Exists(currentPath))
        {
            currentPath = Path.GetDirectoryName(currentPath);
        }

        var current = currentPath is null ? null : new DirectoryInfo(currentPath);

        if (File.Exists(absolutePath) && IsLink(new FileInfo(absolutePath)))
        {
            throw new InvalidOperationException($"Symbolic links are not writable: {ToRelative(absolutePath)}");
        }

        while (current is not null && current.FullName.StartsWith(_rootWithSeparator, _pathComparison))
        {
            if (IsLink(current))
            {
                throw new InvalidOperationException($"Path traverses a symbolic link: {ToRelative(absolutePath)}");
            }

            current = current.Parent;
        }
    }

    private static bool IsLink(FileSystemInfo info) =>
        info.LinkTarget is not null || info.Attributes.HasFlag(FileAttributes.ReparsePoint);

    internal static string NormalizeRelative(string path) => path.Replace('\\', '/').TrimStart('/');
}

internal sealed class WorkspaceInspector
{
    private static readonly HashSet<string> SourceExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".cs", ".csproj", ".props", ".targets", ".json", ".xml", ".ts", ".js", ".md"
    };

    internal async Task<WorkspaceContext> InspectAsync(
        WorkspacePolicy policy,
        CancellationToken cancellationToken)
    {
        var files = EnumerateFiles(policy).Take(100_000).ToList();
        var git = await ReadGitStateAsync(policy.Workspace, cancellationToken).ConfigureAwait(false);

        return new WorkspaceContext
        {
            Branch = git.Branch,
            IsDirty = git.IsDirty,
            SourceFileCount = files.Count(path => SourceExtensions.Contains(Path.GetExtension(path))),
            Projects = files
                .Where(path => path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
                .Select(policy.ToRelative)
                .Order(StringComparer.Ordinal)
                .Take(500)
                .ToList(),
            Solutions = files
                .Where(path => path.EndsWith(".sln", StringComparison.OrdinalIgnoreCase)
                    || path.EndsWith(".slnx", StringComparison.OrdinalIgnoreCase))
                .Select(policy.ToRelative)
                .Order(StringComparer.Ordinal)
                .Take(100)
                .ToList(),
            TopLevelDirectories = Directory.EnumerateDirectories(policy.Workspace)
                .Select(Path.GetFileName)
                .Where(name => !string.IsNullOrEmpty(name)
                    && !policy.Manifest.ExcludedPathSegments.Contains(name, StringComparer.OrdinalIgnoreCase))
                .Cast<string>()
                .Order(StringComparer.Ordinal)
                .ToList()
        };
    }

    internal static IEnumerable<string> EnumerateFiles(WorkspacePolicy policy)
    {
        var pending = new Stack<string>();
        pending.Push(policy.Workspace);

        while (pending.TryPop(out var directory))
        {
            IEnumerable<string> childDirectories;
            IEnumerable<string> files;
            try
            {
                childDirectories = Directory.EnumerateDirectories(directory);
                files = Directory.EnumerateFiles(directory);
            }
            catch (UnauthorizedAccessException)
            {
                continue;
            }

            foreach (var file in files)
            {
                yield return file;
            }

            foreach (var child in childDirectories)
            {
                var name = Path.GetFileName(child);
                if (policy.Manifest.ExcludedPathSegments.Contains(name, StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                var info = new DirectoryInfo(child);
                if (!info.Attributes.HasFlag(FileAttributes.ReparsePoint) && info.LinkTarget is null)
                {
                    pending.Push(child);
                }
            }
        }
    }

    private static async Task<(string Branch, bool IsDirty)> ReadGitStateAsync(
        string workspace,
        CancellationToken cancellationToken)
    {
        try
        {
            var branch = await ProcessRunner.CaptureAsync(
                "git", ["-C", workspace, "branch", "--show-current"], workspace, 15, cancellationToken)
                .ConfigureAwait(false);
            var status = await ProcessRunner.CaptureAsync(
                "git", ["-C", workspace, "status", "--porcelain", "--untracked-files=no"], workspace, 30, cancellationToken)
                .ConfigureAwait(false);
            return (branch.Output.Trim(), !string.IsNullOrWhiteSpace(status.Output));
        }
        catch (Exception exception) when (exception is InvalidOperationException or System.ComponentModel.Win32Exception)
        {
            return (string.Empty, false);
        }
    }
}

internal sealed class WorkspaceReadTools
{
    private static readonly HashSet<string> SearchableExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".cs", ".csproj", ".props", ".targets", ".json", ".xml", ".md", ".ts", ".js", ".yml", ".yaml"
    };

    private readonly WorkspacePolicy _policy;

    internal WorkspaceReadTools(WorkspacePolicy policy)
    {
        _policy = policy;
    }

    public string ListFiles(string glob = "**", int maxResults = 100)
    {
        maxResults = Math.Clamp(maxResults, 1, 500);
        var matcher = GlobToRegex(glob);
        var matches = WorkspaceInspector.EnumerateFiles(_policy)
            .Select(_policy.ToRelative)
            .Where(path => matcher.IsMatch(path))
            .Order(StringComparer.Ordinal)
            .Take(maxResults)
            .ToList();
        return string.Join('\n', matches);
    }

    public string ReadFile(string path, int startLine = 1, int lineCount = 200)
    {
        var absolutePath = _policy.ResolveForRead(path);
        var bytes = File.ReadAllBytes(absolutePath);
        if (bytes.Length > _policy.Manifest.MaxFileBytes)
        {
            return $"ERROR: file exceeds {_policy.Manifest.MaxFileBytes} bytes";
        }

        startLine = Math.Max(startLine, 1);
        lineCount = Math.Clamp(lineCount, 1, 500);
        string text;
        try
        {
            text = new UTF8Encoding(false, true).GetString(bytes);
        }
        catch (DecoderFallbackException)
        {
            return "ERROR: only valid UTF-8 text files can be read";
        }

        var lines = text.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n');
        var selected = lines.Skip(startLine - 1).Take(lineCount)
            .Select((line, index) => $"{startLine + index,6}: {SensitiveText.Redact(line)}");
        return $"sha256={WorkspacePolicy.ComputeSha256(bytes)}\n" + string.Join('\n', selected);
    }

    public string SearchText(string query, string glob = "**", int maxResults = 100)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length > 500)
        {
            return "ERROR: query must contain 1-500 characters";
        }

        maxResults = Math.Clamp(maxResults, 1, 500);
        var matcher = GlobToRegex(glob);
        var results = new List<string>();
        foreach (var file in WorkspaceInspector.EnumerateFiles(_policy))
        {
            if (!SearchableExtensions.Contains(Path.GetExtension(file)))
            {
                continue;
            }

            var relative = _policy.ToRelative(file);
            if (!matcher.IsMatch(relative))
            {
                continue;
            }

            string readablePath;
            try
            {
                readablePath = _policy.ResolveForRead(relative);
            }
            catch (InvalidOperationException)
            {
                continue;
            }

            if (new FileInfo(readablePath).Length > _policy.Manifest.MaxFileBytes)
            {
                continue;
            }

            var lineNumber = 0;
            foreach (var line in File.ReadLines(readablePath))
            {
                lineNumber++;
                if (line.Contains(query, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add($"{relative}:{lineNumber}:{SensitiveText.Redact(line.Trim())}");
                    if (results.Count >= maxResults)
                    {
                        return string.Join('\n', results);
                    }
                }
            }
        }

        return string.Join('\n', results);
    }

    private static Regex GlobToRegex(string glob)
    {
        var normalized = WorkspacePolicy.NormalizeRelative(string.IsNullOrWhiteSpace(glob) ? "*" : glob);
        var expression = Regex.Escape(normalized)
            .Replace("\\*\\*/", "(?:.*/)?", StringComparison.Ordinal)
            .Replace("\\*\\*", ".*", StringComparison.Ordinal)
            .Replace("\\*", "[^/]*", StringComparison.Ordinal)
            .Replace("\\?", "[^/]", StringComparison.Ordinal);
        return new Regex("^" + expression + "$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, TimeSpan.FromSeconds(1));
    }
}

internal static class ProcessRunner
{
    internal static async Task<ProcessResult> CaptureAsync(
        string executable,
        IReadOnlyList<string> arguments,
        string workingDirectory,
        int timeoutSeconds,
        CancellationToken cancellationToken,
        string? standardInput = null)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = executable,
            WorkingDirectory = workingDirectory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = standardInput is not null,
            CreateNoWindow = true
        };
        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = new Process { StartInfo = startInfo };
        if (!process.Start())
        {
            throw new InvalidOperationException($"Could not start process: {executable}");
        }

        if (standardInput is not null)
        {
            try
            {
                await process.StandardInput.WriteAsync(standardInput.AsMemory(), cancellationToken).ConfigureAwait(false);
                process.StandardInput.Close();
            }
            catch
            {
                TryKill(process);
                throw;
            }
        }

        var outputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
        var errorTask = process.StandardError.ReadToEndAsync(cancellationToken);
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(Math.Clamp(timeoutSeconds, 1, 3600)));
        try
        {
            await process.WaitForExitAsync(timeout.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            TryKill(process);
            throw new TimeoutException($"Process timed out after {timeoutSeconds} seconds: {executable}");
        }
        catch (OperationCanceledException)
        {
            TryKill(process);
            throw;
        }

        var output = await outputTask.ConfigureAwait(false);
        var error = await errorTask.ConfigureAwait(false);
        return new ProcessResult(process.ExitCode, output.TrimEnd(), error.TrimEnd());
    }

    private static void TryKill(Process process)
    {
        try
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
            }
        }
        catch (InvalidOperationException)
        {
            // The process exited between HasExited and Kill.
        }
    }
}

internal sealed record ProcessResult(int ExitCode, string Output, string Error)
{
    internal string CombinedOutput => string.IsNullOrEmpty(Error)
        ? Output
        : Output + Environment.NewLine + Error;
}

internal static partial class SensitiveText
{
    [GeneratedRegex(
        "(?i)([\\\"]?(?:api[-_ ]?key|client[-_ ]?secret|app[-_ ]?secret|password|connection[-_ ]?string)[\\\"]?\\s*[=:]\\s*[\\\"])([^\\\"]+)([\\\"])",
        RegexOptions.CultureInvariant,
        matchTimeoutMilliseconds: 1000)]
    private static partial Regex SensitiveValuePattern();

    internal static string Redact(string value) =>
        SensitiveValuePattern().Replace(value, "$1[REDACTED]$3");
}
