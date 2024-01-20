// SPDX-License-Identifier: MIT
using Microsoft.Extensions.Options;
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

            // Using Ada in this test to keep costs down.

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
            var gptRequest = new ChatGPTCompletionRequest
            {
                Model = ChatGPTCompletionModels.Gpt35TurboInstruct,
                Prompt = "How is the weather?",
                Temperature = 0.9f,
                MaxTokens = 10
            };

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

                // Using Ada in this test to keep costs down.
                await foreach(var completion in  client.StreamCompletionAsync(gptRequest).ConfigureAwait(false))
                {
                    if (completion is not null)
                    {
                        Assert.NotNull(completion.GetCompletionText());
                    }
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
