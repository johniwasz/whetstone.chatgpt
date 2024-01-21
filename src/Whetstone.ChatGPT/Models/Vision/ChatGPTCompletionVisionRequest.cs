using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models.Vision
{
    public class ChatGPTCompletionVisionRequest
    {

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("messages")]
        public IEnumerable<ChatGPTCompletionVisionMessage>? Messages { get; set; }

        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; }
    }
}
