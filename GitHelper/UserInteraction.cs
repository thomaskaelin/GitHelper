using System;
using System.Collections.Generic;
using System.Linq;
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
        const string fetchAndPull       = "Fetch & Pull";
        const string switchBranch       = "Switch Branch";
        const string checkoutBranch     = "Checkout Branch";
        const string createBranch       = "Create Branch";
        const string deleteLocalBranch  = "Delete Branch (local)";
        const string deleteRemoteBranch = "Delete Branch (remote)";
        const string quit               = "Quit";

        var userSelection = Prompt.Select(
            "Select your action",
            new[]
            {
                fetchAndPull,
                switchBranch,
                checkoutBranch,
                createBranch,
                deleteLocalBranch,
                deleteRemoteBranch,
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

            case deleteLocalBranch:
                await DeleteLocalBranchAsync();
                break;

            case deleteRemoteBranch:
                await DeleteRemoteBranchAsync();
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

    private async Task DeleteLocalBranchAsync()
    {
        var name = await AskUserToSelectLocalBranchAsync();
        await _gitFlows.DeleteLocalBranchAsync(name);
    }

    private async Task DeleteRemoteBranchAsync()
    {
        var name = await AskUserToSelectRemoteBranchAsync();
        await _gitFlows.DeleteRemoteBranchAsync(name);
    }

    private Task<string> AskUserToSelectLocalBranchAsync() => AskUserToSelectBranchAsync(_gitFlows.GetLocalBranchesAsync);

    private Task<string> AskUserToSelectRemoteBranchAsync() => AskUserToSelectBranchAsync(_gitFlows.GetRemoteBranchesAsync);

    private static async Task<string> AskUserToSelectBranchAsync(Func<Task<IEnumerable<string>>> getBranchesAsync)
    {
        string userSelection = null;

        var branchesAsEnumerable = await getBranchesAsync();
        var branchesAsArray = branchesAsEnumerable.ToArray();

        do
        {
            try
            {
                userSelection = Prompt.Select("Select branch", branchesAsArray);
            }
            catch
            {
                // handle empty selection by user
            }
        } while (!branchesAsArray.Contains(userSelection));

        return userSelection;
    }
}