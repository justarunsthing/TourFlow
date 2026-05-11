using System.Text.Json.Serialization;

namespace TourFlow.Models
{
    public class GrokResponse
    {
        [JsonPropertyName("choices")]
        public List<GrokChoice> Choices { get; set; } = new();
    }

    public class GrokChoice
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("message")]
        public GrokMessage Message { get; set; } = new();

        [JsonPropertyName("finish_reason")]
        public string? FinishReason { get; set; }
    }

    public class GrokMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("reasoning_content")]
        public string? ReasoningContent { get; set; }
    }
}