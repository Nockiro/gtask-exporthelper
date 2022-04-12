using GTI.Core.Contracts.Model;
using System.Collections.Generic;

namespace GTI.Core.Contracts
{
    public interface IGoogleTaskToICalSerializer
    {
        /// <summary>
        /// Serializes a list of Google Tasks (a <see cref="GoogleTaskList"/>) to a string in the iCalendar file format.
        /// </summary>
        /// <param name="taskList">Tasks to serialize.</param>
        /// <returns>String in the iCalendar file format.</returns>
        string Serialize(GoogleTaskList taskList);

        /// <summary>
        /// Serializes a list of Google Tasks (a <see cref="GoogleTaskList"/>) to a dictionary, where every item of the list is its own calendar. <br/>
        /// This is usesful in cases where one might need one ical file per entry.
        /// </summary>
        /// <param name="taskList">Tasks to serialize.</param>
        /// <returns>Dictionary with UID of the entry as key and the item serialized in the iCalendar file format as value.</returns>
        Dictionary<string, string> SerializeMany(GoogleTaskList taskList);
    }
}