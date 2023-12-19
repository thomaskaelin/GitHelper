using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHelper;

public sealed class GitFlows
{
    private readonly GitCliWrapper _gitCliWrapper;

    public GitFlows(string workingDirectory)
    {
        _gitCliWrapper = new GitCliWrapper(workingDirectory);
    }

    public async Task FetchAndPullAsync()
    {
        await RunAsync("fetch", "--tags", "--force");
        await RunAsync("pull");
    }

    public async Task SwitchBranchAsync(string branch)
    {
        await RunAsync("checkout", branch);
    }

    public async Task CreateBranchAsync(string branch)
    {
        await RunAsync("checkout", "-b", branch);
    }

    public async Task DeleteBranchAsync(string branch)
    {
        await RunAsync("branch", "-D", branch);
    }

    public async Task<IEnumerable<string>> GetLocalBranchesAsync()
    {
        var cliResult = await RunAsync(false, "branch");

        var branches = cliResult
            .StandardOutput
            .ReplaceLineEndings()
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(b => b.Replace("* ", string.Empty))
            .Select(b => b.Trim())
            .Order();

        return branches;
    }

    private async Task<GitCliResult> RunAsync(bool printOutput, params string[] arguments)
    {
        var cliResult = await _gitCliWrapper.RunAsync(arguments);

        if (printOutput)
            Console.WriteLine(cliResult.FormattedMessage);

        return cliResult;
    }

    private Task<GitCliResult> RunAsync(params string[] arguments) => RunAsync(true, arguments);
}