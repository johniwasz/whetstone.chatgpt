// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.SimpleCommandLineBot.Models
{
    /// <summary>
    /// Given a prompt, the model will return one or more predicted completions, and can also return the probabilities of alternative tokens at each position.
    /// </summary>
    public class CompletionRequest
    {

        /// <summary>
        /// Initialize the completion request.
        /// </summary>
        /// <remarks>
        /// Given a prompt, the model will return one or more predicted completions, and can also return the probabilities of alternative tokens at each position.
        /// </remarks>
        public CompletionRequest()
        {

        }

        /// <summary>
        /// ID of the model to use. You can use the <see href="https://beta.openai.com/docs/api-reference/models/list">List models</see> API to see all of your available models, or see our <see href="https://beta.openai.com/docs/models/overview">Model overview</see> for descriptions of them.
        /// </summary>
        /// <remarks>
        /// </remarks>
        [JsonInclude]
        [JsonPropertyName("model")]
        public string? Model
        {
            get;
            set;
        }

        /// <summary>
        /// The prompt(s) to generate completions for, encoded as a string, array of strings, array of tokens, or array of token arrays.
        /// </summary>
        /// <remarks>
        /// Note that <|endoftext|> is the document separator that the model sees during training, so if a prompt is not specified the model will generate as if from the beginning of a new document.
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("prompt")]
        public string? Prompt
        {
            get;
            set;
        }

        /// <summary>
        /// The maximum number of <see href="https://beta.openai.com/tokenizer">tokens</see> to generate in the completion.
        /// </summary>
        /// <remarks>The token count of your prompt plus <c><MaxTokens/c> cannot exceed the model's context length. Most models have a context length of 2048 tokens (except for the newest models, which support 4096).</remarks>
        [DefaultValue(16)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("max_tokens")]
        public int MaxTokens
        {
            get;
            set;
        }

        /// <summary>
        /// What <see href="https://towardsdatascience.com/how-to-sample-from-language-models-682bceb97277">sampling temperature</see> to use. Higher values means the model will take more risks. Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer.
        /// </summary>
        /// <remarks>We generally recommend altering this or <c>TopP</c> but not both.</remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("temperature")]
        public float Temperature
        {
            get;
            set;
        }

        /// <summary>
        /// An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered.
        /// </summary>
        /// <remarks>We generally recommend altering this or <c>Temperature</c> but not both.</remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("top_p")]
        public float TopP
        {
            get;
            set;
        }


        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens based on whether they appear in the text so far, increasing the model's likelihood to talk about new topics.
        /// </summary>
        /// <remarks><seealso href="https://beta.openai.com/docs/api-reference/parameter-details">See more information about frequency and presence penalites.</seealso> </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("presence_penalty")]
        public float PresencePenalty
        {
            get;
            set;
        }

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens based on their existing frequency in the text so far, decreasing the model's likelihood to repeat the same line verbatim.
        /// </summary>
        /// <remarks>
        /// <seealso href="https://beta.openai.com/docs/api-reference/parameter-details">See more information about frequency and presence penalites.</seealso>
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("frequency_penalty")]
        public float FrequencyPenalty
        {
            get;
            set;
        }

    }
}
