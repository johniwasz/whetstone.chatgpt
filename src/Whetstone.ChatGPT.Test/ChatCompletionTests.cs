// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models;
using Whetstone.ChatGPT.Models.Image;
using Whetstone.ChatGPT.Models.Vision;
using Xunit;

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

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                var response = await client.CreateChatCompletionAsync(gptRequest);

                Assert.NotNull(response);

#if NETFRAMEWORK
                ChatGPTChatCompletionMessage message = response.GetMessage();
#else
                ChatGPTChatCompletionMessage? message = response.GetMessage();
#endif
                Assert.NotNull(message);

                Assert.Equal(ChatGPTMessageRoles.Assistant, message.Role);

                Assert.NotNull(response.Choices);

                Assert.Single(response.Choices);

                Assert.Equal("stop", response.Choices[0].FinishReason);

                Assert.True(!string.IsNullOrWhiteSpace(response.GetCompletionText()));
            }
        }




        [Fact]
        public async Task TestWeatherToolRequest()
        {
            var tools = new List<ChatGPTTool>()
            {
                new ChatGPTTool()
                {
                    Type = "function",
                    Function = new ChatGPTFunction ()
                    {
                        Name = "get_current_weather",
                        Description = "Get the current weather",
                        Parameters =
                            new ChatGPTParameter()
                            {
                                Type = "object",
                                Properties = new Dictionary<string, ChatGPTParameter>()
                                {
                                    {"location", new ChatGPTParameter()
                                        {   Type = "string",
                                            Description = "The city and state, e.g. San Francisco, CA," } },
                                    {"format", new ChatGPTParameter()
                                        {  Type = "string",
                                           Enum = new List<string>() { "celsius", "fahrenheit" },
                                           Description = "The temperature unit to use. Infer this from the users location." } },
                                }
                        },
                        Required = new List<string> { "location", "format" }
                    }
                },
                new ChatGPTTool()
                {
                    Type = "function",
                    Function = new ChatGPTFunction ()
                    {
                        Name = "get_n_day_weather_forecast",
                        Description = "Get an N-day weather forecast",
                        Parameters =
                          new ChatGPTParameter()
                          {
                                Type = "object",
                                Properties = new Dictionary<string, ChatGPTParameter>()
                                {
                                    {"location", new ChatGPTParameter()
                                        {   Type = "string",
                                            Description = "The city and state, e.g. San Francisco, CA," } },
                                    {"format", new ChatGPTParameter()
                                        {  Type = "string",
                                            Enum = new List<string>() { "celsius", "fahrenheit" },
                                            Description = "The temperature unit to use. Infer this from the users location." } },
                                    {"num_days", new ChatGPTParameter()
                                        {  Type = "integer",
                                            Description = "The number of days to forecast" } },
                                }

                    },
                    Required = new List<string> {"location", "format", "num_days" }
                    }
                },
            };


            var gptRequest = new ChatGPTChatCompletionRequest
            {
                Model = ChatGPT35Models.Turbo,
                Messages = new List<ChatGPTChatCompletionMessage>()
                    {
                        new ChatGPTChatCompletionMessage()
                        {
                            Role = ChatGPTMessageRoles.System,
                            Content = "Don't make assumptions about what values to plug into functions. Ask for clarification if a user request is ambiguous."
                        },
                        new ChatGPTChatCompletionMessage()
                        {
                            Role = ChatGPTMessageRoles.User,
                            Content = "What is the weather like today?"
                        }
                    },
                Tools = tools,
                Temperature = 0.9f,
                MaxTokens = 100
            };

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                var response = await client.CreateChatCompletionAsync(gptRequest);

                Assert.NotNull(response);
#if NETFRAMEWORK
                ChatGPTChatCompletionMessage message = response.GetMessage();
#else
                ChatGPTChatCompletionMessage? message = response.GetMessage();
#endif
                Assert.NotNull(message);

                Assert.Equal(ChatGPTMessageRoles.Assistant, message.Role);

                Assert.NotNull(response.Choices);

                Assert.Single(response.Choices);

                Assert.Equal("tool_calls", response.Choices[0].FinishReason);

                // Assert.True(!string.IsNullOrWhiteSpace(response.GetCompletionText()));
            }
        }

#if !NETFRAMEWORK
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

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

                StringBuilder sb = new StringBuilder();

                await foreach (var completion in client.StreamChatCompletionAsync(gptRequest).ConfigureAwait(false))
                {
                    if (!(completion is null))
                    {

                        string? responseText = completion.GetCompletionText();

                        Assert.NotNull(responseText);

                        sb.Append(responseText);
                    }
                }

                string aggregateResponse = sb.ToString();
            }
        }
#endif
        
        [Fact(Skip="Processing image too expensive for an every day test.")]
        public async Task TestGPTVisionRequest()
        {
            var gptRequest = new ChatGPTCompletionVisionRequest
            {
                Model = "gpt-4-vision-preview",
                Messages = new List<ChatGPTCompletionVisionMessage>()
                {
                    new ChatGPTCompletionVisionMessage()
                    {
                        Content = new List<object>()
                        {
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
                        }
                    },
                },
                MaxTokens = 300
            };

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

                var response = await client.CreateVisionCompletionAsync(gptRequest);

                Assert.NotNull(response);

#if NETFRAMEWORK
                ChatGPTChatCompletionMessage message = response.GetMessage();
#else
                ChatGPTChatCompletionMessage? message = response.GetMessage();
#endif
                Assert.NotNull(message);

                Assert.Equal(ChatGPTMessageRoles.Assistant, message.Role);

                Assert.NotNull(response.Choices);

                Assert.Single(response.Choices);

                Assert.Equal("stop", response.Choices[0].FinishReason);

                Assert.True(!string.IsNullOrWhiteSpace(response.GetCompletionText()));
            }
        }
    }
}
