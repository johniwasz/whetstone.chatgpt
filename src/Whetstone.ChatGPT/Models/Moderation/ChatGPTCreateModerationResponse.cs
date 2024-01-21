// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models.Moderation
{
    public class ChatGPTCreateModerationResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Model that was used by the moderation engine.
        /// </summary>
        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("results")]
        public List<ModerationResult>? Results { get; set; }

    }
}
