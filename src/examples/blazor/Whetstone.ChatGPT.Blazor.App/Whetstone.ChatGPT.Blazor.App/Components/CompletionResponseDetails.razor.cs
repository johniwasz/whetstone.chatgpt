// SPDX-License-Identifier: MIT
using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis.CSharp;
using System.Text.Encodings.Web;
using System.Text.Json;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Blazor.App.Components
{
    public partial class CompletionResponseDetails
    {

        [Parameter]
        public ChatGPTUsage CompletionUsage { get; set; } = new();

        [Parameter]
        public ChatGPTCompletionRequest? CompletionRequest { get; set; } = new();

        [Parameter]
        public ChatGPTCompletionResponse CompletionResponse { get; set; } = new();

        [Parameter]
        public ChatGPTChatCompletionRequest? CompletionChatRequest { get; set; } = new();

        [Parameter]
        public ChatGPTChatCompletionResponse CompletionChatResponse { get; set; } = new();

        private MarkupString completionCode = new();

        private MarkupString completionResponseJson = new();

        protected override void OnParametersSet()
        {
            if (CompletionRequest is not null)
            {
                completionCode = (MarkupString)Parse($$"""
                    ``` C#
                    ChatGPTCompletionRequest gptPromptRequest = new()
                    {
                        Prompt = {{ToQuotedLiteral(CompletionRequest.Prompt)}},
                        Model = "{{CompletionRequest.Model}}",
                        MaxTokens = {{CompletionRequest.MaxTokens}},
                        Temperature = {{CompletionRequest.Temperature}}f,
                    };
                    ```
                    """);
                string completionJson = JsonSerializer.Serialize(CompletionResponse);

                completionResponseJson = (MarkupString)completionJson;
            }

            if(CompletionChatRequest is not null)
            {
                completionCode = (MarkupString)Parse($$"""
                    ``` C#
                    ChatGPTChatCompletionRequest gptChatPromptRequest = new()
                    {
                        Messages = {{WriteMessages(CompletionChatRequest.Messages)}},
                        Model = "{{CompletionChatRequest.Model}}",
                        MaxTokens = {{CompletionChatRequest.MaxTokens}},
                        Temperature = {{CompletionChatRequest.Temperature}}f,
                    };
                    ```
                    """);

                string completionJson = JsonSerializer.Serialize(CompletionChatResponse);

                completionResponseJson = (MarkupString)completionJson;
            }


            base.OnParametersSet();
        }

        private string WriteMessages(List<ChatGPTChatCompletionMessage>? messages)
        {
            if (messages is null)
            {
                return "null";
            }

            string parsedMessages = "new List<ChatGPTChatCompletionMessage>() {\n";

            foreach (var message in messages)
            {
                parsedMessages += ($"                new ChatGPTChatCompletionMessage() {{ Role = \"{message.Role}\", Content = \"{message.Content}\"}},\n");
            }

            parsedMessages += ("              }");

            return parsedMessages;
        }

        private static string Parse(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();
            return Markdown.ToHtml(markdown, pipeline);
        }

        private static string ToQuotedLiteral(string? valueTextForCompiler)
        {
            if (valueTextForCompiler is null)
            {
                return "null";
            }
            return $"{SymbolDisplay.FormatLiteral(valueTextForCompiler, true)}";
        }

    }
}
