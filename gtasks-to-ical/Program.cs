using CommandLine;
using GTI.Cli.Model;
using GTI.Core.Contracts;
using GTI.Core.Contracts.Model;
using GTI.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace GTI.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(o =>
                {
                    writeFromJsonInput(o.JsonInputPath);
                });
        }

        private static void writeFromJsonInput(string jsonInputPath)
        {
            IGoogleTaskDataProvider taskProvider = new GoogleTaskJsonDataProvider(jsonInputPath);
            IGoogleTaskToICalSerializer taskSerializer = new GoogleTaskToICalSerializer();

            List<GoogleTaskList> googleTaskLists = taskProvider.GetTaskLists();

            foreach (GoogleTaskList list in googleTaskLists)
            {
                File.WriteAllText($"{list.Title.ToLower()}.ics", taskSerializer.Serialize(list));
            }
        }
    }
}
