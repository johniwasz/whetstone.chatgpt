// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Whetstone.ChatGPT.Test
{
    internal static class ChatGPTTestUtilties
    {
        internal static string GetChatGPTKey()
        {
#if NETFRAMEWORK
            string chatGPTKey = System.Environment.GetEnvironmentVariable(EnvironmentSettings.ENV_CHATGPT_KEY);
#else
            string? chatGPTKey = System.Environment.GetEnvironmentVariable(EnvironmentSettings.ENV_CHATGPT_KEY);
#endif
            if (string.IsNullOrWhiteSpace(chatGPTKey))
            {
                throw new Exception("ChatGPT Key not found in environment variables");
            }

            return chatGPTKey;
        }


        internal static IChatGPTClient GetClient()
        {

            string chatGPTKey = GetChatGPTKey();

            return new ChatGPTClient(chatGPTKey);
        }

    }
}
