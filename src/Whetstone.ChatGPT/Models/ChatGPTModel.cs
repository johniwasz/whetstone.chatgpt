using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Diagnostics;

namespace Whetstone.ChatGPT.Models
{

    [DebuggerDisplay("Id = {Id}, OwnedBy = {OwnedBy}, CreatedAt = {CreatedAt}")]
    public class ChatGPTModel
    {

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("object")]
        public string? @Object { get; set; }

        [JsonPropertyName("created")]
        public int Created { get; set; }

        [JsonPropertyName("owned_by")]
        public string? OwnedBy { get; set; }

        [JsonPropertyName("permission")]
        public List<ChatGPTModelPermissions>? Permission { get; set; }

        [JsonPropertyName("root")]
        public string? Root { get; set; }

        [JsonPropertyName("parent")]
        public string? Parent { get; set; }
    }
}
