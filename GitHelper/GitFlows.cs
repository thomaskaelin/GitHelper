﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHelper;

public sealed class GitFlows
{
    private const string ActiveBranchMarker = "* ";

    private readonly GitCliWrapper _gitCliWrapper;
    private readonly string _remoteName;

    public GitFlows(IGitConfiguration configuration)
    {
        _gitCliWrapper = new GitCliWrapper(configuration.PathToGitExecutable, configuration.PathToGitRepository);
        _remoteName = configuration.NameOfRemoteServer;
    }

    public async Task FetchAndPullAsync()
    {
        await RunAsync("fetch", _remoteName, "--prune", "--tags", "--force");
        await RunAsync("pull", _remoteName);
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

        await RunAsync("push", _remoteName, "--delete", remoteBranchWithoutPrefix);
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

    public async Task ResetLocalDirectory()
    {
        await RunAsync("reset", "--hard");
    }

    public async Task CleanLocalDirectory()
    {
        await RunAsync("clean", "--force", "-d", "-x");
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

    private string RemoveRemotePrefixFromRemoteBranch(string remoteBranch) =>
        remoteBranch.Replace($"{_remoteName}/", string.Empty);
}