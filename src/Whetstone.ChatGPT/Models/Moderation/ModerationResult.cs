using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models.Moderation
{
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
