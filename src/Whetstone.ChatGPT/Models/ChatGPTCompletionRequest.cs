using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace Whetstone.ChatGPT.Models;


/// <summary>
/// Given a prompt, the model will return one or more predicted completions, and can also return the probabilities of alternative tokens at each position.
/// </summary>
public class ChatGPTCompletionRequest
{

    /// <summary>
    /// Initialize the completion request.
    /// </summary>
    /// <remarks>
    /// Given a prompt, the model will return one or more predicted completions, and can also return the probabilities of alternative tokens at each position.
    /// </remarks>
    public ChatGPTCompletionRequest()
    {

    }

    /// <summary>
    /// ID of the model to use. You can use the <see href="https://beta.openai.com/docs/api-reference/models/list">List models</see> API to see all of your available models, or see our <see href="https://beta.openai.com/docs/models/overview">Model overview</see> for descriptions of them.
    /// </summary>
    /// <remarks>
    /// See <see cref="ChatGPTCompletionModels">ChatGPTCompletionModels</see> for recommended completion modesl.
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
    /// The prompt(s) to generate completions for, encoded as a string, array of strings, array of tokens, or array of token arrays.
    /// </summary>
    /// <remarks>
    /// Note that <|endoftext|> is the document separator that the model sees during training, so if a prompt is not specified the model will generate as if from the beginning of a new document.
    /// </remarks>
    [JsonPropertyOrder(1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("prompt")]
    public string? Prompt
    {
        get;
        set;
    }

    /// <summary>
    /// The suffix that comes after a completion of inserted text.
    /// </summary>
    [JsonPropertyOrder(2)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("suffix")]
    public string? Suffix
    {
        get;
        set;
    }


    /// <summary>
    /// The maximum number of <see href="https://beta.openai.com/tokenizer">tokens</see> to generate in the completion.
    /// </summary>
    /// <remarks>The token count of your prompt plus <c><MaxTokens/c> cannot exceed the model's context length. Most models have a context length of 2048 tokens (except for the newest models, which support 4096).</remarks>
    [JsonPropertyOrder(3)]
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
    public int TopP
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
    /// Include the log probabilities on the logprobs most likely tokens, as well the chosen tokens.
    /// </summary>
    /// <remarks>
    /// For example, if logprobs is 5, the API will return a list of the 5 most likely tokens. The API will always return the logprob of the sampled token, so there may be up to logprobs+1 elements in the response.
    /// The maximum value for logprobs is 5. If you need more than this, please contact us through our Help center and describe your use case.
    /// </remarks>
    [JsonPropertyOrder(8)]
    [DefaultValue(null)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("logprobs")]
    public int? LogProbabilities
    {
        get;
        set;
    }

    /// <summary>
    /// Echo back the prompt in addition to the completion
    /// </summary>
    [JsonPropertyOrder(9)]
    [DefaultValue(false)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("echo")]
    public bool Echo
    {
        get;
        set;
    }

    /// <summary>
    /// Up to 4 sequences where the API will stop generating further tokens. The returned text will not contain the stop sequence.
    /// </summary>   
    [JsonPropertyOrder(10)]
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
    [JsonPropertyOrder(11)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("presence_penalty")]
    public int PresencePenalty
    {
        get;
        set;
    }

    /// <summary>
    /// Number between -2.0 and 2.0. Positive values penalize new tokens based on their existing frequency in the text so far, decreasing the model's likelihood to repeat the same line verbatim.
    /// </summary>
    /// <remarks>
    // <seealso href="https://beta.openai.com/docs/api-reference/parameter-details">See more information about frequency and presence penalites.</seealso>
    /// </remarks>
    [JsonPropertyOrder(12)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("frequency_penalty")]
    public int FrequencyPenalty
    {
        get;
        set;
    }

    /// <summary>
    /// Generates <c>BestOf</c> completions server-side and returns the "best" (the one with the highest log probability per token). Results cannot be streamed.
    /// </summary>
    /// <remarks>
    /// <para>When used with <c>N</c>, <c>BestOf</c> controls the number of candidate completions and n specifies how many to return – <c>BestOf</c> must be greater than v.</para>
    /// <para></para>Note: Because this parameter generates many completions, it can quickly consume your token quota.Use carefully and ensure that you have reasonable settings for <c>MaxTokens</c> and <c>Stop</c>.</para>
    /// </remarks>
    [JsonPropertyOrder(13)]
    [DefaultValue(1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("best_of")]
    public int BestOf
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
    [JsonPropertyOrder(14)]
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
    [JsonPropertyOrder(15)]
    [DefaultValue(null)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("user")]
    public string? User
    {
        get;
        set;
    }
}