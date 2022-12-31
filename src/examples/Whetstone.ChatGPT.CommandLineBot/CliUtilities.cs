using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Test;

namespace Whetstone.ChatGPT.CommandLineBot
{
    internal static class CliUtilities
    {

        internal static string GetUsage()
        {

            StringBuilder helpTextBuilder = new StringBuilder();

            var currentAsmName = System.Reflection.Assembly.GetExecutingAssembly();

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

        internal static ChatGPTCredentials? GetChatGPTCredentials(IConfigurationRoot config)
        {            
            string? apiKey = GetChatGPTAPIKey(config);
            string? organization = GetOrganization(config);

            return apiKey == null ? 
                (ChatGPTCredentials?) null : 
                new ChatGPTCredentials(apiKey, organization);
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

        internal static string? GetOrganization(IConfigurationRoot configuration)
        {
            string? organization = configuration["organization"];
            if (organization is null)
            {
                organization = configuration[EnvironmentSettings.ENV_CHATGPT_ORGANIZATION];
            }
            
            return organization;
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
