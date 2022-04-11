using GTI.Core.Contracts;
using GTI.Core.Contracts.Model;
using System;
using System.Collections.Generic;

namespace GTI.Core.Services
{
    public class GoogleTaskSampleDataProvider : IGoogleTaskDataProvider
    {
        public List<GoogleTaskList> GetTaskLists()
        {
            List<GoogleTaskList> sampleList = new()
            {
                new GoogleTaskList()
                {
                    Id = "MDdySDMtMmZSZm5pSThuYQ",
                    Title = "Testliste",
                    Updated = DateTime.Now,
                    Items = new List<GoogleTask>() {
                    new GoogleTask()
                    {
                        Notes = "Details",
                        Due = new DateTime(2022,12,24),
                        Status = GoogleTaskStatus.Completed
                    },
                    new GoogleTask()
                    {
                        Title = "Testaufgabe mit Unteraufgaben",
                        Notes = "Hier könnte z.B. eine Email auftauchen",
                        Due = new DateTime(2022,12,25),
                        Status = GoogleTaskStatus.NeedsAction
                    }
                }
                },
                new GoogleTaskList() { Id = "ABCDEFGHI", Title = "Testliste2", Updated = DateTime.Now },
            };

            sampleList[0].Items.Add(
                    new GoogleTask()
                    {
                        Title = "Unteraufgabe1",
                        Due = new DateTime(2022, 12, 26),
                        Status = GoogleTaskStatus.Completed,
                        Parent = sampleList[0].Items[1]
                    });

            return sampleList;
        }
    }
}
