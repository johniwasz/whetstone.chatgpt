using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
{

    [DebuggerDisplay("Message = {Message}, Object = {@Object}, Level = {Level}, CreatedAt = {CreatedAt}")]
    public class ChatGPTEvent
    {
        [JsonPropertyName("object")]
        [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This is the name of the property returned by the API.")]
        public string? @Object { get; set; }

        [JsonConverter(typeof(UnixEpochTimeJsonConverter))]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("level")]
        public string? Level { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
