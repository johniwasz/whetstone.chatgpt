// SPDX-License-Identifier: MIT
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Whetstone.ChatGPT.Models.Moderation
{
    public enum ModerationModels
    {
        [EnumMember(Value = "text-moderation-latest")]
        TextLatest,
        [EnumMember(Value = "omni-moderation-latest")]
        OmniLatest,
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
        /// Two content moderations models are available: <c>omni-moderation-latest</c> and <c>text-moderation-latest (Legacy)</c>
        /// </summary>
        /// <remarks>
        /// <para>Use the moderations endpoint to check whether text or images are potentially harmful. If harmful content is identified, you can take corrective action, 
        /// like filtering content or intervening with user accounts creating offending content. The moderation endpoint is free to use.</para>
        /// <para>See <see cref="ChatGPTCompletionModels">ChatGPTCompletionModels</see> for recommended completion models.</para>
        /// </remarks>
        [JsonPropertyOrder(1)]
        [JsonConverter(typeof(EnumConverter<ModerationModels>))]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("model")]
        public ModerationModels? Model { get; set; }
    }
}
