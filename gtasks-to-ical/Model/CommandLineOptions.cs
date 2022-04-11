using CommandLine;

namespace GTI.Cli.Model
{
    internal class CommandLineOptions
    {
        [Option(shortName: 'j', longName: "jsonInput", Required = true, HelpText = "Path to Google Task JSON export file", Default = "tasks.json")]
        public string JsonInputPath { get; set; }
    }
}
