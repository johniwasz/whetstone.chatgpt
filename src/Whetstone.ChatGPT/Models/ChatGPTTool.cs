using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
{
    public class ChatGPTTool
    {
        /// <summary>
        /// The type of the tool. Currently, only function is supported.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = "function";

        [JsonPropertyName("function")]
        public ChatGPTFunction? Function { get; set; }
    }
}
