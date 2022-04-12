using GTI.Core.Contracts;
using GTI.Core.Contracts.Model;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using System;
using System.Collections.Generic;

namespace GTI.Core.Services
{
    public class GoogleTaskToICalSerializer : IGoogleTaskToICalSerializer
    {
        private Dictionary<GoogleTask, Todo> _taskTodoMap;
        private readonly CalendarSerializer _serializer = new();

        public string Serialize(GoogleTaskList taskList)
        {
            _taskTodoMap = new();

            List<Todo> events = getItems(taskList);
            addParentRelations(taskList);

            var calendar = new Calendar();
            calendar.AddTimeZone(TimeZoneInfo.Utc);
            calendar.Todos.AddRange(events);

            return _serializer.SerializeToString(calendar);
        }

        public Dictionary<string, string> SerializeMany(GoogleTaskList taskList)
        {
            _taskTodoMap = new();

            List<Todo> events = getItems(taskList);
            addParentRelations(taskList);

            Dictionary<string, string> calendarEntries = new();

            foreach (var item in events)
            {
                var calendar = new Calendar();
                calendar.AddTimeZone(TimeZoneInfo.Utc);
                calendar.Todos.Add(item);

                calendarEntries.Add(item.Uid, _serializer.SerializeToString(calendar));
            }

            return calendarEntries;
        }

        private List<Todo> getItems(GoogleTaskList taskList)
        {
            List<Todo> events = new();

            foreach (GoogleTask googleTaskItem in taskList.Items)
            {
                Todo todoItem = new()
                {
                    Created = new CalDateTime(googleTaskItem.Created),
                    LastModified = new CalDateTime(googleTaskItem.Updated),
                    Summary = googleTaskItem.Title,
                    Description = googleTaskItem.Notes
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
                    todoItem.AddProperty("X-GTASKURL", googleTaskItem.SelfLink);

                if (googleTaskItem.Due != default)
                    todoItem.Due = new CalDateTime(googleTaskItem.Due);

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
