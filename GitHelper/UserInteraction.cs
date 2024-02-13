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
    private readonly Dictionary<string, Func<Task>> _actions;

    static UserInteraction()
    {
        Console.InputEncoding = ConsoleEncoding;
        Console.OutputEncoding = ConsoleEncoding;
    }

    public UserInteraction(GitFlows gitFlows)
    {
        _gitFlows = gitFlows;
        _actions = new Dictionary<string, Func<Task>>
        {
            { "Fetch & Pull", FetchAndPullAsync },
            { "Switch Branch", SwitchBranchAsync },
            { "Checkout Branch", CheckoutBranchAsync },
            { "Create Branch", CreateBranchAsync },
            { "Delete Branch (local)", DeleteLocalBranchAsync },
            { "Delete Branch (remote)", DeleteRemoteBranchAsync },
            { "Quit", () => Task.CompletedTask }
        };
    }

    public async Task RunAsync()
    {
        var actionNames = _actions.Keys.ToArray();
        var userSelection = ShowSelectionPrompt("Select your action", actionNames);

        await _actions[userSelection]();

        if (userSelection != actionNames.Last())
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