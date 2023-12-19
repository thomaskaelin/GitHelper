namespace GitHelper;

public record GitCliResult
{
    public bool IsSuccess { get; init; }
    public string Command { get; init; }
    public string StandardOutput { get; init; }
    public string ErrorOutput { get; init; }
    public string FormattedMessage => $"[{(IsSuccess ? "OK" : "ERR")}] {Command}";
}