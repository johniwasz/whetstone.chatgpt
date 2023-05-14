// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
{
    public class ChatGPTCreateEmbeddingsResponse
    {

        [JsonPropertyName("object")]
        [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This is the name of the property returned by the API.")]
        public string? @Object { get; set; }

        [JsonPropertyName("data")]
        public List<Embeddings>? Data { get; set; }


        [JsonPropertyName("model")]
        public string? Model { get; set; }


        [JsonPropertyName("usage")]
        public ChatGPTEmbeddingUsage? Usage { get; set; }

    }

    public class Embeddings
    {
        [JsonPropertyName("object")]
        [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This is the name of the property returned by the API.")]
        public string? @Object { get; set; }

        [JsonPropertyName("embedding")]
        public List<double>? Embedding { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }
    }
    
    /// <summary>
    /// Detailed breakdown of tokens used to process the embedding request.
    /// </summary>
    public class ChatGPTEmbeddingUsage
    {
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; }

        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
    }
}
