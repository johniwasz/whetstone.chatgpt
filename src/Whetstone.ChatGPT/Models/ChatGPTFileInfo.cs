// SPDX-License-Identifier: MIT
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;

namespace Whetstone.ChatGPT.Models
{


    [DebuggerDisplay("Id = {Id}, filename = {Filename}, Purpose = {Purpose}")]
    public class ChatGPTFileInfo
    {

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("object")]
        [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This is the name of the property returned by the API.")]
        public string? @Object { get; set; }

        [JsonPropertyName("bytes")]
        public int Bytes { get; set; }

        [JsonConverter(typeof(UnixEpochTimeJsonConverter))]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }


        [JsonPropertyName("filename")]
        public string? Filename { get; set; }


        [JsonPropertyName("purpose")]
        public string? Purpose { get; set; }


    }
}
