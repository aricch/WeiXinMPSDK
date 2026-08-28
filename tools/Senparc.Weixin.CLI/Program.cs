using System.Text;
using System.Text.Json;

namespace Senparc.Weixin.Cli;

internal static class Program
{
    private const string ResponseContract = """
        {
          "schemaVersion": 1,
          "goal": "the requested goal",
          "summary": "short implementation summary",
          "operations": [
            {
              "kind": "replace",
              "path": "relative/path.cs",
              "replacements": [
                {
                  "oldText": "exact existing text",
                  "newText": "replacement text",
                  "expectedOccurrences": 1
                }
              ],
              "expectedSha256": "sha256 returned by read_file"
            },
            {
              "kind": "write",
              "path": "relative/new-file.cs",
              "content": "complete new UTF-8 file"
            }
          ]
        }
        Allowed kinds are write, replace, and delete. write only creates new files. A replace operation may
        contain several sequential replacements for one existing file. Delete is rejected unless explicitly enabled.
        """;

    public static async Task<int> Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        using var cancellation = new CancellationTokenSource();
        Console.CancelKeyPress += (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cancellation.Cancel();
        };

        try
        {
            var arguments = CliArguments.Parse(args);
            return arguments.Command.ToLowerInvariant() switch
            {
                "inspect" => await InspectAsync(arguments, cancellation.Token).ConfigureAwait(false),
                "harness" => await HarnessAsync(arguments, cancellation.Token).ConfigureAwait(false),
                "verify" => await VerifyAsync(arguments, cancellation.Token).ConfigureAwait(false),
                "self-test" => await SelfTests.RunAsync(cancellation.Token).ConfigureAwait(false),
                "help" or "--help" or "-h" => PrintHelp(),
                _ => throw new ArgumentException($"Unknown command: {arguments.Command}")
            };
        }
        catch (OperationCanceledException)
        {
            Console.Error.WriteLine("Cancelled.");
            return 130;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"ERROR: {exception.Message}");
            return 1;
        }
    }

    private static async Task<int> InspectAsync(CliArguments arguments, CancellationToken cancellationToken)
    {
        var (policy, _) = await CreatePolicyAsync(arguments, cancellationToken).ConfigureAwait(false);
        var context = await new WorkspaceInspector().InspectAsync(policy, cancellationToken).ConfigureAwait(false);
        var json = JsonSerializer.Serialize(context, JsonDefaults.Options);
        await WriteOptionalOutputAsync(arguments.Get("output"), json, cancellationToken).ConfigureAwait(false);
        Console.WriteLine(json);
        return 0;
    }

    private static async Task<int> HarnessAsync(CliArguments arguments, CancellationToken cancellationToken)
    {
        var goal = arguments.Require("goal");
        var (policy, manifest) = await CreatePolicyAsync(arguments, cancellationToken).ConfigureAwait(false);
        AddVerificationOptions(arguments, manifest);
        var context = await new WorkspaceInspector().InspectAsync(policy, cancellationToken).ConfigureAwait(false);
        var request = new AgentRequest
        {
            Goal = goal,
            Workspace = context,
            Policy = manifest,
            ResponseContract = ResponseContract
        };

        var planner = CreatePlanner(arguments, policy);
        var plan = await planner.CreatePlanAsync(request, cancellationToken).ConfigureAwait(false);
        plan.Goal = goal;
        var planJson = JsonSerializer.Serialize(plan, JsonDefaults.Options);
        await WriteOptionalOutputAsync(arguments.Get("plan-output"), planJson, cancellationToken).ConfigureAwait(false);
        Console.WriteLine(planJson);

        var reportPath = ResolveOptionalWorkspaceOutput(policy, arguments.Get("report"));
        var options = new HarnessExecutionOptions(
            Apply: arguments.HasFlag("apply"),
            AllowDirty: arguments.HasFlag("allow-dirty"),
            AllowDelete: arguments.HasFlag("allow-delete"),
            KeepFailed: arguments.HasFlag("keep-failed"),
            ReportPath: reportPath);
        var report = await new HarnessEngine(policy)
            .ExecuteAsync(plan, planner.Name, options, cancellationToken)
            .ConfigureAwait(false);
        Console.WriteLine(JsonSerializer.Serialize(report, JsonDefaults.Options));
        return 0;
    }

    private static async Task<int> VerifyAsync(CliArguments arguments, CancellationToken cancellationToken)
    {
        var (policy, manifest) = await CreatePolicyAsync(arguments, cancellationToken).ConfigureAwait(false);
        AddVerificationOptions(arguments, manifest);
        if (manifest.Verification.Count == 0)
        {
            throw new ArgumentException("verify requires at least one --build or --test option.");
        }

        var plan = new HarnessPlan
        {
            Goal = "Verify workspace",
            Summary = "Run configured deterministic verification steps."
        };
        var reportPath = ResolveOptionalWorkspaceOutput(policy, arguments.Get("report"));
        var report = await new HarnessEngine(policy).ExecuteAsync(
            plan,
            "none",
            new HarnessExecutionOptions(
                Apply: true,
                AllowDirty: true,
                AllowDelete: false,
                KeepFailed: false,
                ReportPath: reportPath),
            cancellationToken).ConfigureAwait(false);
        Console.WriteLine(JsonSerializer.Serialize(report, JsonDefaults.Options));
        return 0;
    }

    private static IHarnessPlanner CreatePlanner(CliArguments arguments, WorkspacePolicy policy)
    {
        var planInput = arguments.Get("plan-input");
        if (!string.IsNullOrWhiteSpace(planInput))
        {
            return new FilePlanner(planInput);
        }

        var agentCommand = arguments.Get("agent-command");
        if (!string.IsNullOrWhiteSpace(agentCommand))
        {
            return new ProcessPlanner(
                agentCommand,
                arguments.GetMany("agent-arg"),
                policy.Workspace,
                ParsePositiveInt(arguments.Get("agent-timeout"), 600, "agent-timeout"));
        }

        var configurationPath = arguments.Get("ai-config")
            ?? throw new ArgumentException(
                "Choose a planner with --plan-input, --agent-command, or --ai-config.");
        return new AgentKernelPlanner(
            configurationPath,
            arguments.Get("model-user") ?? "WeixinHarness",
            new WorkspaceReadTools(policy));
    }

    private static async Task<(WorkspacePolicy Policy, HarnessManifest Manifest)> CreatePolicyAsync(
        CliArguments arguments,
        CancellationToken cancellationToken)
    {
        var workspace = Path.GetFullPath(arguments.Get("workspace") ?? Environment.CurrentDirectory);
        if (!Directory.Exists(workspace))
        {
            throw new DirectoryNotFoundException($"Workspace does not exist: {workspace}");
        }

        HarnessManifest manifest;
        var manifestPath = arguments.Get("manifest");
        if (string.IsNullOrWhiteSpace(manifestPath))
        {
            manifest = new HarnessManifest();
        }
        else
        {
            var json = await File.ReadAllTextAsync(Path.GetFullPath(manifestPath), cancellationToken)
                .ConfigureAwait(false);
            manifest = JsonSerializer.Deserialize<HarnessManifest>(json, JsonDefaults.Options)
                ?? throw new InvalidOperationException("Manifest JSON evaluated to null.");
        }

        ValidateManifest(manifest);
        return (new WorkspacePolicy(workspace, manifest), manifest);
    }

    private static void ValidateManifest(HarnessManifest manifest)
    {
        if (manifest.AllowedRoots.Count == 0)
        {
            throw new InvalidOperationException("Manifest allowedRoots must not be empty.");
        }

        if (manifest.MaxOperations is < 1 or > 1_000)
        {
            throw new InvalidOperationException("Manifest maxOperations must be between 1 and 1000.");
        }

        if (manifest.MaxFileBytes is < 1_024 or > 16_777_216)
        {
            throw new InvalidOperationException("Manifest maxFileBytes must be between 1 KB and 16 MB.");
        }
    }

    private static void AddVerificationOptions(CliArguments arguments, HarnessManifest manifest)
    {
        var framework = arguments.Get("framework");
        var timeout = ParsePositiveInt(arguments.Get("verify-timeout"), 600, "verify-timeout");
        manifest.Verification.AddRange(arguments.GetMany("build").Select(project => new VerificationStep
        {
            Kind = "build",
            Project = project,
            Framework = framework,
            TimeoutSeconds = timeout
        }));
        manifest.Verification.AddRange(arguments.GetMany("test").Select(project => new VerificationStep
        {
            Kind = "test",
            Project = project,
            Framework = framework,
            TimeoutSeconds = timeout
        }));
    }

    private static int ParsePositiveInt(string? value, int fallback, string optionName)
    {
        if (value is null)
        {
            return fallback;
        }

        return int.TryParse(value, out var parsed) && parsed > 0
            ? parsed
            : throw new ArgumentException($"--{optionName} must be a positive integer.");
    }

    private static string? ResolveOptionalWorkspaceOutput(WorkspacePolicy policy, string? path) =>
        string.IsNullOrWhiteSpace(path) ? null : policy.ResolveForWrite(path);

    private static async Task WriteOptionalOutputAsync(
        string? outputPath,
        string content,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(outputPath))
        {
            return;
        }

        var absolutePath = Path.GetFullPath(outputPath);
        Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)
            ?? throw new InvalidOperationException("Output path has no parent directory."));
        await File.WriteAllTextAsync(absolutePath, content, new UTF8Encoding(false), cancellationToken)
            .ConfigureAwait(false);
    }

    private static int PrintHelp()
    {
        Console.WriteLine("""
            Senparc.Weixin CLI — policy-controlled AI development harness

            Commands:
              weixin inspect [--workspace PATH] [--manifest FILE] [--output FILE]
              weixin harness --goal TEXT [planner] [options]
              weixin verify (--build PROJECT | --test PROJECT) [options]
              weixin self-test

            Planners (choose exactly one):
              --ai-config FILE              Senparc.AI.AgentKernel / MAF planner
              --agent-command EXE           JSON stdin/stdout process planner
              --agent-arg VALUE             Repeatable process argument
              --plan-input FILE             Apply or validate an existing plan

            Harness options:
              --workspace PATH              Workspace root (default: current directory)
              --manifest FILE               Policy and verification manifest
              --plan-output FILE            Save generated plan JSON
              --report RELATIVE_PATH        Save structured run report in workspace
              --apply                       Apply after validation (default is dry-run)
              --allow-dirty                 Permit edits to already dirty Git paths
              --allow-delete                Permit delete operations
              --keep-failed                 Do not roll back after verification failure
              --build PROJECT               Repeatable dotnet build verification
              --test PROJECT                Repeatable dotnet test verification
              --framework TFM               Framework for all CLI verification steps

            AI configuration environment variables use the WEIXIN_HARNESS_ prefix and
            double underscores for nested keys. Secrets are never written to reports.
            """);
        return 0;
    }
}
