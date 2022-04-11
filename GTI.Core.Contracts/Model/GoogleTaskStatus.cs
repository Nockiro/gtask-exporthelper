using System.Text.Json.Serialization;

namespace GTI.Core.Contracts.Model
{
    public enum GoogleTaskStatus
    {
        None,
        [JsonConverter(typeof(JsonStringEnumConverter))]
        NeedsAction,
        [JsonConverter(typeof(JsonStringEnumConverter))]
        Completed
    }
}