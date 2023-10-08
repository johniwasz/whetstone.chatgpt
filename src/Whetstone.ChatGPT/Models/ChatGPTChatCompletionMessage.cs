// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
{


    public class ChatGPTChatCompletionMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = "system";

        [JsonPropertyName("content")]
        public string? Content { get; set; }

    }
}
