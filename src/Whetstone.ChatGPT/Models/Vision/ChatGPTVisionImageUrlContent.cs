using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models.Vision
{
    public class ChatGPTVisionImageUrlContent //: ChatGPTVIsionContent
    {
        /// <summary>
        /// The type of the message is `image_url`
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = "image_url";

        [JsonPropertyName("image_url")]
        public ChatGPTVisionImageUrl? ImageUrl { get; set; }
    }
}
