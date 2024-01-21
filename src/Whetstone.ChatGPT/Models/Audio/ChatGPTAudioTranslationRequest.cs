// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models.File;

namespace Whetstone.ChatGPT.Models.Audio
{
    /// <summary>
    /// Translates audio into into English.
    /// </summary>
    /// <remarks>
    /// See <see href="https://platform.openai.com/docs/api-reference/audio/create">Create Translation</see>
    /// </remarks>
    [DebuggerDisplay("Text = {Text}")]
    public class ChatGPTAudioTranslationRequest
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
    }
}
