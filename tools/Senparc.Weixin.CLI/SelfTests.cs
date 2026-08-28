using System.Text;

namespace Senparc.Weixin.Cli;

internal static class SelfTests
{
    internal static async Task<int> RunAsync(CancellationToken cancellationToken)
    {
        var root = Path.Combine(Path.GetTempPath(), "weixin-harness-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(root, "src"));
        try
        {
            var target = Path.Combine(root, "src", "sample.txt");
            await File.WriteAllTextAsync(target, "before\n", new UTF8Encoding(false), cancellationToken)
                .ConfigureAwait(false);
            UnixFileMode? originalMode = !OperatingSystem.IsWindows() ? File.GetUnixFileMode(target) : null;
            var bomTarget = Path.Combine(root, "src", "bom.txt");
            var bomPayload = Encoding.UTF8.Preamble.ToArray().Concat(Encoding.UTF8.GetBytes("bom-before\n")).ToArray();
            await File.WriteAllBytesAsync(bomTarget, bomPayload, cancellationToken).ConfigureAwait(false);
            var policy = new WorkspacePolicy(root, new HarnessManifest { AllowedRoots = ["src"] });
            var original = await File.ReadAllBytesAsync(target, cancellationToken).ConfigureAwait(false);
            var originalBom = await File.ReadAllBytesAsync(bomTarget, cancellationToken).ConfigureAwait(false);
            var plan = new HarnessPlan
            {
                Goal = "self test",
                Summary = "replace and create",
                Operations =
                [
                    new FileOperation
                    {
                        Kind = "replace",
                        Path = "src/sample.txt",
                        Replacements =
                        [
                            new TextReplacement { OldText = "before", NewText = "middle" },
                            new TextReplacement { OldText = "middle", NewText = "after" }
                        ],
                        ExpectedSha256 = WorkspacePolicy.ComputeSha256(original)
                    },
                    new FileOperation
                    {
                        Kind = "replace",
                        Path = "src/bom.txt",
                        OldText = "bom-before",
                        NewText = "bom-after",
                        ExpectedSha256 = WorkspacePolicy.ComputeSha256(originalBom)
                    },
                    new FileOperation
                    {
                        Kind = "write",
                        Path = "src/new.txt",
                        Content = "new\n"
                    }
                ]
            };

            var engine = new HarnessEngine(policy);
            var dryRun = await engine.ExecuteAsync(
                plan,
                "self-test",
                new HarnessExecutionOptions(false, true, false, false, null),
                cancellationToken).ConfigureAwait(false);
            Assert(dryRun.Status == "planned", "dry-run status");
            Assert(await File.ReadAllTextAsync(target, cancellationToken).ConfigureAwait(false) == "before\n", "dry-run mutation");

            var applied = await engine.ExecuteAsync(
                plan,
                "self-test",
                new HarnessExecutionOptions(true, true, false, false, null),
                cancellationToken).ConfigureAwait(false);
            Assert(applied.Status == "applied", "apply status");
            Assert(await File.ReadAllTextAsync(target, cancellationToken).ConfigureAwait(false) == "after\n", "replace result");
            Assert(File.Exists(Path.Combine(root, "src", "new.txt")), "new file result");
            var appliedBom = await File.ReadAllBytesAsync(bomTarget, cancellationToken).ConfigureAwait(false);
            Assert(appliedBom.AsSpan().StartsWith(Encoding.UTF8.Preamble), "UTF-8 BOM preservation");
            if (!OperatingSystem.IsWindows() && originalMode.HasValue)
            {
                Assert(File.GetUnixFileMode(target) == originalMode.Value, "Unix file mode preservation");
            }

            AssertThrows(() => policy.ResolveForWrite("../escape.txt"), "path traversal");
            AssertThrows(() => policy.ResolveForWrite("src/key.pem"), "key material");

            var configurationDirectory = Path.Combine(root, "configuration");
            Directory.CreateDirectory(configurationDirectory);
            var baseConfigurationPath = Path.Combine(configurationDirectory, "appsettings.example.json");
            var developmentConfigurationPath = Path.Combine(
                configurationDirectory,
                "appsettings.Development.json");
            await File.WriteAllTextAsync(
                baseConfigurationPath,
                """{"Marker":"base"}""",
                cancellationToken).ConfigureAwait(false);
            await File.WriteAllTextAsync(
                developmentConfigurationPath,
                """{"Marker":"development"}""",
                cancellationToken).ConfigureAwait(false);
            Assert(
                AgentKernelPlanner.BuildConfiguration(baseConfigurationPath, "Development")["Marker"]
                    == "development",
                "Development configuration override");
            Assert(
                AgentKernelPlanner.BuildConfiguration(baseConfigurationPath, null)["Marker"]
                    == "development",
                "local Development configuration fallback");
            Assert(
                AgentKernelPlanner.BuildConfiguration(baseConfigurationPath, "Production")["Marker"]
                    == "base",
                "Production configuration isolation");

            var wrongHash = new HarnessPlan
            {
                Goal = "wrong hash",
                Operations =
                [
                    new FileOperation
                    {
                        Kind = "write",
                        Path = "src/sample.txt",
                        Content = "unsafe",
                        ExpectedSha256 = new string('0', 64)
                    }
                ]
            };
            AssertThrows(() => engine.Prepare(wrongHash, allowDelete: false), "hash mismatch");

            var updated = await File.ReadAllBytesAsync(target, cancellationToken).ConfigureAwait(false);
            var rollbackManifest = new HarnessManifest
            {
                AllowedRoots = ["src"],
                Verification = [new VerificationStep { Kind = "build", Project = "src/missing.csproj" }]
            };
            var rollbackEngine = new HarnessEngine(new WorkspacePolicy(root, rollbackManifest));
            var rollbackPlan = new HarnessPlan
            {
                Goal = "rollback",
                Operations =
                [
                    new FileOperation
                    {
                        Kind = "replace",
                        Path = "src/sample.txt",
                        OldText = "after",
                        NewText = "should-rollback",
                        ExpectedSha256 = WorkspacePolicy.ComputeSha256(updated)
                    }
                ]
            };
            try
            {
                await rollbackEngine.ExecuteAsync(
                    rollbackPlan,
                    "self-test",
                    new HarnessExecutionOptions(true, true, false, false, null),
                    cancellationToken).ConfigureAwait(false);
                throw new InvalidOperationException("Self-test expected verification failure.");
            }
            catch (InvalidOperationException exception) when (
                exception.Message.Contains("does not exist", StringComparison.Ordinal))
            {
                // Expected: verification target validation failed after the plan was applied.
            }

            Assert(await File.ReadAllTextAsync(target, cancellationToken).ConfigureAwait(false) == "after\n", "rollback result");
            Console.WriteLine("Self-test passed.");
            return 0;
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    private static void Assert(bool condition, string name)
    {
        if (!condition)
        {
            throw new InvalidOperationException($"Self-test assertion failed: {name}");
        }
    }

    private static void AssertThrows(Action action, string name)
    {
        try
        {
            action();
            throw new InvalidOperationException($"Self-test expected exception: {name}");
        }
        catch (InvalidOperationException exception) when (
            !exception.Message.StartsWith("Self-test expected", StringComparison.Ordinal))
        {
            // Expected.
        }
    }
}
