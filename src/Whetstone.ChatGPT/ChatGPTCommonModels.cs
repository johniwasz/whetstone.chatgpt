// SPDX-License-Identifier: MIT
namespace Whetstone.ChatGPT
{
    /// <summary>
    /// The name of the base model to fine-tune. You can select one of "ada", "babbage", "curie", "davinci", or a fine-tuned model created after 2022-04-21.
    /// </summary>
    /// <remarks>
    /// <para>See <see cref="Models.ChatGPTCreateFineTuneRequest.Model">Fine Tune Model</see> for more information.</para>
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

        /// <summary>
        /// Snapshot of gpt-4 from March 14th 2023. Unlike <c>gpt-4</c>, this model will not receive updates, and will only be supported for a three month period ending on June 14th 2023.
        /// </summary>
        /// <remarks>
        /// <para>Max Tokens: 8,192</para>
        /// <para>Training Data: Up to Sep 2021</para>
        /// <para><see href="https://platform.openai.com/docs/models/gpt-4">GPT-4</see></para>
        /// </remarks>
        [Obsolete("This model is not longer supported.")]
        public readonly static string GPT4_0314 = "gpt-4-0314";

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
        /// Snapshot of <c>gpt-4-32</c> from March 14th 2023. Unlike <c>gpt-4-32</c>, this model will not receive updates, and will only be supported for a three month period ending on June 14th 2023.
        /// </summary>
        /// <remarks>
        /// <para>Max Tokens: 32,768</para>
        /// <para>Training Data: Up to March 2023</para>
        /// <para><see href="https://platform.openai.com/docs/models/gpt-4">GPT-4</see></para>
        /// </remarks>
        [Obsolete("This model is not supported as of 2023-06-14.")]
        public readonly static string GPT4_32K_0314 = "gpt-4-32k-0314";


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

    /// <summary>
    /// GPT-3.5 models can understand and generate natural language or code. Our most capable and cost effective model in the GPT-3.5 family is <c>gpt-3.5-turbo</c> which has been optimized for chat but works well for traditional completions tasks as well.
    /// </summary>
    /// <remarks>
    /// <para>
    /// We recommend using gpt-3.5-turbo over the other GPT-3.5 models because of its lower cost.
    /// </para>
    /// <para>
    /// OpenAI models are non-deterministic, meaning that identical inputs can yield different outputs. Setting <see href="https://platform.openai.com/docs/api-reference/completions/create#completions/create-temperature">temperature</see> to 0 will make the outputs mostly deterministic, but a small amount of variability may remain.
    /// </para>
    /// <para><see href="https://platform.openai.com/docs/models/gpt-3-5">GPT-3.5</see></para>
    /// </remarks>
    public static class ChatGPT35Models
    {
        /// <summary>
        /// Most capable GPT-3.5 model and optimized for chat at 1/10th the cost of <c>text-davinci-003</c>. Will be updated with our latest model iteration.
        /// </summary>
        /// <remarks>
        /// <para>Max Tokens: 4,097</para>
        /// <para>Training Data: Up to June 2021</para>
        /// <para><see href="https://platform.openai.com/docs/models/gpt-3-5">GPT-3.5</see></para>
        /// </remarks>
        public readonly static string Turbo = "gpt-3.5-turbo";

        /// <summary>
        /// Snapshot of <c>gpt-3.5-turbo</c> from March 1st 2023. Unlike <c>gpt-3.5-turbo</c>, this model will not receive updates, and will only be supported for a three month period ending on June 1st 2023
        /// </summary>
        /// <remarks>
        /// <para>Max Tokens: 4,097</para>
        /// <para>Training Data: Up to June 2021</para>
        /// <para><see href="https://platform.openai.com/docs/models/gpt-3-5">GPT-3.5</see></para>
        /// </remarks>
        [Obsolete("This model will not be supported as of 2023-09-13. Please use gpt4 instead.")]
        public readonly static string Turbo0301 = "gpt-3.5-turbo-0301";

        /// <summary>
        /// Can do any language task with better quality, longer output, and consistent instruction-following than the curie, babbage, or ada models. Also supports inserting completions within text.
        /// </summary>
        /// <remarks>
        /// <para>Max Tokens: 4,097</para>
        /// <para>Training Data: Up to June 2021</para>
        /// <para><see href="https://platform.openai.com/docs/models/gpt-3-5">GPT-3.5</see></para>
        /// </remarks>
        public readonly static string Davinci003 = "text-davinci-003";

