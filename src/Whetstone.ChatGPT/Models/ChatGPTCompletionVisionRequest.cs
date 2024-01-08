using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
{
    public class ChatGPTCompletionVisionRequest
    {

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("messages")]
        public IEnumerable<ChatGPTCompletionVisionMessage>? Messages { get; set; }

        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; }

    }

    public class ChatGPTCompletionVisionMessage
    {
        /// <summary>
        /// The role of the messages author, supported values include `assistant`, `system`, `user`, `tool`. `function` is deprecated.
        /// </summary>
        /// <remarks>Defaults to `system`.</remarks>
        [JsonPropertyName("role")]
        public string Role { get; set; } = "user";

        /// <summary>
        /// The contents of the message.
        /// </summary>
        [JsonPropertyName("content")]
        public List<object> Content { get; set; }

        /// <summary>
        /// An optional name for the participant. Provides the model information to differentiate between participants of the same role.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

    }

    //public abstract class ChatGPTVIsionContent
    //{

    //    public virtual string Type { get; set; }
    //}

    public class ChatGPTVisionTextContent //: ChatGPTVIsionContent
    {
        /// <summary>
        /// The type of the message is `text`
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = "text";

        [JsonPropertyName("text")]
        public string? Text { get; set; } 
    }

    public class ChatGPTVisionImageUrlContent //: ChatGPTVIsionContent
    {
        /// <summary>
        /// The type of the message is `image_url`
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = "image_url";

        [JsonPropertyName("image_url")]
        public ChatGPTVisionImageUrl? ImageUrl { get; set; }
    }

    public class ChatGPTVisionImageUrl
    {

        [JsonPropertyName("url")]
        public string? Url { get; set; }


        /// <summary>
        /// Supported vaues are `low`, `high` and `auto`.
        /// </summary>
        [JsonPropertyName("detail")]
        public string? Detail { get; set; } = "auto";
    }
}
