// SPDX-License-Identifier: MIT
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.TweetGPT.WebHookManager.Models
{
    public class WebhookCrcResponse
    {

        [JsonPropertyName("response_token")]
        [JsonProperty(PropertyName = "response_token")]
        public string? ResponseToken { get; set; }
    }
}
