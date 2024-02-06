using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace GitHelper;

public class UserInteraction
{
    private static readonly Encoding ConsoleEncoding = Encoding.UTF8;
    private static readonly Style UserInputStyle = new(foreground: Color.Red1);
    private readonly GitFlows _gitFlows;

    static UserInteraction()
    {
        Console.InputEncoding = ConsoleEncoding;
        Console.OutputEncoding = ConsoleEncoding;
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

        var userSelection = ShowSelectionPrompt(
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
        var name = ShowInputPrompt("Name of the branch");
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
                userSelection = ShowSelectionPrompt("Select branch", branchesAsArray);
            }
            catch
            {
                // handle empty selection by user
            }
        } while (!branchesAsArray.Contains(userSelection));

        return userSelection;
    }

    private static string ShowInputPrompt(string title)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>(title)
                .PromptStyle(UserInputStyle));
    }

    private static string ShowSelectionPrompt(string title, string[] choices)
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .AddChoices(choices)
                .PageSize(20)
                .WrapAround()
                .HighlightStyle(UserInputStyle));
    }
}