// SPDX-License-Identifier: MIT
using System.Text.Json.Serialization;

namespace Whetstone.ChatGPT.Models.FineTuning
{

    /// <summary>
    /// A single line item in a fine tuning file.
    /// </summary>
    /// <remarks>
    /// <para>This is a single training prompt and completion.</para>
    /// <para>For `babbage-002` and `davinci-002`. </para>
    /// <para>See <see href="https://beta.openai.com/docs/guides/fine-tuning">Fine Tuning</see></para>
    /// </remarks>
    public class ChatGPTFineTuneLine
    {
        public ChatGPTFineTuneLine()
        {
        }

        public ChatGPTFineTuneLine(string prompt, string completion)
        {
            Prompt = prompt ?? throw new ArgumentNullException(nameof(prompt));
            Completion = completion ?? throw new ArgumentNullException(nameof(completion));
        }

        /// <summary>
        /// The prompt(s) to generate completions for, encoded as a string, array of strings, array of tokens, or array of token arrays.
        /// </summary>
        /// <remarks>
        /// Note that <|endoftext|> is the document separator that the model sees during training, so if a prompt is not specified the model will generate as if from the beginning of a new document.
        /// </remarks>
        [JsonPropertyName("prompt")]
        public string? Prompt { get; set; }

        /// <summary>
        /// Ideal generated text.
        /// </summary>
        [JsonPropertyName("completion")]
        public string? Completion { get; set; }

    }
}
