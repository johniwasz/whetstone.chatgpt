using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT
{
    public class ChatGPTCompletionModels
    {

        /// <summary>
        /// Most capable GPT-3 model. Can do any task the other models can do, often with higher quality, longer output and better instruction-following. Also supports inserting completions within text.
        /// </summary>
        public const string Davinci = "davinci";

        /// <summary>
        /// Very capable, but faster and lower cost than Davinci.
        /// </summary>
        public const string Curie = "text-curie-001";

        /// <summary>
        ///  Capable of straightforward tasks, very fast, and lower cost.
        /// </summary>
        public const string Babbage = "text-babbage-001";

        /// <summary>
        /// Capable of very simple tasks, usually the fastest model in the GPT-3 series, and lowest cost.
        /// </summary>
        public const string Ada = "text-ada-001";
    }
}
