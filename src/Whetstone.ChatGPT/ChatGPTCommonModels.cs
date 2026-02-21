// SPDX-License-Identifier: MIT
using System.Runtime.Serialization;

namespace Whetstone.ChatGPT
{
    /// <summary>
    /// The name of the base model to fine-tune. You can select one of "ada", "babbage", "curie", "davinci", or a fine-tuned model created after 2022-04-21.
    /// </summary>
    /// <remarks>

    /* Unmerged change from project 'Whetstone.ChatGPT (net8.0)'
    Before:
        /// <para>See <see cref="Models.ChatGPTCreateFineTuneRequest.Model">Fine Tune Model</see> for more information.</para>
    After:
        /// <para>See <see cref="ChatGPTCreateFineTuneRequest.Model">Fine Tune Model</see> for more information.</para>
    */
    /// <para>See <see cref="FineTuning.ChatGPTCreateFineTuneRequest.Model">Fine Tune Model</see> for more information.</para>
    /// </remarks>
    public static class ChatGPTFineTuneModels
    {
        public const string Ada = "ada";

        public const string Babage = "babbage";

        public const string Curie = "curie";

        public const string Davinci = "davinci";
    }

    /// <summary>
    /// Currently, the only text edit model supported is Davinci.
    /// </summary>
    public static class ChatGPTEditModels
    {
        /// <summary>
        /// Currently, the only edit model supported is Davinci.
        /// </summary>
        public readonly static string Davinci = "text-davinci-edit-001";

        public readonly static string DavinciCode = "code-davinci-edit-001";
    }

#pragma warning disable CA1707 // Identifiers should not contain underscores
    public static class ChatGPT5Models
    {
        /// <summary>
        /// Complex reasoning, broad world knowledge, and code-heavy or multi-step agentic tasks.
        /// </summary>
        public static readonly string GPT_52 = "gpt-5.2";

        /// <summary>
        /// Tough problems that may take longer to solve but require harder thinking
        /// </summary>
        public static readonly string GPT_52_PRO = "gpt-5.2-pro";

        /// <summary>
        /// Companies building interactive coding products; full spectrum of coding tasks
        /// </summary>
        public static readonly string GPT_52_CODEX = "gpt-5.2-codex";

        /// <summary>
        /// Cost-optimized reasoning and chat; balances speed, cost, and capability
        /// </summary>
        public static readonly string GPT_5_MINI = "gpt-5-mini";

        /// <summary>
        /// High-throughput tasks, especially simple instruction-following or classification
        /// </summary>
        public static readonly string GPT_5_NANO = "gpt-5-nano";
    }
#pragma warning restore CA1707 // Identifiers should not contain underscores

#pragma warning disable CA1707 // Identifiers should not contain underscores
    /// <summary>
    /// GPT-4 is a large multimodal model (accepting text inputs and emitting text outputs today, with image inputs coming in the future) that can solve difficult problems with greater accuracy than any of our previous models, thanks to its broader general knowledge and advanced reasoning capabilities. Like <c>gpt-3.5-turbo</c>, GPT-4 is optimized for chat but works well for traditional completions tasks.
    /// </summary>
    /// <remarks>
    /// <para>
    /// GPT-4 is currently in a limited beta and only accessible to those who have been granted access. Please <see href="https://openai.com/waitlist/gpt-4"?>join the waitlist</see> to get access when capacity is available.
    /// </para>
    /// <para>
    /// For many basic tasks, the difference between GPT-4 and GPT-3.5 models is not significant. However, in more complex reasoning situations, GPT-4 is much more capable than any of our previous models.
    /// </para>
    /// <para><see href="https://platform.openai.com/docs/models/gpt-4">GPT-4</see></para>
    /// </remarks>
    public static class ChatGPT4Models
    {
        /// <summary>
        /// More capable than any GPT-3.5 model, able to do more complex tasks, and optimized for chat. Will be updated with our latest model iteration.
        /// </summary>
        /// <remarks>
        /// <para>Max Tokens: 8,192</para>
        /// <para>Training Data: Up to Sep 2021</para>
        /// <para><see href="https://platform.openai.com/docs/models/gpt-4">GPT-4</see></para>
        /// </remarks>
        public readonly static string GPT4 = "gpt-4";

        public readonly static string GPT4o = "gpt-4o";


        /// <summary>
        /// Same capabilities as the base <c>gpt-4</c> mode but with 4x the context length. Will be updated with our latest model iteration.
        /// </summary>
        /// <remarks>
        /// <para>Max Tokens: 32,768</para>
        /// <para>Training Data: Up to Sep 2021</para>
        /// <para><see href="https://platform.openai.com/docs/models/gpt-4">GPT-4</see></para>
        /// </remarks>
        public readonly static string GPT4_32K = "gpt-4-32k";

        /// <summary>
        /// Snapshot of <c>gpt-4-32</c> from June 13th 2023. Unlike <c>gpt-4-32</c>, this model will not receive updates, and will only be supported for a three month period ending.
        /// </summary>
        /// <remarks>
        /// <para>Max Tokens: 32,768</para>
        /// <para>Training Data: Up to June 2023</para>
        /// <para><see href="https://platform.openai.com/docs/models/gpt-4">GPT-4</see></para>
        /// </remarks>
        public readonly static string GPT4_32K_0613 = "gpt-4-32k-0613";

        /// <summary>
        /// Includes an updated and improved model with function calling.
        /// </summary>
        public readonly static string GPT4_0613 = "gpt-4-0613";
    }
#pragma warning restore CA1707 // Identifiers should not contain underscores

    public static class ChatGPTImageModels
    {

        public static readonly string Dall_E_2 = "dall-e-2";
        
        public static readonly string Dall_E_3 = "dall-e-3";

        public static readonly string Gpt_Image_1 = "gpt-image-1";

        public static readonly string Gpt_Image_1_Mini = "gpt-image-1-mini";

        public static readonly string Gpt_Image_1_5 = "gpt-image-1.5";
    }


    /// <summary>
    /// Provides a set of avaialbe completion models. 
    /// </summary>
    /// <remarks>
    /// <para>See <see href="https://beta.openai.com/docs/models/overview">Models Overview</see>.</para>
    /// </remarks>
    public static class ChatGPTCompletionModels
    {

        public readonly static string Gpt35TurboInstruct = "gpt-3.5-turbo-instruct";
    }

    public static class ChatGPTEmbeddingModels
    {
        public readonly static string Ada = "text-embedding-ada-002";

    }
    
    public static class ChatGPTMessageRoles
    {
        public readonly static string User = "user";

        public readonly static string System = "system";

        public readonly static string Assistant = "assistant";

        public readonly static string Tool = "tool";

    }

}
