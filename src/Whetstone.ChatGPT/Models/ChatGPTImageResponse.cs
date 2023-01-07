using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
{
    public class ChatGPTImageResponse
    {
        [JsonConverter(typeof(UnixEpochTimeJsonConverter))]
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }


        [JsonPropertyName("data")]
        public List<GeneratedImage>? Data { get; set; }
    }

    public class GeneratedImage
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("b64_json")]
        public string? Base64 { get; set; }
    }
}
