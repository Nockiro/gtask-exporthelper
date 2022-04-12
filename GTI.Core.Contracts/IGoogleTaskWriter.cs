using GTI.Core.Contracts.Model;
using System.Collections.Generic;

namespace GTI.Core.Services
{
    public interface IGoogleTaskWriter
    {
        /// <summary>
        /// Writes the given list of <see cref="GoogleTaskList"/> to a target defined by the class implementing this interface.
        /// </summary>
        /// <param name="listsToWrite">List with Google Task lists to write.</param>
        void Write(List<GoogleTaskList> listsToWrite);
    }
}