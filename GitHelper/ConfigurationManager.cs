using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace GitHelper;

public class ConfigurationManager
{
    private const string NameOfConfigurationFile = "settings.json";

    private static string PathToConfigurationFile => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NameOfConfigurationFile);

    private static readonly Encoding EncodingOfConfigurationFile = Encoding.UTF8;

    public Configuration Configuration { get; private set; } = new();

    public bool LoadConfiguration()
    {
        try
        {
            LoadConfigurationFromFile();
            return true;
        }
        catch
        {
            CreateExampleConfiguration();
            SaveConfigurationToFile();
            ShowErrorMessage();
            return false;
        }
    }

    private void LoadConfigurationFromFile()
    {
        var configAsJson = File.ReadAllText(PathToConfigurationFile, EncodingOfConfigurationFile);
        Configuration = JsonConvert.DeserializeObject<Configuration>(configAsJson);
    }

    private void SaveConfigurationToFile()
    {
        var serializedConfiguration = JsonConvert.SerializeObject(Configuration, Formatting.Indented);
        File.WriteAllText(PathToConfigurationFile, serializedConfiguration, EncodingOfConfigurationFile);
    }

    private void CreateExampleConfiguration()
    {
        Configuration = new Configuration
        {
            PathToGitExecutable = "git",
            PathToGitRepository = @"C:\path\to\your\git\repo",
            NameOfRemoteServer = "origin",
            EncodingOfConsole = Encoding.UTF8.CodePage.ToString(),
            ColorForConsoleInput = ConsoleColor.Red.ToString()
        };
    }

    private static void ShowErrorMessage()
    {
        const string message = $"No config file found. A file called '{NameOfConfigurationFile}' was created in the execution directory. Please make sure to set the values accordingly.";
        UserInteraction.ShowError(message);
    }
}