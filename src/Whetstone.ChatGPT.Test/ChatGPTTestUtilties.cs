using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Test
{
    internal static class ChatGPTTestUtilties
    {
        internal static string GetChatGPTKey()
        {

            string? chatGPTKey = System.Environment.GetEnvironmentVariable(EnvironmentSettings.ENV_CHATGPT_KEY);

            if (string.IsNullOrWhiteSpace(chatGPTKey))
            {
                throw new Exception("ChatGPT Key not found in environment variables");
            }

            return chatGPTKey;
        }

    }
}
