using GTI.Core.Contracts;
using GTI.Core.Contracts.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GTI.Core.Services
{
    public class GoogleTaskJsonDataProvider : IGoogleTaskDataProvider
    {
        public string CurrentFilePath { get; }

        public GoogleTaskJsonDataProvider(string filePath)
        {
            CurrentFilePath = filePath;
        }

        public List<GoogleTaskList> GetTaskLists()
        {
            JsonSerializerOptions jsonOptions = new()
            {
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };

            GoogleTaskListJsonBase items = JsonSerializer.Deserialize<GoogleTaskListJsonBase>(File.ReadAllText(CurrentFilePath), jsonOptions);

            setParentFromParentId(items);
            return items.Items;
        }

        private void setParentFromParentId(GoogleTaskListJsonBase items)
        {
            foreach (GoogleTaskList list in items.Items)
            {
                foreach (GoogleTask task in list.Items)
                {
                    task.Parent = list.Items.Where(i => i.Id == task.ParentId).FirstOrDefault();
                }
            }
        }
    }
}
