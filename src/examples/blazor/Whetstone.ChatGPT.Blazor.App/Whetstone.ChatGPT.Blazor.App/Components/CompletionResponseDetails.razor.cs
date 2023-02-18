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
        public ChatGPTCompletionRequest CompletionRequest { get; set; } = new();

        [Parameter]
        public ChatGPTCompletionResponse CompletionResponse { get; set; } = new();

        private MarkupString completionCode = new();

        private MarkupString completionResponseJson = new();

        protected override void OnParametersSet()
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

            base.OnParametersSet();
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
