// SPDX-License-Identifier: MIT
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Whetstone.ChatGPT.Models
{
    public class ChatGPTChatCompletionRequest
    {

        public ChatGPTChatCompletionRequest()
        {
        }

        /// <summary>
        /// ID of the model to use. You can use the <see href="https://beta.openai.com/docs/api-reference/models/list">List models</see> API to see all of your available models, or see our <see href="https://beta.openai.com/docs/models/overview">Model overview</see> for descriptions of them.
        /// </summary>
        /// <remarks>
        /// See <see cref="ChatGPTCompletionModels">ChatGPTCompletionModels</see> for recommended completion models.
        /// </remarks>
        [JsonPropertyOrder(0)]
        [JsonInclude]
        [JsonPropertyName("model")]
        public string? Model
        {
            get;
            set;
        }

        /// <summary>
        /// The messages to generate chat completions for, in the chat format.
        /// </summary>
        [JsonPropertyOrder(1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("messages")]
        public List<ChatGPTChatCompletionMessage>? Messages
        {
            get;
            set;
        }

        /// <summary>
        /// The maximum number of <see href="https://beta.openai.com/tokenizer">tokens</see> to generate in the completion.
        /// </summary>
        /// <remarks>The token count of your prompt plus <c><MaxTokens/c> cannot exceed the model's context length. Most models have a context length of 2048 tokens (except for the newest models, which support 4096).</remarks>
        [JsonPropertyOrder(2)]
        [DefaultValue(16)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("max_tokens")]
        public int? MaxTokens { get; set; } = 16;


        [JsonPropertyOrder(3)]
        [DefaultValue(16)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("max_completion_tokens")]
        public int? MaxCompletionTokens { get; set; } = 16; 
        

        /// <summary>
        /// What <see href="https://towardsdatascience.com/how-to-sample-from-language-models-682bceb97277">sampling temperature</see> to use. Higher values means the model will take more risks. Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer.
        /// </summary>
        /// <remarks>We generally recommend altering this or <c>TopP</c> but not both.</remarks>
        [JsonPropertyOrder(4)]
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
        [JsonPropertyOrder(5)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("top_p")]
        public float TopP
        {
            get;
            set;
        }

        /// <summary>
        /// How many completions to generate for each prompt.
        /// </summary>
        /// <remarks>
        ///  Because this parameter generates many completions, it can quickly consume your token quota. Use carefully and ensure that you have reasonable settings for max_tokens and stop.
        /// </remarks>
        [JsonPropertyOrder(6)]
        [DefaultValue(1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("n")]
        public int CompletionResponses
        {
            get;
            set;
        }

        /// <summary>
        /// Whether to stream back partial progress. If set, tokens will be sent as data-only <seealso href="https://developer.mozilla.org/en-US/docs/Web/API/Server-sent_events/Using_server-sent_events#Event_stream_format">server-sent</seealso> events as they become available, with the stream terminated by a data: <c>[DONE] message.</c>
        /// </summary>
        [JsonPropertyOrder(7)]
        [DefaultValue(false)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("stream")]
        public bool Stream
        {
            get;
            set;
        }

        /// <summary>
        /// Up to 4 sequences where the API will stop generating further tokens. The returned text will not contain the stop sequence.
        /// </summary>   
        [JsonPropertyOrder(8)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("stop")]
        public List<string>? Stop
        {
            get;
            set;
        }

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens based on whether they appear in the text so far, increasing the model's likelihood to talk about new topics.
        /// </summary>
        /// <remarks><seealso href="https://beta.openai.com/docs/api-reference/parameter-details">See more information about frequency and presence penalites.</seealso> </remarks>
        [JsonPropertyOrder(9)]
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
        [JsonPropertyOrder(10)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("frequency_penalty")]
        public float FrequencyPenalty
        {
            get;
            set;
        }

        /// <summary>
        /// Modify the likelihood of specified tokens appearing in the completion.
        /// </summary>
        /// <remarks>
        /// <para>Accepts a json object that maps tokens (specified by their token ID in the GPT tokenizer) to an associated bias value from -100 to 100. You can use this tokenizer tool (which works for both GPT-2 and GPT-3) to convert text to token IDs. Mathematically, the bias is added to the logits generated by the model prior to sampling. The exact effect will vary per model, but values between -1 and 1 should decrease or increase likelihood of selection; values like -100 or 100 should result in a ban or exclusive selection of the relevant token.</para>
        /// <para>As an example, you can pass <c>{"50256": -100}</c> to prevent the <|endoftext|> token from being generated.</para>
        /// <seealso href="https://beta.openai.com/docs/guides/safety-best-practices/end-user-ids">End User Ids</seealso>
        /// </remarks>
        [JsonPropertyOrder(11)]
        [DefaultValue(null)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("logit_bias")]
        public Dictionary<string, int>? LogitBias
        {
            get;
            set;
        }

        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse
        /// </summary>
        [JsonPropertyOrder(12)]
        [DefaultValue(null)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("user")]
        public string? User
        {
            get;
            set;
        }

        /// <summary>
        /// This feature is in Beta. If specified, our system will make a best effort to sample deterministically, 
        /// such that repeated requests with the same seed and parameters should return the same result. 
        /// Determinism is not guaranteed, and you should refer to the `system_fingerprint` response parameter to 
        /// monitor changes in the backend.
        /// </summary>
        [DefaultValue(null)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("seed")]
        public int? Seed
        {
            get;
            set;
        }

        /// <summary>
        /// A list of tools the model may call.Currently, only functions are supported as a tool.Use this to provide a list of functions the model may generate JSON inputs for.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("tools")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<ChatGPTTool>? Tools { get; set; } = null;

        /// <summary>
        /// Controls which (if any) function is called by the model. 
        /// none means the model will not call a function and instead generates a message. 
        /// auto means the model can pick between generating a message or calling a function. 
        /// Specifying a particular function via {"type: "function", "function": {"name": "my_function"}} 
        /// forces the model to call that function.
        /// </summary>
        /// <remarks>`none` is the default when no functions are present. `auto` is the default if functions are present. Use <see cref="ChatGPTTool">ChatGPTTool</see> to invoke a function.</remarks>
        [DefaultValue(null)]
        [JsonPropertyName("tool_choice")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public dynamic? ToolChoice { get; set; } = null;
    }
}
