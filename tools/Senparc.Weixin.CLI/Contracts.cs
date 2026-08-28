using System.Text.Json;
using System.Text.Json.Serialization;

namespace Senparc.Weixin.Cli;

internal static class JsonDefaults
{
    internal static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}

internal sealed class HarnessPlan
{
    public int SchemaVersion { get; set; } = 1;

    public string Goal { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public List<FileOperation> Operations { get; set; } = [];
}

internal sealed class FileOperation
{
    /// <summary>write, replace, or delete.</summary>
    public string Kind { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;

    public string? Content { get; set; }

    public string? OldText { get; set; }

    public string? NewText { get; set; }

    public int ExpectedOccurrences { get; set; } = 1;

    public List<TextReplacement> Replacements { get; set; } = [];

    public string? ExpectedSha256 { get; set; }
}

internal sealed class TextReplacement
{
    public string OldText { get; set; } = string.Empty;

    public string NewText { get; set; } = string.Empty;

    public int ExpectedOccurrences { get; set; } = 1;
}

internal sealed class HarnessManifest
{
    public List<string> AllowedRoots { get; set; } = ["."];

    public List<string> ExcludedPathSegments { get; set; } =
    [
        ".git", ".svn", ".hg", ".vs", ".idea", "bin", "obj", "node_modules"
    ];

    public int MaxOperations { get; set; } = 100;

    public int MaxFileBytes { get; set; } = 1_048_576;

    public List<VerificationStep> Verification { get; set; } = [];
}

internal sealed class VerificationStep
{
    /// <summary>build or test.</summary>
    public string Kind { get; set; } = string.Empty;

    public string Project { get; set; } = string.Empty;

    public string? Framework { get; set; }

    public int TimeoutSeconds { get; set; } = 600;
}

internal sealed class WorkspaceContext
{
    public string Branch { get; set; } = string.Empty;

    public bool IsDirty { get; set; }

    public int SourceFileCount { get; set; }

    public List<string> Projects { get; set; } = [];

    public List<string> Solutions { get; set; } = [];

    public List<string> TopLevelDirectories { get; set; } = [];
}

internal sealed class AgentRequest
{
    public int ProtocolVersion { get; set; } = 1;

    public string Goal { get; set; } = string.Empty;

    public WorkspaceContext Workspace { get; set; } = new();

    public HarnessManifest Policy { get; set; } = new();

    public string ResponseContract { get; set; } = string.Empty;
}

internal sealed class HarnessReport
{
    public string RunId { get; set; } = Guid.NewGuid().ToString("N");

    public DateTimeOffset StartedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? FinishedAtUtc { get; set; }

    public string Goal { get; set; } = string.Empty;

    public string Workspace { get; set; } = string.Empty;

    public string Status { get; set; } = "started";

    public string Planner { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public List<string> ChangedFiles { get; set; } = [];

    public List<VerificationResult> Verification { get; set; } = [];

    public List<string> Diagnostics { get; set; } = [];
}

internal sealed class VerificationResult
{
    public string Kind { get; set; } = string.Empty;

    public string Project { get; set; } = string.Empty;

    public int ExitCode { get; set; }

    public double DurationSeconds { get; set; }

    public string Output { get; set; } = string.Empty;
}

internal sealed record PreparedChange(
    string RelativePath,
    string AbsolutePath,
    byte[]? OriginalContent,
    byte[]? NewContent,
    bool Delete,
    UnixFileMode? OriginalUnixMode);

internal sealed record HarnessExecutionOptions(
    bool Apply,
    bool AllowDirty,
    bool AllowDelete,
    bool KeepFailed,
    string? ReportPath);
