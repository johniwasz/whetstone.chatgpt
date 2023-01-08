using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NuGet.Frameworks;
using Whetstone.ChatGPT;
using Whetstone.ChatGPT.Models;
using Xunit.Abstractions;
using Xunit.Sdk;

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
            
            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

                // Using Ada in this test to keep costs down.

                var gptRequest = new ChatGPTCompletionRequest
                {
                    Model = ChatGPTCompletionModels.Ada,
                    Prompt = "How is the weather?",
                    Temperature = 0.9f,
                    MaxTokens = 10
                };

                var response = await client.CreateCompletionAsync(gptRequest);

                Assert.NotNull(response);

                Assert.True(!string.IsNullOrWhiteSpace(response.GetCompletionText()));
            }
        }

        [Fact]
        public async Task TestGPTClientEdit()
        {

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

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

                // This is a beta and has been known to return a completion response
                // instead of an edit response. 
                // It responded with "TIS MONDAY" on a failed test run.
                // For now, mismatches will result in a warning message.
                // rather than a failure.

                Assert.NotNull(editTextResponse);

                Assert.False(string.IsNullOrWhiteSpace(editTextResponse));

                string expectedText = "What day of the week is it?";

                if (!editTextResponse.Contains(expectedText))
                {
                    _testOutputHelper.WriteLine($"Expected '{expectedText}'. Returned: {editTextResponse}");
                }
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

                var textModels = modelResponse.Data.Where(x => x.Id is not null && x.Id.Contains("edit"));

                Assert.NotEmpty(textModels);
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

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

                // Using Ada in this test to keep costs down.

                var gptRequest = new ChatGPTCompletionRequest
                {
                    Model = ChatGPTCompletionModels.Ada,
                    Prompt = "How is the weather?",
                    Temperature = 0.9f,
                    MaxTokens = 10
                };

                await foreach(var completion in  client.StreamCompletionAsync(gptRequest).ConfigureAwait(false))
                {
                    if(completion is not null)
                        Assert.NotNull(completion.GetCompletionText());
                }
            }
        }


        [Fact]
        public async Task GPTModel()
        {

            IChatGPTClient client = ChatGPTTestUtilties.GetClient();

            var model = await client.RetrieveModelAsync(ChatGPTCompletionModels.Ada);

            Assert.NotNull(model);

        }



#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8604 // Possible null reference argument.

        [Fact]
        public void NullSessionConstruction()
        {
            Assert.Throws<ArgumentException>(() => new ChatGPTClient((string?) null));
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
