using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models.Vision
{
    public class ChatGPTVisionTextContent //: ChatGPTVIsionContent
    {
        /// <summary>
        /// The type of the message is `text`
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = "text";

        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
