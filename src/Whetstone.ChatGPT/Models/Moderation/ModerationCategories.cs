using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models.Moderation
{
    public class ModerationCategories
    {
        [JsonPropertyName("hate")]
        public bool Hate { get; set; }

        [JsonPropertyName("hate/threatening")]
        public bool HateThreatening { get; set; }

        [JsonPropertyName("self-harm")]
        public bool SelfHarm { get; set; }

        [JsonPropertyName("sexual")]
        public bool Sexual { get; set; }

        [JsonPropertyName("sexual/minors")]
        public bool SexualMinors { get; set; }

        [JsonPropertyName("violence")]
        public bool Violence { get; set; }

        [JsonPropertyName("violence/graphic")]
        public bool ViolenceGraphic { get; set; }
    }
}
