using CommandLine;
using GTI.Cli.Model;
using GTI.Core.Contracts;
using GTI.Core.Contracts.Model;
using GTI.Core.Services;
using System;
using System.Collections.Generic;

namespace GTI.Cli
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            IGoogleTaskWriter taskWriter = null;
            IGoogleTaskDataProvider taskProvider = null;
            IGoogleTaskToICalSerializer taskSerializer = new GoogleTaskToICalSerializer();

            Parser commandLineParser = new(with =>
            {
                with.CaseInsensitiveEnumValues = true;
                with.AutoHelp = true;
                with.AutoVersion = true;
                with.EnableDashDash = true;

                with.HelpWriter = Console.Error;
            });

            commandLineParser.ParseArguments<CommandLineOptions>(args)
                .WithParsed(o =>
                {
                    taskProvider = new GoogleTaskJsonDataProvider(o.JsonInputPath);

                    switch (o.OutputMode)
                    {
                        case ICalOutputMode.File:
                            taskWriter = new GoogleTaskFileWriter(taskSerializer, new GoogleTaskFileWriteOptions()
                            {
                                OutputDirectory = o.OutputPath
                            });
                            break;

                        case ICalOutputMode.CalDAV:
                            taskWriter = new GoogleTaskCalDAVWriter(taskSerializer, new GoogleTaskCalDAVWriteOptions()
                            {
                                BaseUri = o.CalDavUri,
                                AuthUser = o.CalDavUser,
                                AuthPass = o.CalDavPass
                            });
                            break;
                    }
                });

            if (taskWriter != null)
            {
                Console.WriteLine("Retrieving task list.." + Environment.NewLine);
                List<GoogleTaskList> googleTaskLists = taskProvider.GetTaskLists();

                Console.WriteLine("Writing output.." + Environment.NewLine);
                taskWriter.Write(googleTaskLists);

                Console.WriteLine("Export done.");
            }
        }
    }
}