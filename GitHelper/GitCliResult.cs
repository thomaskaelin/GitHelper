using System.Text;

namespace GitHelper;

public record GitCliResult
{
    public bool IsSuccess { get; init; }

    public string Command { get; init; }

    public string StandardOutput { get; init; }

    public string ErrorOutput { get; init; }

    public string FormattedMessage
    {
        get
        {
            const string indent = "   ";

            var result = new StringBuilder();

            result.AppendLine();
            result.AppendLine($"{indent}Command: {Command}");
            result.AppendLine($"{indent}Status:  {(IsSuccess ? "\u2705" : "\u26d4")}  "); // important: do not remove whitespaces at the end

            if (!IsSuccess)
                result.AppendLine($"{indent}Message: {ErrorOutput.ReplaceLineEndings(" | ")}");

            return result.ToString();
        }
    }
}