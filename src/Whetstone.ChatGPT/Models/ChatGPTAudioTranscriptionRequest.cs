using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
{
    public enum AudioResponseFormatText
    {
        [EnumMember(Value = "text")]
        Text = 0,
        [EnumMember(Value = "srt")]
        SubRip = 1,
        [EnumMember(Value = "vtt")]
        WebVtt = 2
    }

    /// <summary>
    /// Transcribes audio into the input language.
    /// </summary>
    /// <remarks>
    /// Related guide: <see href="https://platform.openai.com/docs/guides/speech-to-text">Speech to Text</see>
    /// </remarks>
    [DebuggerDisplay("Text = {Text}")]
    public class ChatGPTAudioTranscriptionRequest
    {
        /// <summary>
        /// The audio file to transcribe, in one of these formats: mp3, mp4, mpeg, mpga, m4a, wav, or webm.
        /// </summary>
        [JsonPropertyName("file")]
        public ChatGPTFileContent? File { get; set; }

        /// <summary>
        /// ID of the model to use. Only <c>whisper-1</c> is currently available.
        /// </summary>       
        [JsonPropertyName("model")]
        public string? Model { get; set; } = "whisper-1";

        /// <summary>
        /// An optional text to guide the model's style or continue a previous audio segment. The <see href="https://platform.openai.com/docs/guides/speech-to-text/prompting">prompt</see> should match the audio language.
        /// </summary>
        [JsonPropertyName("prompt")]
        public string? Prompt { get; set; }

        /// <summary>
        /// The sampling temperature, between 0 and 1. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic. If set to 0, the model will use <see href="https://en.wikipedia.org/wiki/Log_probability">log probability</see> to automatically increase the temperature until certain thresholds are hit.
        /// </summary>
        [JsonPropertyName("temperature")]
        public float Temperature { get; set; } = 0.0f;

        /// <summary>
        /// The language of the input audio. Supplying the input language in <see href="https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes">ISO-639-1</see> format will improve accuracy and latency.
        /// </summary>
        [JsonPropertyName("language")]
        public string? Language { get; set; }

    }
}
