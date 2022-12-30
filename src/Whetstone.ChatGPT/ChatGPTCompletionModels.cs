using System;


namespace Whetstone.ChatGPT;


/// <summary>
/// Provides a set of avaialbe completion models. 
/// </summary>
/// <remarks>
/// <para>See <see href="https://beta.openai.com/docs/models/overview">Models Overview</see>.</para>
/// </remarks>
public class ChatGPTCompletionModels
{

    /// <summary>
    /// Most capable GPT-3 model. Can do any task the other models can do, often with higher quality, longer output and better instruction-following. Also supports <see href="https://beta.openai.com/docs/guides/completion/inserting-text">inserting</see> completions within text.
    /// </summary>
    /// <remarks>
    /// <para>Davinci is the most capable model family and can perform any task the other models can perform and often with less instruction. For applications requiring a lot of understanding of the content, like summarization for a specific audience and creative content generation, Davinci is going to produce the best results. These increased capabilities require more compute resources, so Davinci costs more per API call and is not as fast as the other models.</para>
    /// <para>Another area where Davinci shines is in understanding the intent of text. Davinci is quite good at solving many kinds of logic problems and explaining the motives of characters. Davinci has been able to solve some of the most challenging AI problems involving cause and effect.</para>
    /// <para>Good at: Complex intent, cause and effect, summarization for audience</para>
    /// <para><see href="https://beta.openai.com/docs/models/overview">Models Overview</see></para>
    /// </remarks>
    public readonly static string Davinci = "text-davinci-003";

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
