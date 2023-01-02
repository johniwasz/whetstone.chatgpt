using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
{

    /// <summary>
    /// Return an edited version of the prompt given an instruction.
    /// </summary>
    public class ChatGPTCreateEditResponse
    {

        /// <summary>
        /// This value will be "edit"
        /// </summary>
        [JsonPropertyName("object")]
        [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This is the name of the property returned by the API.")]
        public string? @Object { get; set; }

        [JsonConverter(typeof(UnixEpochTimeJsonConverter))]
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }
        
        /// <summary>
        /// Edit choices returned by the GPT-3 API.
        /// </summary>
        [JsonPropertyName("choices")]
        public List<ChatGPTChoice>? Choices { get; set; }


        /// <summary>
        /// Details the number of tokens used to process the edit.
        /// </summary>
        [JsonPropertyName("usage")]
        public ChatGPTUsage? Usage { get; set; }

    }
}
