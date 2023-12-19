using System.Diagnostics.CodeAnalysis;
using CommandLine;

namespace GitHelper
{
    public static class Program
    {
        // ReSharper disable once ClassNeverInstantiated.Local
        private class Options
        {
            [Option('p', "path", Required = true, HelpText = "Path to the local Git-repository")]
            public string Path { get; set; }
        }

        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Options))]
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    var gitFlows = new GitFlows(o.Path);
                    var userInteraction = new UserInteraction(gitFlows);
                    userInteraction.RunAsync().Wait();
                });
        }
    }
}