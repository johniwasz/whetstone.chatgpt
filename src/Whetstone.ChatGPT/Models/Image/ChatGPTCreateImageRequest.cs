// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models.Image
{
    public enum CreatedImageSize
    {
        [EnumMember(Value = "256x256")]
        Size256x256,
        [EnumMember(Value = "512x512")]
        Size512x512,
        [EnumMember(Value = "1024x1024")]
        Size1024x1024,
        [EnumMember(Value = "1024x1536")]
        Size1024x1536,
        [EnumMember(Value = "1536x1024")]
        Size1536x1024,
        [EnumMember(Value = "1792x1024")]
        Size1792x1024,
        [EnumMember(Value = "1024x1792")]
        Size1024x1792,
        [EnumMember(Value = "auto")]
        Auto
    }

    public enum CreatedImageFormat
    {
        [EnumMember(Value = "url")]
        Url,
        [EnumMember(Value = "b64_json")]
        Base64
    }

    public enum ImageBackground
    {
        [EnumMember(Value = "transparent")]
        Transparent,
        [EnumMember(Value = "opaque")]
        Opaque,
        [EnumMember(Value = "auto")]
        Auto
    }

    public enum ImageModeration
    {
        [EnumMember(Value = "low")]
        Low,
        [EnumMember(Value = "auto")]
        Auto
    }

    public enum ImageOutputFormat
    {
        [EnumMember(Value = "png")]
        Png,
        [EnumMember(Value = "jpeg")]
        Jpeg,
        [EnumMember(Value = "webp")]
        Webp
    }

    public enum ImageQuality
    {
        [EnumMember(Value = "auto")]
        Auto,
        [EnumMember(Value = "standard")]
        Standard,
        [EnumMember(Value = "hd")]
        Hd,
        [EnumMember(Value = "low")]
        Low,
        [EnumMember(Value = "medium")]
        Medium,
        [EnumMember(Value = "high")]
        High
    }

    public enum ImageStyle
    {
        [EnumMember(Value = "vivid")]
        Vivid,
        [EnumMember(Value = "natural")]
        Natural
    }



    /// <summary>
    /// Request to create an image given a prompt.
    /// </summary>
    public class ChatGPTCreateImageRequest
    {
        /// <summary>
        /// A text description of the desired image(s). The maximum length is 32000 characters for GPT image models, 1000 characters for dall-e-2, and 4000 characters for dall-e-3.
        /// </summary>
        [JsonPropertyName("prompt")]
        public string? Prompt { get; set; }

        /// <summary>
        /// The model to use for image generation. One of dall-e-2, dall-e-3, or a GPT image model (gpt-image-1, gpt-image-1-mini, gpt-image-1.5). Defaults to dall-e-2 unless a parameter specific to the GPT image models is used.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("model")]
        public string? Model { get; set; }

        /// <summary>
        /// The number of images to generate. Must be between 1 and 10. For dall-e-3, only n=1 is supported.
        /// </summary>
        [DefaultValue(1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("n")]
        public int NumberOfImagesToGenerate { get; set; } = 1;

        /// <summary>
        /// The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024 for dall-e-2; one of 1024x1024, 1792x1024, or 1024x1792 for dall-e-3; and one of 1024x1024, 1536x1024, 1024x1536, or auto for GPT image models.
        /// </summary>        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(EnumConverter<CreatedImageSize>))]
        [JsonPropertyName("size")]
        public CreatedImageSize? Size { get; set; }

        /// <summary>
        /// The quality of the image that will be generated. auto (default) will automatically select the best quality for the given model. high, medium, and low are supported for GPT image models. hd and standard are supported for dall-e-3. standard is the only option for dall-e-2.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(EnumConverter<ImageQuality>))]
        [JsonPropertyName("quality")]
        public ImageQuality? Quality { get; set; }

        /// <summary>
        /// The format in which generated images with dall-e-2 and dall-e-3 are returned. Must be one of url or b64_json. URLs are only valid for 60 minutes after the image has been generated. This parameter isn't supported for the GPT image models, which always return base64-encoded images.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(EnumConverter<CreatedImageFormat>))]
        [JsonPropertyName("response_format")]
        public CreatedImageFormat? ResponseFormat { get; set; }

        /// <summary>
        /// The format in which the generated images are returned. This parameter is only supported for the GPT image models. Must be one of png, jpeg, or webp.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(EnumConverter<ImageOutputFormat>))]
        [JsonPropertyName("output_format")]
        public ImageOutputFormat? OutputFormat { get; set; }

        /// <summary>
        /// The compression level (0-100%) for the generated images. This parameter is only supported for the GPT image models with the webp or jpeg output formats, and defaults to 100.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("output_compression")]
        public int? OutputCompression { get; set; }

        /// <summary>
        /// Allows to set transparency for the background of the generated image(s). This parameter is only supported for the GPT image models. Must be one of transparent, opaque or auto (default value). When auto is used, the model will automatically determine the best background for the image.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(EnumConverter<ImageBackground>))]
        [JsonPropertyName("background")]
        public ImageBackground? Background { get; set; }

        /// <summary>
        /// Control the content-moderation level for images generated by the GPT image models. Must be either low for less restrictive filtering or auto (default value).
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(EnumConverter<ImageModeration>))]
        [JsonPropertyName("moderation")]
        public ImageModeration? Moderation { get; set; }

        /// <summary>
        /// The style of the generated images. This parameter is only supported for dall-e-3. Must be one of vivid or natural. Vivid causes the model to lean towards generating hyper-real and dramatic images. Natural causes the model to produce more natural, less hyper-real looking images.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(EnumConverter<ImageStyle>))]
        [JsonPropertyName("style")]
        public ImageStyle? Style { get; set; }

        /// <summary>
        /// Generate the image in streaming mode. Defaults to false. This parameter is only supported for the GPT image models.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("stream")]
        public bool? Stream { get; set; }

        /// <summary>
        /// The number of partial images to generate. This parameter is used for streaming responses that return partial images. Value must be between 0 and 3. When set to 0, the response will be a single image sent in one streaming event.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("partial_images")]
        public int? PartialImages { get; set; }

        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("user")]
        public string? User { get; set; }
    }
}
