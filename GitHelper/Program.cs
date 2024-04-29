namespace GitHelper
{
    public static class Program
    {
        public static void Main()
        {
            var configurationManager = new ConfigurationManager();
            if (!configurationManager.LoadConfiguration())
                return;

            var gitFlows = new GitFlows(configurationManager.Configuration);
            var userInteraction = new UserInteraction(configurationManager.Configuration, gitFlows);
            userInteraction.RunAsync().Wait();
        }
    }
}