﻿// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Whetstone.ChatGPT.Models
{
    /// <summary>
    /// OpenAI generated stream response to a <c>ChatGPTChatCompletionRequest</c>. Stream responses do not include Usage tokens.
    /// </summary>
    public class ChatGPTChatCompletionStreamResponse
    {
        /// <summary>
        /// Server generated ID for the completion response.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// The OpenAI object used to serialize the response. For completion responese, this should be <c>chat.completion.chunk</c>.
        /// </summary>
        [JsonPropertyName("object")]
        [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This is the name of the property returned by the API.")]
        public string? @Object { get; set; }

        /// <summary>
        /// Date and time the completion response was generated.
        /// </summary>
        [JsonConverter(typeof(UnixEpochTimeJsonConverter))]
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Completion choices generated by the OpenAI API.
        /// </summary>
        [JsonPropertyName("choices")]
        public List<ChatGPTStreamedChatChoice>? Choices { get; set; }
    }

    public class ChatGPTStreamedChatChoice
    {
        [JsonPropertyName("index")]
        public int? Index { get; set; }

        [JsonPropertyName("delta")]
        public ChatGPTChatCompletionMessage? Delta { get; set; }

        [JsonPropertyName("finish_reason")]
        public string? FinishReason { get; set; }

    }
}