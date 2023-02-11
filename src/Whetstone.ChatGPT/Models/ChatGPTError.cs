using System.Text.Json.Serialization;

namespace Whetstone.ChatGPT.Models
{

    /// <summary>
    /// Error response message returned by the GPT-3 API.
    /// </summary>
    public class ChatGPTErrorResponse
    {

        /// <summary>
        /// The error returned by the GPT-3 API. 
        /// </summary>
        [JsonPropertyName("error")]
        public ChatGPTError? Error { get; set; }
    }

    /// <summary>
    /// Represents an error returned by the GPT-3 API.
    /// </summary>
    public class ChatGPTError
    {
        /// <summary>
        /// Human-readable error message.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("param")]
        public object? Param { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }
    }
}