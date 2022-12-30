using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Whetstone.ChatGPT.Models
{
    public class ChatGPTModelsResponse
    {
        [JsonPropertyName("models")]
        public string? @Object { get; set; }

        [JsonPropertyName("data")]
        public List<ChatGPTModel>? Data { get; set; }
    }
}
