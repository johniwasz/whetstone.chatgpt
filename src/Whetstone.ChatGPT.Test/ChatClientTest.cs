using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet.Frameworks;
using Whetstone.ChatGPT;
using Whetstone.ChatGPT.Models;
using Xunit.Sdk;

namespace Whetstone.ChatGPT.Test
{
    public class ChatClientTest
    {
        

        [Fact]
        public async Task TestGPTClientCompletion()
        {
            string apiKey = ChatGPTTestUtilties.GetChatGPTKey();

            ChatGPTClient client = new ChatGPTClient(apiKey);

            // Using Ada in this test to keep costs down.

            var gptRequest = new ChatGPTCompletionRequest
            {
                Model = ChatGPTCompletionModels.Ada,
                Prompt = "How is the weather?",
                Temperature = 0.9f,
                MaxTokens = 140
            };
            
            var response = await client.CreateCompletionAsync(gptRequest);

            Assert.NotNull(response);
            
            Assert.True(!string.IsNullOrWhiteSpace(response.GetCompletionText()));
            
        }

        [Fact]
        public async Task TestGPTClientEdit()
        {
            string apiKey = ChatGPTTestUtilties.GetChatGPTKey();

            ChatGPTClient client = new ChatGPTClient(apiKey);

            var gptEditRequest = new ChatGPTCreateEditRequest
            {             
                Input = "What day of the wek is it?",
                Instruction = "Fix spelling mistakes",
                Temperature = 0
            };

            var response = await client.CreateEditAsync(gptEditRequest);

            Assert.NotNull(response);

            Assert.Equal("edit", response.Object);

            string? editTextResponse = response.GetEditedText();

            Assert.Contains("What day of the week is it?", editTextResponse, StringComparison.InvariantCultureIgnoreCase);

        }

        [Fact]
        public async Task GPTModelsList()
        {
            string apiKey = ChatGPTTestUtilties.GetChatGPTKey();
            ChatGPTClient client = new ChatGPTClient(apiKey);

            ChatGPTModelsResponse? modelResponse = await client.GetModelsAsync();

            Assert.NotNull(modelResponse);

            Assert.Equal("list", modelResponse.Object);

            Assert.NotNull(modelResponse.Data);
                
            Assert.NotEmpty(modelResponse.Data);

            var textModels = modelResponse.Data.Where(x => x.Id is not null && x.Id.Contains("edit"));

            Assert.NotEmpty(textModels);

        }

        [Fact]
        public async Task GPTModelsListWithHttpClient()
        {
            string apiKey = ChatGPTTestUtilties.GetChatGPTKey();

            using (HttpClient httpClient = new())
            {
                ChatGPTClient client = new ChatGPTClient(apiKey, httpClient);
                
                ChatGPTModelsResponse? modelResponse = await client.GetModelsAsync();

                Assert.NotNull(modelResponse);

                Assert.NotNull(modelResponse.Data);

                Assert.NotEmpty(modelResponse.Data);
            }

        }


        [Fact]
        public async Task GPTModel()
        {
            string apiKey = ChatGPTTestUtilties.GetChatGPTKey();
            ChatGPTClient client = new ChatGPTClient(apiKey);

            var model = await client.RetrieveModelAsync("text-ada-001");

            Assert.NotNull(model);

        }



#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8604 // Possible null reference argument.

        [Fact]
        public void NullSessionConstruction()
        {
            Assert.Throws<ArgumentException>(() => new ChatGPTClient((string?) null));
        }

        [Fact]
        public void NullHttpClientConstruction()
        {
            string apiKey = ChatGPTTestUtilties.GetChatGPTKey();

            HttpClient? client = null;

            Assert.Throws<ArgumentNullException>(() => new ChatGPTClient(apiKey, client));
        }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
       
    }
}
