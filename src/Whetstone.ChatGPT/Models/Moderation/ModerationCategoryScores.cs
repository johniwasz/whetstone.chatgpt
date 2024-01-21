using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models.Moderation
{
    public class ModerationCategoryScores
    {
        [JsonPropertyName("hate")]
        public float Hate { get; set; }

        [JsonPropertyName("hate/threatening")]
        public float HateThreatening { get; set; }

        [JsonPropertyName("elf-harm")]
        public float SelfHarm { get; set; }

        [JsonPropertyName("sexual")]
        public float Sexual { get; set; }

        [JsonPropertyName("sexual/minors")]
        public float SexualMinors { get; set; }

        [JsonPropertyName("violence")]
        public float Violence { get; set; }

        [JsonPropertyName("violence/graphic")]
        public float ViolenceGraphic { get; set; }
    }
}
