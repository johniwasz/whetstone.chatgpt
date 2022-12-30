using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Whetstone.ChatGPT.Models
{
    /// <summary>
    /// A list of available completion models.
    /// </summary>
    public class ChatGPTModelsResponse
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
        public List<ChatGPTModel>? Data { get; set; }
    }
}
