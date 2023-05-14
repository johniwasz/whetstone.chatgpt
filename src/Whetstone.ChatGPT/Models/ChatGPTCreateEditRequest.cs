// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
{

    /// <summary>
    /// Request to create a new edit for the provided input, instruction, and parameters
    /// </summary>
    public class ChatGPTCreateEditRequest
    {

        /// <summary>
        /// ID of the model to use. You can use the <see href="https://beta.openai.com/docs/api-reference/models/list">List models</see> API to see all of your available models, or see our <see href="https://beta.openai.com/docs/models/overview">Model overview</see> for descriptions of them.
        /// </summary>
        /// <remarks>
        /// Required. See <see cref="ChatGPTCompletionModels">ChatGPTCompletionModels</see> for recommended completion models.
        /// </remarks>
        [JsonPropertyOrder(0)]        
        [JsonPropertyName("model")]
        [JsonInclude]
        public string? Model
        {
            get;
            set;
        }

        /// <summary>
        /// The input text to use as a starting point for the edit.
        /// </summary>
        [JsonPropertyOrder(1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("input")]
        public string? Input
        {
            get;
            set;
        }


        /// <summary>
        /// The instruction that tells the model how to edit the prompt.
        /// </summary>
        /// <remarks>
        /// Required.
        /// </remarks>
        [JsonPropertyOrder(2)]
        [JsonPropertyName("instruction")]
        [JsonInclude]
        public string? Instruction
        {
            get;
            set;
        }


        /// <summary>
        /// How many edits to generate for the input and instruction.
        /// </summary>
        /// <remarks>
        /// If this parameter generates many edits it can consume your token quota.
        /// </remarks>
        [JsonPropertyOrder(3)]
        [DefaultValue(1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("n")]
        public int EditResponses
        {
            get;
            set;
        }

        /// <summary>
        /// What <see href="https://towardsdatascience.com/how-to-sample-from-language-models-682bceb97277">sampling temperature</see> to use. Higher values means the model will take more risks. Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer.
        /// </summary>
        /// <remarks>We generally recommend altering this or <c>TopP</c> but not both.</remarks>
        [JsonPropertyOrder(4)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("Temperature")]
        public float Temperature
        {
            get;
            set;
        }


        /// <summary>
        /// An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered.
        /// </summary>
        /// <remarks>We generally recommend altering this or <c>Temperature</c> but not both.</remarks>
        [JsonPropertyOrder(5)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("top_p")]
        public float TopP
        {
            get;
            set;
        }
    }
}
