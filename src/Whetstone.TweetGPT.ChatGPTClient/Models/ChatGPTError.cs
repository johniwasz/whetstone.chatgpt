using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Whetstone.TweetGPT.ChatGPTClient.Models
{
    public class ChatGPTErrorResponse
    {

        [JsonPropertyName("error")]
        public ChatGPTError? Error { get; set; }

    }


    public class ChatGPTError
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("param")]
        public object? Param { get; set; }


        [JsonPropertyName("code")]
        public string? Code { get; set; }
    }

}
