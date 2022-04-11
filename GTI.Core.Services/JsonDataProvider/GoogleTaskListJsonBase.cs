using GTI.Core.Contracts.Model;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GTI.Core.Services
{
    internal class GoogleTaskListJsonBase
    {
        [JsonPropertyName("items")]
        public List<GoogleTaskList> Items { get; set; } = new List<GoogleTaskList>();
    }
}
