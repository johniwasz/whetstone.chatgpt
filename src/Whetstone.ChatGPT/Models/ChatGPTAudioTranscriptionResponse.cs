using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
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
        public List<GPTAudioSegment>? Segments { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }

    public class GPTAudioSegment
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("seek")]
        public int? Seek { get; set; }

        [JsonPropertyName("start")]
        public float? Start { get; set; }

        [JsonPropertyName("end")]
        public float? End { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("tokens")]
        public List<int>? Tokens { get; set; }

        /// <summary>
        /// The sampling temperature, between 0 and 1. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic. If set to 0, the model will use <see href="https://en.wikipedia.org/wiki/Log_probability">log probability</see> to automatically increase the temperature until certain thresholds are hit.
        /// </summary>
        [JsonPropertyName("temperature")]
        public float? Temperature { get; set; }

        [JsonPropertyName("avg_logprob")]
        public double? AverageLogProbability { get; set; }

        [JsonPropertyName("compression_ratio")]
        public double? CompressionRatio { get; set; }

        [JsonPropertyName("no_speech_prob")]
        public double? NoSpeechProbability { get; set; }
        
        [JsonPropertyName("transient")]
        public bool Transient { get; set; }
    }
}
