using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet.Frameworks;
using Whetstone.TweetGPT.ChatGPTClient;
using Whetstone.TweetGPT.ChatGPTClient.Models;
using Xunit.Sdk;

namespace Whetstone.TweetGPT.WebhookManager.Test
{
    public class ChatClientTest
    {
        

        [Fact]
        public async Task TestGPTClientCompletion()
        {
            string sessionToken = GetChatGPTKey();

            ChatClient client = new ChatClient(sessionToken);


            var gptRequest = new ChatGPTCompletionRequest
            {
                Model = "text-davinci-001",
                Prompt = "How is the weather?",
                Temperature = 0.9f,
                MaxTokens = 140
            };

            var response = await client.GetResponseAsync(gptRequest);

            Assert.NotNull(response);
            
            Assert.True(!string.IsNullOrWhiteSpace(response.Choices?[0]?.Text));

        }

        [Fact]
        public async Task GPTModelsList()
        {
            string sessionToken = GetChatGPTKey();
            ChatClient client = new ChatClient(sessionToken);

            ChatGPTModelsResponse modelResponse = await client.GetModelsAsync();

            Assert.NotNull(modelResponse.Data);
                
            Assert.NotEmpty(modelResponse.Data);
        }


        [Fact]
        public async Task GPTModel()
        {
            string sessionToken = GetChatGPTKey();
            ChatClient client = new ChatClient(sessionToken);

            var model = await client.GetModelAsync("text-davinci-003");

            Assert.NotNull(model);

            //Assert.Equal(model.Data. "text-davinci-001", model.Id);
        }

        private string GetChatGPTKey()
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
