using System.Collections.Generic;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;

namespace GitHelper
{
    public sealed class GitCliWrapper
    {
        private const string GitExecutable = "git";
        private readonly string _workingDirectory;

        public GitCliWrapper(string workingDirectory)
        {
            _workingDirectory = workingDirectory;
        }

        public async Task<GitCliResult> RunAsync(params string[] arguments)
        {
            var libraryResult = await RunWithCliWrap(arguments);
            var internalResult = CreateResult(arguments, libraryResult);

            return internalResult;
        }

        private Task<BufferedCommandResult> RunWithCliWrap(IEnumerable<string> arguments)
        {
            return Cli
                .Wrap(GitExecutable)
                .WithArguments(arguments)
                .WithWorkingDirectory(_workingDirectory)
                .ExecuteBufferedAsync();
        }

        private static GitCliResult CreateResult(string[] arguments, BufferedCommandResult result)
        {
            return new GitCliResult
            {
                IsSuccess = result.ExitCode == 0,
                Command = $"{GitExecutable} {string.Join(' ', arguments)}",
                StandardOutput = result.StandardOutput,
                ErrorOutput = result.StandardError
            };
        }
    }
}