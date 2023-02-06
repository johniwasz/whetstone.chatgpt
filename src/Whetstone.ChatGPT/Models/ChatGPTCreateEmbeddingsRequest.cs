using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
{
    /// <summary>
    /// Defines a request to create a embedding vector representing the input text.
    /// </summary>
    /// <remarks>
    /// <para>Get a vector representation of a given input that can be easily consumed by machine learning models and algorithms.</para>
    /// <para><see href="https://beta.openai.com/docs/api-reference/embeddings/create">Create embeddings</see></para>
    /// </remarks>
    public class ChatGPTCreateEmbeddingsRequest
    {
        /// <summary>
        /// ID of the model to use. You can use the <see href="https://beta.openai.com/docs/api-reference/models/list">List models</see> API to see all of your available models, or see our <see href="https://beta.openai.com/docs/models/overview">Model overview</see> for descriptions of them.
        /// </summary>
        /// <remarks>
        /// <para>Defaults to <c><text-embedding-ada-002</c></para>
        /// <para>See <see cref="ChatGPTEmbeddingModels">ChatGPTEmbeddingModels</see> for recommended embedding models.</para>   
        /// </remarks>
        [JsonPropertyOrder(0)]
        [JsonInclude]
        [JsonPropertyName("model")]
        public string? Model { get; set; } = ChatGPTEmbeddingModels.Ada;

        /// <summary>
        /// Input text to get embeddings for, encoded as a string or array of tokens. To get embeddings for multiple inputs in a single request, pass an array of strings or array of token arrays. Each input must not exceed 8192 tokens in length.
        /// </summary>
        [JsonPropertyOrder(1)]
        [JsonInclude]
        [JsonPropertyName("input")]
        public List<string>? Inputs { get; set; }


        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse. <see cref="https://beta.openai.com/docs/guides/safety-best-practices/end-user-ids">Learn more.</see>
        /// </summary>
        [JsonPropertyOrder(2)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("user")]
        public string? User { get; set; }

    }
}
