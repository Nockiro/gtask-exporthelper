using CommandLine;
using System;

namespace GTI.Cli.Model
{
    internal class CommandLineOptions
    {
        [Option(shortName: 'j',
            longName: "jsonInput",
            Required = true, 
            HelpText = "Path to Google Task JSON export file")]
        public string JsonInputPath { get; set; }

        [Option(shortName: 'm',
            longName: "outputMode",
            HelpText = "Mode that the output generation should follow.\n" +
            "Must be one of the following: File, CalDAV\n" +
            "Note: CalDAV mode currently only works with adding new calendars.",
            Default = "File")]
        public ICalOutputMode OutputMode { get; set; }

        [Option(shortName: 'o',
            longName: "outputPath",
            HelpText = "Path to the folder in which the files generated are to be saved.\nOutputMode: File",
            SetName = "FileOpts")]
        public string OutputPath { get; set; }

        [Option(longName: "calDavUri",
            HelpText = "Base URL of the CALDAV endpoint.\n" +
            "OutputMode: CalDAV\n" +
            "Example: https://yournextcloud.tld/remote.php/dav/calendars/youruser",
            SetName = "CalDavOpts")]
        public Uri CalDavUri { get; set; }

        [Option(longName: "calDavUser", 
            HelpText = "User for the CALDAV endpoint.\nOutputMode: CalDAV",
            SetName = "CalDavOpts")]
        public string CalDavUser { get; set; }

        [Option(longName: "calDavPass",
            HelpText = "Password for the CALDAV endpoint.\nOutputMode: CalDAV", 
            SetName = "CalDavOpts")]
        public string CalDavPass { get; set; }
    }
}