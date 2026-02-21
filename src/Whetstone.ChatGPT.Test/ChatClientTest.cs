// SPDX-License-Identifier: MIT
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models;
using Xunit;
using Xunit.Abstractions;

namespace Whetstone.ChatGPT.Test
{
    public class ChatClientTest
    {

        private readonly ITestOutputHelper _testOutputHelper;

        public ChatClientTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
        }

        [Fact]
        public async Task GPTModelsList()
        {

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                ChatGPTListResponse<ChatGPTModel>? modelResponse = await client.ListModelsAsync();
                Assert.NotNull(modelResponse);

                Assert.Equal("list", modelResponse.Object);

                Assert.NotNull(modelResponse.Data);

                Assert.NotEmpty(modelResponse.Data);
            }
        }


        [Fact]
        public async Task GPTModelsListWithHttpClient()
        {

            IOptions<ChatGPTCredentials> credsOptions = new OptionsWrapper<ChatGPTCredentials>(new ChatGPTCredentials
            {
                ApiKey = ChatGPTTestUtilties.GetChatGPTKey()
            });

            using (HttpClient httpClient = new HttpClient())
            {
                using (IChatGPTClient client = new ChatGPTClient(credsOptions, httpClient))
                {
                    ChatGPTListResponse<ChatGPTModel>? modelResponse = await client.ListModelsAsync();

                    Assert.NotNull(modelResponse);

                    Assert.NotNull(modelResponse.Data);

                    Assert.NotEmpty(modelResponse.Data);
                }
            }

        }



        [Fact]
        public async Task GPTModel()
        {

            IChatGPTClient client = ChatGPTTestUtilties.GetClient();

            var model = await client.RetrieveModelAsync(ChatGPTCompletionModels.Gpt35TurboInstruct);

            Assert.NotNull(model);

        }


        [Fact]
        public void NullSessionConstruction()
        {
            IChatGPTClient constructedClient = new ChatGPTClient((string?)null);
            Assert.NotNull(constructedClient);
        }

        /*
        [Fact]
        public void NullHttpClientConstruction()
        {
            string apiKey = ChatGPTTestUtilties.GetChatGPTKey();

            HttpClient? client = null;

            Assert.Throws<ArgumentNullException>(() => new ChatGPTClient(apiKey, client));
        }
        */

    }
}
