using System;
using System.Text;
using System.Threading.Tasks;
using Sharprompt;

namespace GitHelper;

public class UserInteraction
{
    private readonly GitFlows _gitFlows;

    static UserInteraction()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Prompt.ColorSchema.Answer = ConsoleColor.DarkRed;
        Prompt.ColorSchema.Select = ConsoleColor.DarkRed;
    }

    public UserInteraction(GitFlows gitFlows)
    {
        _gitFlows = gitFlows;
    }

    public async Task RunAsync()
    {
        const string fetchAndPull   = "Fetch & Pull";
        const string switchBranch   = "Switch Branch";
        const string checkoutBranch = "Checkout Branch";
        const string createBranch   = "Create Branch";
        const string deleteBranch   = "Delete Branch (locally)";
        const string quit           = "Quit";

        var userSelection = Prompt.Select(
            "Select your action",
            new[]
            {
                fetchAndPull,
                switchBranch,
                checkoutBranch,
                createBranch,
                deleteBranch,
                quit
            });

        switch (userSelection)
        {
            case fetchAndPull:
                await FetchAndPullAsync();
                break;

            case switchBranch:
                await SwitchBranchAsync();
                break;

            case checkoutBranch:
                await CheckoutBranchAsync();
                break;

            case createBranch:
                await CreateBranchAsync();
                break;

            case deleteBranch:
                await DeleteBranchAsync();
                break;
        }

        if (userSelection != quit)
        {
            await RunAsync();
        }
    }

    private async Task FetchAndPullAsync()
    {
        await _gitFlows.FetchAndPullAsync();
    }

    private async Task SwitchBranchAsync()
    {
        var name = await AskUserToSelectLocalBranchAsync();
        await _gitFlows.SwitchBranchAsync(name);
    }

    private async Task CheckoutBranchAsync()
    {
        var name = await AskUserToSelectRemoteBranchAsync();
        await _gitFlows.CheckoutBranchAsync(name);
    }

    private async Task CreateBranchAsync()
    {
        var name = Prompt.Input<string>("Name of the branch");
        await _gitFlows.CreateBranchAsync(name);
    }

    private async Task DeleteBranchAsync()
    {
        var name = await AskUserToSelectLocalBranchAsync();
        await _gitFlows.DeleteBranchAsync(name);
    }

    private async Task<string> AskUserToSelectLocalBranchAsync()
    {
        var branches = await _gitFlows.GetLocalBranchesAsync();
        var userSelection = Prompt.Select("Select branch", branches);

        return userSelection;
    }

    private async Task<string> AskUserToSelectRemoteBranchAsync()
    {
        var branches = await _gitFlows.GetRemoteBranchesAsync();
        var userSelection = Prompt.Select("Select branch", branches);

        return userSelection;
    }
}