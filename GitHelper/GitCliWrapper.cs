using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;

namespace GitHelper
{
    public sealed class GitCliWrapper
    {
        private readonly string _gitExecutable;
        private readonly string _workingDirectory;

        public GitCliWrapper(string gitExecutable, string workingDirectory)
        {
            _gitExecutable = gitExecutable;
            _workingDirectory = workingDirectory;
        }

        public async Task<GitCliResult> RunAsync(params string[] arguments)
        {
            try
            {
                var libraryResult = await RunWithCliWrap(arguments);
                var internalResult = CreateResult(arguments, libraryResult);

                return internalResult;
            }
            catch (Exception e)
            {
                return CreateResult(arguments, e);
            }
        }

        private Task<BufferedCommandResult> RunWithCliWrap(IEnumerable<string> arguments)
        {
            return Cli
                .Wrap(_gitExecutable)
                .WithArguments(arguments)
                .WithWorkingDirectory(_workingDirectory)
                .ExecuteBufferedAsync();
        }

        private GitCliResult CreateResult(string[] arguments, BufferedCommandResult result)
        {
            return new GitCliResult
            {
                IsSuccess = result.ExitCode == 0,
                Command = FormatArguments(arguments),
                StandardOutput = result.StandardOutput,
                ErrorOutput = result.StandardError
            };
        }

        private GitCliResult CreateResult(string[] arguments, Exception exception)
        {
            return new GitCliResult
            {
                IsSuccess = false,
                Command = FormatArguments(arguments),
                StandardOutput = string.Empty,
                ErrorOutput = exception.Message
            };
        }

        private string FormatArguments(string[] arguments) => $"{_gitExecutable} {string.Join(' ', arguments)}";
    }
}