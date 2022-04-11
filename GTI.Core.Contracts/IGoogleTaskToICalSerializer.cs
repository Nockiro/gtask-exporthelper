using GTI.Core.Contracts.Model;

namespace GTI.Core.Contracts
{
    public interface IGoogleTaskToICalSerializer
    {
        string Serialize(GoogleTaskList taskList);
    }
}