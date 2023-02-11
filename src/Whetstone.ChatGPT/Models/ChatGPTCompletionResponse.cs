﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Whetstone.ChatGPT.Models
{
    public class ChatGPTCompletionResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("object")]
        [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This is the name of the property returned by the API.")]
        public string? @Object { get; set; }

        [JsonConverter(typeof(UnixEpochTimeJsonConverter))]
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Model used to generate the original request. 
        /// </summary>
        /// <remarks>
        /// See <see cref="ChatGPTCompletionModels">ChatGPTCompletionModels</see> for recommended completion models.
        /// </remarks>
        [JsonPropertyName("model")]
        public string? Model { get; set; }

        /// <summary>
        /// Completion choices generated by the GPT-3 API.
        /// </summary>
        [JsonPropertyName("choices")]
        public List<ChatGPTChoice>? Choices { get; set; }

        /// <summary>
        /// Details the number of tokens used to process the completion.
        /// </summary>
        [JsonPropertyName("usage")]
        public ChatGPTUsage? Usage { get; set; }
    }

    /// <summary>
    /// Detailed breakdown of tokens used to process the completion request.
    /// </summary>
    public class ChatGPTUsage
    {
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; }

        [JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; set; }
        
        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
    }

    [DebuggerDisplay("Text = {Text}")]
    public class ChatGPTChoice
    {
        /// <summary>
        /// Completion text generated by the GPT-3 API.
        /// </summary>
        /// <remarks>Returned text does not include the stop sequence.</remarks>
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }

        /// <summary>
        /// This is populated if the <see cref="ChatGPTCompletionRequest.LogProbabilities">ChatGPTCompletionRequest.LogProbabilities</see> is set to a vaule of 1-5.
        /// </summary>
        [JsonPropertyName("logprobs")]
        public object? LogProbabilities { get; set; }

        [JsonPropertyName("finish_reason")]
        public string? FinishReason { get; set; }
    }
}