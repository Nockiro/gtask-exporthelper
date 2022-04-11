using GTI.Core.Contracts.Model;

namespace GTI.Core.Contracts
{
    public interface IGoogleTaskToICalSerializer
    {
        /// <summary>
        /// Serializes a list of Google Tasks (a <see cref="GoogleTaskList"/>) to a string in the iCalendar file format.
        /// </summary>
        /// <param name="taskList">Tasks to serialize.</param>
        /// <returns>String in the iCalendar file format</returns>
        string Serialize(GoogleTaskList taskList);
    }
}