// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Test
{
    public class ChatCompletionTests
    {

        [Fact]
        public async Task TestGPT35Request()
        {
            var gptRequest = new ChatGPTChatCompletionRequest
            {
                Model = ChatGPT35Models.Turbo,
                Messages = new List<ChatGPTChatCompletionMessage>()
                    {
                        new ChatGPTChatCompletionMessage()
                        {
                            Role = ChatGPTMessageRoles.System,
                            Content = "You are a helpful assistant."
                        },
                        new ChatGPTChatCompletionMessage()
                        {
                            Role = ChatGPTMessageRoles.User,
                            Content = "Who won the world series in 2020?"
                        },
                        new ChatGPTChatCompletionMessage()
                        {
                            Role = ChatGPTMessageRoles.Assistant,
                            Content = "The Los Angeles Dodgers won the World Series in 2020."
                        },
                        new ChatGPTChatCompletionMessage()
                        {
                            Role = ChatGPTMessageRoles.User,
                            Content = "Where was it played?"
                        }
                    },
                Temperature = 0.9f,
                MaxTokens = 100
            };

            using IChatGPTClient client = ChatGPTTestUtilties.GetClient();
            
            var response = await client.CreateChatCompletionAsync(gptRequest);

            Assert.NotNull(response);

            ChatGPTChatCompletionMessage? message = response.GetMessage();

            Assert.NotNull(message);

            Assert.Equal(ChatGPTMessageRoles.Assistant, message.Role);

            Assert.NotNull(response.Choices);

            Assert.Single(response.Choices);

            Assert.Equal("stop", response.Choices[0].FinishReason);

            Assert.True(!string.IsNullOrWhiteSpace(response.GetCompletionText()));
        }

        [Fact]
        public async Task TestGPT35StreamRequest()
        {
            var gptRequest = new ChatGPTChatCompletionRequest
            {
                Model = ChatGPT35Models.Turbo,
                Messages = new List<ChatGPTChatCompletionMessage>()
                    {
                        new ChatGPTChatCompletionMessage()
                        {
                            Role = ChatGPTMessageRoles.System,
                            Content = "You are a helpful assistant."
                        },
                        new ChatGPTChatCompletionMessage()
                        {
                            Role = ChatGPTMessageRoles.User,
                            Content = "Who won the world series in 2020?"
                        },
                        new ChatGPTChatCompletionMessage()
                        {
                            Role = ChatGPTMessageRoles.Assistant,
                            Content = "The Los Angeles Dodgers won the World Series in 2020."
                        },
                        new ChatGPTChatCompletionMessage()
                        {
                            Role = ChatGPTMessageRoles.User,
                            Content = "Where was it played?"
                        }
                    },
                Temperature = 0.9f,
                MaxTokens = 100
            };

            using IChatGPTClient client = ChatGPTTestUtilties.GetClient();

            StringBuilder sb = new();

            await foreach (var completion in client.StreamChatCompletionAsync(gptRequest).ConfigureAwait(false))
            {
                if (completion is not null)
                {
                    string? responseText = completion.GetCompletionText();
                    Assert.NotNull(responseText);

                    sb.Append(responseText);
                }
            }

            string aggregateResponse = sb.ToString();
        }

        [Fact(Skip="Processing image too expensive for an every day test.")]
        public async Task TestGPTVisionRequest()
        {
            var gptRequest = new ChatGPTCompletionVisionRequest
            {
                Model = "gpt-4-vision-preview",
                Messages = new List<ChatGPTCompletionVisionMessage>()
                {
                    new()
                    {
                        Content =
                        [
                            new ChatGPTVisionTextContent()
                            {
                                Text = "What’s in this image?"
                            },
                            new ChatGPTVisionImageUrlContent()
                            {
                                ImageUrl = new ChatGPTVisionImageUrl()
                                {
                                    Url = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Gfp-wisconsin-madison-the-nature-boardwalk.jpg/2560px-Gfp-wisconsin-madison-the-nature-boardwalk.jpg"
                                }
                            }
                        ]
                    },
                },
                MaxTokens = 300
            };

            using IChatGPTClient client = ChatGPTTestUtilties.GetClient();

            var response = await client.CreateVisionCompletionAsync(gptRequest);

            Assert.NotNull(response);

            ChatGPTChatCompletionMessage? message = response.GetMessage();

            Assert.NotNull(message);

            Assert.Equal(ChatGPTMessageRoles.Assistant, message.Role);

            Assert.NotNull(response.Choices);

            Assert.Single(response.Choices);

            Assert.Equal("stop", response.Choices[0].FinishReason);

            Assert.True(!string.IsNullOrWhiteSpace(response.GetCompletionText()));
        }

    }
}
