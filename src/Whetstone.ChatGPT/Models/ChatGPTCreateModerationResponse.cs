using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
{
    public class ChatGPTCreateModerationResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }


        /// <summary>
        /// Model that was used by the moderation engine.
        /// </summary>
        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("results")]
        public List<ModerationResult>? Results { get; set; }
        
    }

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

    public class ModerationResult
    {
        [JsonPropertyName("categories")]
        public ModerationCategories? Categories { get; set; }

        [JsonPropertyName("category_scores")]
        public ModerationCategoryScores? CategoryScores { get; set; }

        [JsonPropertyName("flagged")]
        public bool Flagged { get; set; }
    }
}
