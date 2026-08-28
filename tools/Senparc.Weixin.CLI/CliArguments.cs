namespace Senparc.Weixin.Cli;

internal sealed class CliArguments
{
    private readonly Dictionary<string, List<string>> _options = new(StringComparer.OrdinalIgnoreCase);

    private readonly HashSet<string> _flags = new(StringComparer.OrdinalIgnoreCase);

    private CliArguments(string command)
    {
        Command = command;
    }

    public string Command { get; }

    public static CliArguments Parse(string[] args)
    {
        var command = args.Length == 0 || args[0].StartsWith('-') ? "help" : args[0];
        var parsed = new CliArguments(command);
        var start = command == "help" && (args.Length == 0 || args[0].StartsWith('-')) ? 0 : 1;

        for (var index = start; index < args.Length; index++)
        {
            var token = args[index];
            if (!token.StartsWith("--", StringComparison.Ordinal))
            {
                throw new ArgumentException($"Unexpected argument: {token}");
            }

            var key = token[2..];
            if (index + 1 >= args.Length || args[index + 1].StartsWith("--", StringComparison.Ordinal))
            {
                parsed._flags.Add(key);
                continue;
            }

            var value = args[++index];
            if (!parsed._options.TryGetValue(key, out var values))
            {
                values = [];
                parsed._options[key] = values;
            }

            values.Add(value);
        }

        return parsed;
    }

    public bool HasFlag(string name) => _flags.Contains(name);

    public string? Get(string name) =>
        _options.TryGetValue(name, out var values) ? values[^1] : null;

    public string Require(string name) =>
        Get(name) ?? throw new ArgumentException($"Missing required option --{name}.");

    public IReadOnlyList<string> GetMany(string name) =>
        _options.TryGetValue(name, out var values) ? values : [];
}
