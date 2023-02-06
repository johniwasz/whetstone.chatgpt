using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.SimpleCommandLineBot
{
    internal static class CliUtilities
    {

        internal static string GetUsage()
        {

            StringBuilder helpTextBuilder = new StringBuilder();
            
            helpTextBuilder.AppendLine("Usage: chatgpt-marv [OPTIONS]");

            helpTextBuilder.AppendLine();

            helpTextBuilder.AppendLine("Run a sample chatbot using the OpenAI ChatGPT API");

            helpTextBuilder.AppendLine();

            helpTextBuilder.AppendLine("options:");
            helpTextBuilder.AppendLine("  --apikey [APIKEY]                         The OpenAI API Key to use. If not specified, the environment variable OPENAI_API_KEY will be used.");
            helpTextBuilder.AppendLine("  --organization [ORGANIZATION]             OpenAI organization. If not specified, the environment variable OPEN_API_ORGANIZATION is used");
            helpTextBuilder.AppendLine();
            return helpTextBuilder.ToString();
        }

        internal static string? GetChatGPTAPIKey(IConfigurationRoot config)
        {
            string? apiKey = config["apikey"];

            if (config["apikey"] is null)
            {
                apiKey = config[EnvironmentSettings.ENV_CHATGPT_KEY];
            }
            
            return apiKey;
        }

        internal static bool IsExitInput(string? input)
        {
            if (input is null)
            {
                return false;
            }

            return input.Equals("exit", StringComparison.OrdinalIgnoreCase);
        }
    }
}
