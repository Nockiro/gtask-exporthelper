using GTI.Core.Contracts.Model;
using System.Collections.Generic;

namespace GTI.Core.Contracts
{
    public interface IGoogleTaskDataProvider
    {
        /// <summary>
        /// Gets a list of Google Tasks lists.
        /// </summary>
        List<GoogleTaskList> GetTaskLists();
    }
}
