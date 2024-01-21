using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models.Vision
{
    public class ChatGPTVisionImageUrl
    {

        [JsonPropertyName("url")]
        public string? Url { get; set; }


        /// <summary>
        /// Supported vaues are `low`, `high` and `auto`.
        /// </summary>
        [JsonPropertyName("detail")]
        public string? Detail { get; set; } = "auto";
    }
}