        /// <summary>
        /// Similar capabilities to <c><text-davinci-003</c> but trained with supervised fine-tuning instead of reinforcement learning
        /// </summary>
        /// <remarks>
        /// <para>Max Tokens: 4,097</para>
        /// <para>Training Data: Up to June 2021</para>
        /// <para><see href="https://platform.openai.com/docs/models/gpt-3-5">GPT-3.5</see></para>
        /// </remarks>
        public readonly static string Davinci002 = "text-davinci-002";

        /// <summary>
        /// Optimized for code-completion tasks
        /// </summary>
        /// <remarks>
        /// <para>Max Tokens: 8,001</para>
        /// <para>Training Data: Up to June 2021</para>
        /// <para><see href="https://platform.openai.com/docs/models/gpt-3-5">GPT-3.5</see></para>
        /// </remarks>
        [Obsolete("This model is no longer supported as of 2023-03-23. Please use gpt4 instead.")]
        public readonly static string CodeDavinci002 = "code-davinci-002";

        /// <summary>
        /// Same function calling as GPT-4 as well as more reliable steerability via the system message, two features that allow developers to guide the model's responses more effectively.
        /// </summary>
        public readonly static string Turbo0613 = "gpt-3.5-turbo-0613";

        /// <summary>
        /// Offers 4 times the context length of gpt-3.5-turbo at twice the price: $0.003 per 1K input tokens and $0.004 per 1K output tokens. 16k context means the model can now support ~20 pages of text in a single request.
        /// </summary>
        public readonly static string Turbo16k = "gpt-3.5-turbo-16k";
    }

    /// <summary>
    /// Provides a set of avaialbe completion models. 
    /// </summary>
    /// <remarks>
    /// <para>See <see href="https://beta.openai.com/docs/models/overview">Models Overview</see>.</para>
    /// </remarks>
    public static class ChatGPTCompletionModels
    {
        /// <summary>
        /// Very capable, but faster and lower cost than Davinci.
        /// </summary>
        /// <remarks>
        /// <para>Curie is extremely powerful, yet very fast. While Davinci is stronger when it comes to analyzing complicated text, Curie is quite capable for many nuanced tasks like sentiment classification and summarization. Curie is also quite good at answering questions and performing Q&A and as a general service chatbot.</para>
        /// <para>Good at: Language translation, complex classification, text sentiment, summarization</para>
        /// <para><see href="https://beta.openai.com/docs/models/overview">Models Overview</see></para>
        /// </remarks>
        public readonly static string Curie = "text-curie-001";

        /// <summary>
        /// Capable of straightforward tasks, very fast, and lower cost.
        /// </summary>
        /// <remarks>
        /// <para>Babbage can perform straightforward tasks like simple classification. It’s also quite capable when it comes to Semantic Search ranking how well documents match up with search queries.</para>
        /// <para>Good at: Moderate classification, semantic search classification</para>
        /// </remarks>
        /// <para><see href="https://beta.openai.com/docs/models/overview">Models Overview</see></para>
        public readonly static string Babbage = "text-babbage-001";

        /// <summary>
        /// Capable of very simple tasks, usually the fastest model in the GPT-3 series, and lowest cost.
        /// </summary>
        /// <remarks>
        /// <para>Ada is usually the fastest model and can perform tasks like parsing text, address correction and certain kinds of classification tasks that don’t require too much nuance. Ada’s performance can often be improved by providing more context.</para>
        /// <para>Good at: Parsing text, simple classification, address correction, keywords</para>
        /// <para>Note: Any task performed by a faster model like Ada can be performed by a more powerful model like Curie or Davinci.</para>
        /// <para><see href="https://beta.openai.com/docs/models/overview">Models Overview</see></para>
        /// </remarks>
        public readonly static string Ada = "text-ada-001";
    }

    public static class ChatGPTEmbeddingModels
    {
        public readonly static string Ada = "text-embedding-ada-002";

        [Obsolete("This model will not be supported as of 2024-01-04. Please use text-embedding-ada-002 instead.")]
        public readonly static string AdaSearch = "text-search-ada-doc-001";
    }
    
    public static class ChatGPTMessageRoles
    {
        public readonly static string User = "user";

        public readonly static string System = "system";

        public readonly static string Assistant = "assistant";

    }

}
