﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace GitHelper;

public class UserInteraction
{
    private readonly GitFlows _gitFlows;
    private readonly Dictionary<string, Func<Task>> _actions;
    private Style _userInputStyle;

    public UserInteraction(
        IConsoleConfiguration configuration,
        GitFlows gitFlows)
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
            { "Reset Local Directory", ResetLocalDirectoryAsync },
            { "Reset & Clean Local Directory", ResetAndCleanLocalDirectoryAsync },
        };

        InitializeConsoleEncoding(configuration);
        InitializeConsoleStyle(configuration);
    }

    public static void ShowError(string message) => AnsiConsole.Markup($"[red]{message}[/]");

    public async Task RunAsync()
    {
        var actionNames = _actions.Keys.ToArray();
        var userSelection = ShowSelectionPrompt("Select your action", actionNames, "Quit");

        if (userSelection.WasCancelled)
            return;

        await _actions[userSelection.Selection]();
        await RunAsync();
    }

    private static void InitializeConsoleEncoding(IConsoleConfiguration configuration)
    {
        var encoding = Encoding.GetEncoding(int.Parse(configuration.EncodingOfConsole));

        Console.InputEncoding = encoding;
        Console.OutputEncoding = encoding;
    }

    private void InitializeConsoleStyle(IConsoleConfiguration configuration)
    {
        var consoleColor = Enum.Parse<ConsoleColor>(configuration.ColorForConsoleInput, true);
        var foregroundColor = Color.FromConsoleColor(consoleColor);

        _userInputStyle = new Style(foreground: foregroundColor);
    }

    private async Task FetchAndPullAsync()
    {
        await _gitFlows.FetchAndPullAsync();
    }

    private async Task SwitchBranchAsync()
    {
        var selection = await AskUserToSelectLocalBranchAsync();

        if (selection.WasCancelled)
            return;

        await _gitFlows.SwitchBranchAsync(selection.Selection);
    }

    private async Task CheckoutBranchAsync()
    {
        var selection = await AskUserToSelectRemoteBranchAsync();

        if (selection.WasCancelled)
            return;

        await _gitFlows.CheckoutBranchAsync(selection.Selection);
    }

    private async Task CreateBranchAsync()
    {
        var name = ShowInputPrompt("Name of the branch");
        await _gitFlows.CreateBranchAsync(name);
    }

    private async Task DeleteLocalBranchAsync()
    {
        var selection = await AskUserToSelectLocalBranchAsync();

        if (selection.WasCancelled)
            return;

        await _gitFlows.DeleteLocalBranchAsync(selection.Selection);
    }

    private async Task DeleteRemoteBranchAsync()
    {
        var selection = await AskUserToSelectRemoteBranchAsync();

        if (selection.WasCancelled)
            return;

        await _gitFlows.DeleteRemoteBranchAsync(selection.Selection);
    }

    private async Task ResetLocalDirectoryAsync()
    {
        await _gitFlows.ResetLocalDirectory();
    }

    private async Task ResetAndCleanLocalDirectoryAsync()
    {
        await _gitFlows.ResetLocalDirectory();
        await _gitFlows.CleanLocalDirectory();
    }

    private Task<SelectionResult> AskUserToSelectLocalBranchAsync() =>
        AskUserToSelectBranchAsync(_gitFlows.GetLocalBranchesAsync);

    private Task<SelectionResult> AskUserToSelectRemoteBranchAsync() =>
        AskUserToSelectBranchAsync(_gitFlows.GetRemoteBranchesAsync);

    private async Task<SelectionResult> AskUserToSelectBranchAsync(
        Func<Task<IEnumerable<string>>> getBranchesAsync)
    {
        var branchesAsEnumerable = await getBranchesAsync();
        var branchesAsArray = branchesAsEnumerable.ToArray();
        var selectionResult = ShowSelectionPrompt("Select branch", branchesAsArray, "-- cancel --");

        return selectionResult;
    }

    private string ShowInputPrompt(string title)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>(title)
                .PromptStyle(_userInputStyle));
    }

    private SelectionResult ShowSelectionPrompt(string title, string[] choices, string cancelChoice = null)
    {
        if (!string.IsNullOrEmpty(cancelChoice))
            choices = new List<string>(choices) { cancelChoice }.ToArray();

        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .AddChoices(choices)
                .EnableSearch()
                .PageSize(20)
                .WrapAround()
                .HighlightStyle(_userInputStyle));

        var selectionResult = new SelectionResult
        {
            Selection = selection,
            WasCancelled = selection == cancelChoice
        };

        return selectionResult;
    }

    private class SelectionResult
    {
        internal string Selection { get; init; }
        internal bool WasCancelled { get; init; }
    }
}