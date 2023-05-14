// SPDX-License-Identifier: MIT
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Whetstone.ChatGPT.Models
{
    public class ChatGPTListResponse<T>
    {
        /// <summary>
        /// Returned value should be "list"
        /// </summary>
        [JsonPropertyName("object")]
        [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This is the name of the property returned by the API.")]
        public string? @Object { get; set; }

        /// <summary>
        /// List of available models.
        /// </summary>
        [JsonPropertyName("data")]
        public List<T>? Data { get; set; }
    }
}
