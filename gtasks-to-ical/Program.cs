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
            //IGoogleTaskDataProvider taskProvider = new GoogleTaskMockDataProvider();
            IGoogleTaskDataProvider taskProvider = new GoogleTaskJsonDataProvider("tasks.json");
            IGoogleTaskToICalSerializer taskSerializer = new GoogleTaskToICalSerializer();

            List<GoogleTaskList> googleTaskLists = taskProvider.GetTaskLists();

            foreach (GoogleTaskList list in googleTaskLists)
            {
                File.WriteAllText($"{list.Title.ToLower()}.ics", taskSerializer.Serialize(list));
            }
        }
    }
}
