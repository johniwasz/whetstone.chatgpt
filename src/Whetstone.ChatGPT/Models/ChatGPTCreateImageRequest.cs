// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
{
    public enum CreatedImageSize
    {
        [EnumMember(Value = "256x256")]
        Size256,
        [EnumMember(Value = "512x512")]
        Size512,
        [EnumMember(Value = "1024x1024")]
        Size1024
    }

    public enum CreatedImageFormat
    {
        [EnumMember(Value = "url")]
        Url,
        [EnumMember(Value = "b64_json")]
        Base64
    }

    /// <summary>
    /// Request to create an image given a prompt.
    /// </summary>
    public class ChatGPTCreateImageRequest
    {
        /// <summary>
        /// A text description of the desired image(s). The maximum length is 1000 characters.
        /// </summary>
        [JsonPropertyName("prompt")]
        public string? Prompt { get; set; }

        /// <summary>
        /// The number of images to generate. Must be between 1 and 10.
        /// </summary>
        [DefaultValue(1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("n")]
        public int NumberOfImagesToGenerate { get; set; } = 1;


        /// <summary>
        /// The size of the generated images. Must be one of <c>256x256</c>, <c>512x512</c>, or <c>1024x1024</c>.
        /// </summary>        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [DefaultValue(CreatedImageSize.Size1024)]
        [JsonConverter(typeof(EnumConverter<CreatedImageSize>))]
        [JsonPropertyName("size")]
        public CreatedImageSize Size { get; set; } = CreatedImageSize.Size1024;


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [DefaultValue(CreatedImageFormat.Url)]
        [JsonConverter(typeof(EnumConverter<CreatedImageFormat>))]
        [JsonPropertyName("response_format")]
        public CreatedImageFormat ResponseFormat { get; set; } = CreatedImageFormat.Url;

        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.<see href="https://beta.openai.com/docs/guides/safety-best-practices/end-user-ids">Learn more.</see>
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("user")]
        public string? User { get; set; }
    }
}
