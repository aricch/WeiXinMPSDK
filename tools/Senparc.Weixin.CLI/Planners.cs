using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Senparc.AI.AgentKernel;
using Senparc.AI.AgentKernel.Handlers;

namespace Senparc.Weixin.Cli;

internal interface IHarnessPlanner
{
    string Name { get; }

    Task<HarnessPlan> CreatePlanAsync(AgentRequest request, CancellationToken cancellationToken);
}

internal sealed class FilePlanner : IHarnessPlanner
{
    private readonly string _planPath;

    internal FilePlanner(string planPath)
    {
        _planPath = Path.GetFullPath(planPath);
    }

    public string Name => "file";

    public async Task<HarnessPlan> CreatePlanAsync(AgentRequest request, CancellationToken cancellationToken)
    {
        var json = await File.ReadAllTextAsync(_planPath, cancellationToken).ConfigureAwait(false);
        return PlanJson.Parse(json, request.Goal);
    }
}

internal sealed class ProcessPlanner : IHarnessPlanner
{
    private readonly string _executable;
    private readonly IReadOnlyList<string> _arguments;
    private readonly string _workingDirectory;
    private readonly int _timeoutSeconds;

    internal ProcessPlanner(
        string executable,
        IReadOnlyList<string> arguments,
        string workingDirectory,
        int timeoutSeconds)
    {
        _executable = executable;
        _arguments = arguments;
        _workingDirectory = workingDirectory;
        _timeoutSeconds = timeoutSeconds;
    }

    public string Name => "process";

    public async Task<HarnessPlan> CreatePlanAsync(AgentRequest request, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Serialize(request, JsonDefaults.Options);
        var result = await ProcessRunner.CaptureAsync(
            _executable,
            _arguments,
            _workingDirectory,
            _timeoutSeconds,
            cancellationToken,
            input).ConfigureAwait(false);
        if (result.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"Agent process exited with code {result.ExitCode}: {Limit(result.CombinedOutput, 4_000)}");
        }

        if (result.Output.Length > 4_000_000)
        {
            throw new InvalidOperationException("Agent response exceeded the 4 MB protocol limit.");
        }

        return PlanJson.Parse(result.Output, request.Goal);
    }

    private static string Limit(string value, int maxCharacters) =>
        value.Length <= maxCharacters ? value : value[^maxCharacters..];
}

internal sealed class AgentKernelPlanner : IHarnessPlanner
{
    private const string SystemPrompt = """
        You are the planning agent inside Senparc.Weixin CLI Harness.
        Inspect the workspace only through the supplied read-only tools. Never invent file content or hashes.
        Return one JSON object only, with no Markdown fence and no explanatory text.
        Existing files must use replace or delete and must include the exact SHA-256 returned by read_file.
        New files use write and must omit expectedSha256. Prefer small, reviewable changes.
        Do not generate shell commands, package credentials, certificates, or secret values.
        The local harness will enforce policy, apply atomically, run separately configured verification, and roll back failures.
        """;

    private readonly string _configurationPath;
    private readonly string _modelUserId;
    private readonly WorkspaceReadTools _workspaceTools;

    internal AgentKernelPlanner(
        string configurationPath,
        string modelUserId,
        WorkspaceReadTools workspaceTools)
    {
        _configurationPath = Path.GetFullPath(configurationPath);
        _modelUserId = modelUserId;
        _workspaceTools = workspaceTools;
    }

    public string Name => "agentkernel-maf";

