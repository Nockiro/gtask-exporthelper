using GTI.Core.Contracts.Model;
using System.Collections.Generic;

namespace GTI.Core.Services
{
    public interface IGoogleTaskWriter
    {
        void Write(List<GoogleTaskList> listsToWrite);
    }
}