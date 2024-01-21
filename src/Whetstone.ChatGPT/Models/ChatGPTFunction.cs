using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
{
    public class ChatGPTFunction
    {

        /// <summary>
        /// A description of what the function does, used by the model to choose when and how to call the function.
        /// </summary>
        [JsonPropertyName("description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; } = null;

        /// <summary>
        /// The name of the function to be called. Must be a-z, A-Z, 0-9, or contain underscores and dashes, 
        /// with a maximum length of 64.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// The parameters the functions accepts, described as a JSON Schema object. See the guide for examples, and the JSON Schema reference for documentation about the format.
        /// To describe a function that accepts no parameters, provide the value {"type": "object", "properties": {}}.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("parameters")]
        public ChatGPTParameter? Parameters { get; set; } = null;

        /// <summary>
        /// List of required parameters
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("required")]
        public List<string>? Required { get; set; } = null;
    }

    public class ChatGPTParameter()
    {

        [JsonPropertyName("type")]
        public string Type { get; set; } = String.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// The parameters the functions accepts, described as a JSON Schema object. See the guide for examples, and the JSON Schema reference for documentation about the format.
        /// To describe a function that accepts no parameters, provide the value {"type": "object", "properties": {}}.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("properties")]
        public Dictionary<string, ChatGPTParameter>? Properties { get; set; } = null;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("enum")]
        public List<string>? Enum { get; set; } = null;
    }
}
