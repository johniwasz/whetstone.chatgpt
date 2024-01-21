// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models.Audio
{
    public class ChatGPTAudioResponse
    {
        [JsonPropertyName("task")]
        public string? Task { get; set; }

        [JsonPropertyName("language")]
        public string? Language { get; set; }

        [JsonPropertyName("duration")]
        public float? Duration { get; set; }

        [JsonPropertyName("segments")]
        public List<AudioSegment>? Segments { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }

 
}
