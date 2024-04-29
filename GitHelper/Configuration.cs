using System;

namespace GitHelper;

public class Configuration : IGitConfiguration, IConsoleConfiguration
{
    public string PathToGitExecutable { get; init; }

    public string PathToGitRepository { get; init; }

    public string NameOfRemoteServer { get; init; }

    public string EncodingOfConsole { get; init; }

    public string ColorForConsoleInput { get; init; }
}

public interface IGitConfiguration
{
    public string PathToGitExecutable { get; }

    public string PathToGitRepository { get; }

    public string NameOfRemoteServer { get; }
}

public interface IConsoleConfiguration
{
    public string EncodingOfConsole { get; }

    public string ColorForConsoleInput { get; }
}