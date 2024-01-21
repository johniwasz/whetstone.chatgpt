using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models.FineTuning
{
    public class ChatGPTTurboFineTuneLine
    {

        [JsonPropertyName("messages")]
        public List<ChatGPTTurboFineTuneLineMessage> Messages { get; set; } = [];

    }
}
