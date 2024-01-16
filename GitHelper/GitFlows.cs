using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHelper;

public sealed class GitFlows
{
    private const string RemoteName = "origin";
    private const string RemotePrefix = $"{RemoteName}/";
    private const string ActiveBranchMarker = "* ";

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

    public async Task SwitchBranchAsync(string localBranch)
    {
        await RunAsync("checkout", localBranch);
    }

    public async Task CheckoutBranchAsync(string remoteBranch)
    {
        var localBranch = RemoveRemotePrefixFromRemoteBranch(remoteBranch);

        await RunAsync("checkout", "-b", localBranch, remoteBranch);
    }

    public async Task CreateBranchAsync(string localBranch)
    {
        await RunAsync("checkout", "-b", localBranch);
    }

    public async Task DeleteLocalBranchAsync(string localBranch)
    {
        await RunAsync("branch", "-D", localBranch);
    }

    public async Task DeleteRemoteBranchAsync(string remoteBranch)
    {
        var remoteBranchWithoutPrefix = RemoveRemotePrefixFromRemoteBranch(remoteBranch);

        await RunAsync("push", RemoteName, "--delete", remoteBranchWithoutPrefix);
    }

    public async Task<IEnumerable<string>> GetLocalBranchesAsync()
    {
        var cliResult = await RunAsync(false, "branch");
        var branches = ExtractBranches(cliResult);

        return branches;
    }

    public async Task<IEnumerable<string>> GetRemoteBranchesAsync()
    {
        var cliResult = await RunAsync(false, "branch", "-r");
        var branches = ExtractBranches(cliResult);

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

    private static IOrderedEnumerable<string> ExtractBranches(GitCliResult cliResult)
    {
        return cliResult
            .StandardOutput
            .ReplaceLineEndings()
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(b => b.Replace(ActiveBranchMarker, string.Empty))
            .Select(b => b.Trim())
            .Order();
    }

    private static string RemoveRemotePrefixFromRemoteBranch(string remoteBranch) => remoteBranch.Replace(RemotePrefix, string.Empty);
}