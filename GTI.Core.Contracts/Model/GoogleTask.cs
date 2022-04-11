using System;
using System.Text.Json.Serialization;

namespace GTI.Core.Contracts.Model
{
    /// <summary>
    /// Represents one task from a Google Tasks list.
    /// </summary>
    public class GoogleTask
    {
        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        [JsonPropertyName("due")]
        public DateTime Due { get; set; }

        /// <summary>
        /// Note: Tasks JSON returns the date in UTC!
        /// </summary>
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("task_type")]
        public string TaskType { get; set; }

        [JsonPropertyName("updated")]
        public DateTime Updated { get; set; }

        [JsonPropertyName("selfLink")]
        public string SelfLink { get; set; }

        [JsonPropertyName("status")]
        public GoogleTaskStatus Status { get; set; }

        [JsonPropertyName("completed")]
        public DateTime? Completed { get; set; }

        [JsonPropertyName("parent")]
        public string ParentId { get; set; }

        public GoogleTask Parent { get; set; }
    }
}