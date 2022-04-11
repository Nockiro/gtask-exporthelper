using GTI.Core.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTI.Core.Contracts
{
    public interface IGoogleTaskDataProvider
    {
        List<GoogleTaskList> GetTaskLists();
    }
}
