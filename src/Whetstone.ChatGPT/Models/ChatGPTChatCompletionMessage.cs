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
    public enum MessageRole
    {
        [EnumMember(Value = "system")]
        System,
        [EnumMember(Value = "user")]
        User,
        [EnumMember(Value = "assistant")]
        Assistant
    }

    public class ChatGPTChatCompletionMessage
    {        
        [DefaultValue(MessageRole.User)]
        [JsonConverter(typeof(EnumConverter<MessageRole>))]
        [JsonPropertyName("role")]
        public MessageRole Role { get; set; } = MessageRole.User;

        [JsonPropertyName("content")]
        public string? Content { get; set; }

    }
}