    public async Task<HarnessPlan> CreatePlanAsync(AgentRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var configuration = BuildConfiguration(_configurationPath, ResolveEnvironmentName());
        var setting = configuration.GetSection("SenparcAiSetting").Get<SenparcAiSetting>()
            ?? throw new InvalidOperationException(
                $"Missing SenparcAiSetting in {_configurationPath}. See appsettings.example.json.");

        var tools = new List<AITool>
        {
            AIFunctionFactory.Create(
                (string glob, int maxResults) => _workspaceTools.ListFiles(glob, maxResults),
                "list_files",
                "List workspace-relative file paths. Use glob patterns such as **/*.cs."),
            AIFunctionFactory.Create(
                (string path, int startLine, int lineCount) => _workspaceTools.ReadFile(path, startLine, lineCount),
                "read_file",
                "Read a UTF-8 file range. The first line contains the SHA-256 required for modifications."),
            AIFunctionFactory.Create(
                (string query, string glob, int maxResults) => _workspaceTools.SearchText(query, glob, maxResults),
                "search_text",
                "Search text in workspace files and return path, line number, and matching line.")
        };

        var chatOptions = new ChatClientAgentOptions
        {
            ChatOptions = new ChatOptions
            {
                Instructions = SystemPrompt,
                Temperature = 0,
                TopP = 0.5f,
                MaxOutputTokens = 16_000,
                Tools = tools
            }
        };

        var aiHandler = new AgentAiHandler(setting);
        var runnable = await aiHandler.IWantTo(setting)
            .ConfigChatModel(_modelUserId, chatOptions)
            .BuildKernelWithAgentSessionAsync()
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);
        var session = runnable.Kernel.AgentSession
            ?? throw new InvalidOperationException("AgentKernel did not create an AgentSession.");
        var prompt = BuildPrompt(request);
        var result = await runnable.RunChatAsync(prompt, session)
            .WaitAsync(cancellationToken)
            .ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        return PlanJson.Parse(result.OutputString, request.Goal);
    }

    internal static IConfigurationRoot BuildConfiguration(
        string configurationPath,
        string? environmentName)
    {
        var baseConfigurationPath = Path.GetFullPath(configurationPath);
        var developmentConfigurationPath = Path.Combine(
            Path.GetDirectoryName(baseConfigurationPath) ?? Environment.CurrentDirectory,
            "appsettings.Development.json");
        var hasExplicitEnvironment = !string.IsNullOrWhiteSpace(environmentName);
        var shouldLoadDevelopmentConfiguration = File.Exists(developmentConfigurationPath)
            && (!hasExplicitEnvironment
                || string.Equals(environmentName, "Development", StringComparison.OrdinalIgnoreCase));

        var builder = new ConfigurationBuilder()
            .AddJsonFile(baseConfigurationPath, optional: false, reloadOnChange: false);
        if (shouldLoadDevelopmentConfiguration
            && !string.Equals(
                baseConfigurationPath,
                developmentConfigurationPath,
                OperatingSystem.IsWindows()
                    ? StringComparison.OrdinalIgnoreCase
                    : StringComparison.Ordinal))
        {
            builder.AddJsonFile(
                developmentConfigurationPath,
                optional: false,
                reloadOnChange: false);
        }

        return builder
            .AddEnvironmentVariables(prefix: "WEIXIN_HARNESS_")
            .Build();
    }

    private static string? ResolveEnvironmentName()
    {
        var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        if (!string.IsNullOrWhiteSpace(environmentName))
        {
            return environmentName;
        }

        environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        return string.IsNullOrWhiteSpace(environmentName) ? null : environmentName;
    }

    private static string BuildPrompt(AgentRequest request)
    {
        return """
            Create a minimal implementation plan for the following development goal.
            Use list_files, search_text, and read_file until every proposed edit is grounded in current source.

            GOAL:
            """ + request.Goal + """


            WORKSPACE INVENTORY:
            """ + JsonSerializer.Serialize(request.Workspace, JsonDefaults.Options) + """


            POLICY:
            """ + JsonSerializer.Serialize(request.Policy, JsonDefaults.Options) + """


            REQUIRED RESPONSE CONTRACT:
            """ + request.ResponseContract;
    }
}

internal static class PlanJson
{
    internal static HarnessPlan Parse(string response, string fallbackGoal)
    {
        if (string.IsNullOrWhiteSpace(response))
        {
            throw new InvalidOperationException("Planner returned an empty response.");
        }

        var json = ExtractObject(response.Trim());
        try
        {
            var plan = JsonSerializer.Deserialize<HarnessPlan>(json, JsonDefaults.Options)
                ?? throw new InvalidOperationException("Planner returned JSON null.");
            if (string.IsNullOrWhiteSpace(plan.Goal))
            {
                plan.Goal = fallbackGoal;
            }

            return plan;
        }
        catch (JsonException exception)
        {
            throw new InvalidOperationException(
                $"Planner response is not a valid HarnessPlan: {exception.Message}", exception);
        }
    }

    private static string ExtractObject(string response)
    {
        if (response.StartsWith("```", StringComparison.Ordinal))
        {
            var firstNewLine = response.IndexOf('\n');
            var lastFence = response.LastIndexOf("```", StringComparison.Ordinal);
            if (firstNewLine > 0 && lastFence > firstNewLine)
            {
                response = response[(firstNewLine + 1)..lastFence].Trim();
            }
        }

        var start = response.IndexOf('{');
        var end = response.LastIndexOf('}');
        return start >= 0 && end > start ? response[start..(end + 1)] : response;
    }
}
