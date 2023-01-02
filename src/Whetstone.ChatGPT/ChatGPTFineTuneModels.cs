using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
