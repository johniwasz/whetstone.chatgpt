using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace Whetstone.TweetGPT.ChatGPTClient.Models;


public class ChatGPTCompletionRequest
{

    ///"{{\"prompt\": \"{prompt}\", \"max_tokens\": 64, \"temperature\": 0.9, \"top_p\": 1, \"frequency_penalty\": 0, \"presence_penalty\": 0, \"stop\": \"\\n\"}}
    public ChatGPTCompletionRequest()
    {

    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("model")]
    public string Model
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
    public string Prompt
    {
        get;
        set;
    }


    /// <summary>
    /// The maximum number of tokens to generate in the completion.
    /// </summary>
    /// <remarks>The token count of your prompt plus max_tokens cannot exceed the model's context length. Most models have a context length of 2048 tokens (except for the newest models, which support 4096).</remarks>
    [DefaultValue(16)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("max_tokens")]
    public int MaxTokens
    {
        get;
        set;
    }

    /// <summary>
    /// What sampling temperature to use. Higher values means the model will take more risks. Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer.
    /// </summary>
    /// <remarks>We generally recommend altering this or top_p but not both.</remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("temperature")]
    public float Temperature
    {
        get;
        set;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("top_p")]
    public int TopP
    {
        get;
        set;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("frequency_penalty")]
    public int FrequencyPenalty
    {
        get;
        set;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("presence_penalty")]
    public int PresencePenalty
    {
        get;
        set;
    }

    /// <summary>
    /// Up to 4 sequences where the API will stop generating further tokens. The returned text will not contain the stop sequence.
    /// </summary>
    [DefaultValue("\\n")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("stop")]
    public string Stop
    {
        get;
        set;
    }


}
