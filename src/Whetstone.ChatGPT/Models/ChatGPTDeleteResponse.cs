using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Whetstone.ChatGPT.Models
{
    [DebuggerDisplay("Id = {Id}, Deleted = {Deleted}")]
    public class ChatGPTDeleteResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("object")]
        [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This is the name of the property returned by the API.")]
        public string? @Object { get; set; }

        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }
    }
}
