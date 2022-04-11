using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GTI.Core.Contracts.Model
{
    public class GoogleTaskListJsonBase
    {
        [JsonPropertyName("items")]
        public List<GoogleTaskList> Items { get; set; } = new List<GoogleTaskList>();
    }
}
