using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Whetstone.ChatGPT.Models
{
    public enum ModerationModels
    {        
        [EnumMember(Value = "text-moderation-latest")]
        Latest,
        [EnumMember(Value = "text-moderation-stable")]
        Stable
    }

    /// <summary>
    /// Request to Classify if text violates OpenAI's Content Policy
    /// </summary>
    /// <remarks>
    /// <see href="https://beta.openai.com/docs/api-reference/moderations/create">Create moderation</see>.
    /// </remarks>
    /// 
    public class ChatGPTCreateModerationRequest
    {
        /// <summary>
        /// The input text to classify.
        /// </summary>
        [JsonPropertyOrder(0)]
        [JsonInclude]
        [JsonPropertyName("input")]
        public List<string>? Inputs { get; set; }

        /// <summary>
        /// Two content moderations models are available: <c>text-moderation-stable</c> and <c>text-moderation-latest.</c>
        /// </summary>
        /// <remarks>
        /// <para>The default is <c>text-moderation-latest</c> which will be automatically upgraded over time. 
        /// This ensures you are always using our most accurate model. If you use <c>text-moderation-stable</c>, we will provide advanced notice before updating the model. 
        /// Accuracy of <c>text-moderation-stable</c> may be slightly lower than for <c>text-moderation-latest</c>.</para>
        /// <para>Defaults to <c><text-embedding-ada-002</c></para>
        /// <para>See <see cref="ChatGPTCompletionModels">ChatGPTCompletionModels</see> for recommended completion models.</para>
        /// </remarks>
        [JsonPropertyOrder(1)]
        [JsonConverter(typeof(EnumConverter<ModerationModels>))]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("model")]
        public ModerationModels? Model { get; set; }
    }
}
