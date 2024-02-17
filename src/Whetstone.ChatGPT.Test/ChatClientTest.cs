// SPDX-License-Identifier: MIT
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using Whetstone.ChatGPT.Models;
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
        public async Task TestGPTClientCompletion()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var gptRequest = new ChatGPTCompletionRequest
            {
                Model = ChatGPTCompletionModels.Gpt35TurboInstruct,
                Prompt = "How is the weather?",
                Temperature = 0.9f,
                MaxTokens = 10
            };


            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

                var response = await client.CreateCompletionAsync(gptRequest);

                Assert.NotNull(response);

                Assert.True(!string.IsNullOrWhiteSpace(response.GetCompletionText()));
#pragma warning restore CS0618 // Type or member is obsolete
            }
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

            using (HttpClient httpClient = new())
            {
                IChatGPTClient client = new ChatGPTClient(credsOptions, httpClient);

                ChatGPTListResponse<ChatGPTModel>? modelResponse = await client.ListModelsAsync();

                Assert.NotNull(modelResponse);

                Assert.NotNull(modelResponse.Data);

                Assert.NotEmpty(modelResponse.Data);
            }

        }

        [Fact]
        public async Task TestGPTClientStreamCompletion()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var gptRequest = new ChatGPTCompletionRequest
            {
                Model = ChatGPTCompletionModels.Gpt35TurboInstruct,
                Prompt = "How is the weather?",
                Temperature = 0.9f,
                MaxTokens = 10
            };
#pragma warning restore CS0618 // Type or member is obsolete

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

                // Using Ada in this test to keep costs down.
#pragma warning disable CS0618 // Type or member is obsolete
                await foreach(var completion in  client.StreamCompletionAsync(gptRequest).ConfigureAwait(false))
                {
                    if (completion is not null)
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        Assert.NotNull(completion.GetCompletionText());
#pragma warning restore CS0618 // Type or member is obsolete
                    }
                }
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        [Fact]
        public async Task GPTModel()
        {

            IChatGPTClient client = ChatGPTTestUtilties.GetClient();

            var model = await client.RetrieveModelAsync(ChatGPTCompletionModels.Gpt35TurboInstruct);

            Assert.NotNull(model);

        }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8604 // Possible null reference argument.

        [Fact]
        public void NullSessionConstruction()
        {
            IChatGPTClient constructedClient = new ChatGPTClient((string?) null);
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
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
       
    }
}
