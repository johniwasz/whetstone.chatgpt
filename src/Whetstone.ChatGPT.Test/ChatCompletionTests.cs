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
                            Role = MessageRole.System,
                            Content = "You are a helpful assistant."
                        },
                        new ChatGPTChatCompletionMessage()
                        {
                            Role = MessageRole.User,
                            Content = "Who won the world series in 2020?"
                        },
                        new ChatGPTChatCompletionMessage()
                        {
                            Role = MessageRole.Assistant,
                            Content = "The Los Angeles Dodgers won the World Series in 2020."
                        },
                        new ChatGPTChatCompletionMessage()
                        {
                            Role = MessageRole.User,
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

            Assert.Equal(MessageRole.Assistant, message.Role);

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
                            Role = MessageRole.System,
                            Content = "You are a helpful assistant."
                        },
                        new ChatGPTChatCompletionMessage()
                        {
                            Role = MessageRole.User,
                            Content = "Who won the world series in 2020?"
                        },
                        new ChatGPTChatCompletionMessage()
                        {
                            Role = MessageRole.Assistant,
                            Content = "The Los Angeles Dodgers won the World Series in 2020."
                        },
                        new ChatGPTChatCompletionMessage()
                        {
                            Role = MessageRole.User,
                            Content = "Where was it played?"
                        }
                    },
                Temperature = 0.9f,
                MaxTokens = 100
            };

            using IChatGPTClient client = ChatGPTTestUtilties.GetClient();

            StringBuilder sb = new StringBuilder();

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

    }
}
