using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models.Vision
{
    public class ChatGPTCompletionVisionMessage
    {
        /// <summary>
        /// The role of the messages author, supported values include `assistant`, `system`, `user`, `tool`. `function` is deprecated.
        /// </summary>
        /// <remarks>Defaults to `system`.</remarks>
        [JsonPropertyName("role")]
        public string Role { get; set; } = "user";

        /// <summary>
        /// The contents of the message.
        /// </summary>
        [JsonPropertyName("content")]
        public List<object>? Content { get; set; }

        /// <summary>
        /// An optional name for the participant. Provides the model information to differentiate between participants of the same role.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

    }
}
