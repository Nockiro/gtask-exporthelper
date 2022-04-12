using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GTI.Core.Contracts.Model
{
    /// <summary>
    /// Represents one list of Google Tasks.
    /// </summary>
    public class GoogleTaskList
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("updated")]
        public DateTime Updated { get; set; }

        [JsonPropertyName("items")]
        public List<GoogleTask> Items { get; set; } = new List<GoogleTask>();
    }
}
