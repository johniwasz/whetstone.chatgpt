using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models.FineTuning
{
    public class ChatGPTTurboFineTuneLineMessage
    {
        public ChatGPTTurboFineTuneLineMessage()
        {
        }

        public ChatGPTTurboFineTuneLineMessage(string role, string content)
        {
            if(string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Cannot be null, empty, or whitespace", nameof(role));

            this.Role = role;
            this.Content = content;
        }


        [JsonPropertyName("role")]
        public string? Role { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }
    }
}
