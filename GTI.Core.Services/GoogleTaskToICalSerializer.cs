using GTI.Core.Contracts;
using GTI.Core.Contracts.Model;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTI.Core.Services
{
    public class GoogleTaskToICalSerializer : IGoogleTaskToICalSerializer
    {
        private Dictionary<GoogleTask, Todo> _taskTodoMap;

        public string Serialize(GoogleTaskList taskList)
        {
            _taskTodoMap = new();

            var calendar = new Calendar();
            calendar.AddTimeZone(TimeZoneInfo.Utc);

            List<Todo> events = getMainItems(taskList);
            addParentRelations(taskList);
            calendar.Todos.AddRange(events);

            var serializer = new CalendarSerializer();
            return serializer.SerializeToString(calendar);
        }

        private List<Todo> getMainItems(GoogleTaskList taskList)
        {
            List<Todo> events = new();

            foreach (GoogleTask googleTaskItem in taskList.Items)
            {
                Todo todoItem = new()
                {
                    Created = new CalDateTime(googleTaskItem.Created),
                    LastModified = new CalDateTime(googleTaskItem.Updated),
                    Summary = googleTaskItem.Title,
                    Description = googleTaskItem.Notes,
                };

                string iCalStatus = getICalStatus(googleTaskItem.Status);
                int? iCalPercentCompleted = getPercentCompleted(googleTaskItem.Status);

                if (!String.IsNullOrEmpty(iCalStatus))
                    todoItem.Status = iCalStatus;

                if (iCalPercentCompleted.HasValue)
                    todoItem.PercentComplete = iCalPercentCompleted.Value;

                if (googleTaskItem.Completed.HasValue)
                    todoItem.Completed = new CalDateTime(googleTaskItem.Completed.Value);

                if (googleTaskItem.SelfLink != null)
                    todoItem.Url = new Uri(googleTaskItem.SelfLink);

                events.Add(todoItem);

                _taskTodoMap.Add(googleTaskItem, todoItem);
            }

            return events;
        }

        private void addParentRelations(GoogleTaskList taskList)
        {
            foreach (GoogleTask googleTask in taskList.Items)
            {
                if (googleTask.Parent == null)
                {
                    continue;
                }

                // Find parent of current google tasks in created todos and assign it to the parent of this tasks' todo
                _taskTodoMap[googleTask].RelatedComponents = new List<string> { _taskTodoMap[googleTask.Parent].Uid };
            }
        }

        private int? getPercentCompleted(GoogleTaskStatus status)
        {
            switch (status)
            {
                case GoogleTaskStatus.NeedsAction:
                    return null;
                case GoogleTaskStatus.Completed:
                    return 100;
                case GoogleTaskStatus.None:
                    throw new NotImplementedException("Google Task Status not recognized.");
            }

            throw new NotImplementedException("Google Task Status not recognized.");
        }

        private string getICalStatus(GoogleTaskStatus status)
        {
            switch (status)
            {
                case GoogleTaskStatus.NeedsAction:
                    return "NEEDS-ACTION";
                case GoogleTaskStatus.Completed:
                    return "COMPLETED";
                case GoogleTaskStatus.None:
                    throw new NotImplementedException("Google Task Status not recognized.");
            }

            throw new NotImplementedException("Google Task Status not recognized.");
        }
    }
}
