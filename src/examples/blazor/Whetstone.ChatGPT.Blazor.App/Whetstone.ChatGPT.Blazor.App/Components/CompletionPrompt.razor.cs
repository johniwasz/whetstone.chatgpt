using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using Whetstone.ChatGPT.Blazor.App.Models;
using Whetstone.ChatGPT.Blazor.App.State;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Blazor.App.Components
{
    public partial class CompletionPrompt
    {
        [Parameter]
        public string Title { get; set; } = "Playground";

        [Parameter]
        public string Prompt 
        {
            get; set;
        } = default!;

        [CascadingParameter]
        public ApplicationState AppState { get; set; } = default!;

        [Parameter]
        public EventCallback<ChatGPTCompletionResponse> OnCompletionResponseAsync { get; set; } = default!;

        [Parameter]
        public Func<string, MarkupString> FormatPromptResponse { get; set; } = FormatResponse;

        [Parameter]
        public int DefaultMaxTokens { get; set; } = 200;

        private MarkupString? PromptResponse { get; set; } = default!;

        private ChatGPTUsage? completionUsage { get; set; } = default!;

        private ChatGPTCompletionRequest gptCompletionRequest = default!;

        private ChatGPTCompletionResponse gptCompletionResponse = default!;

        private Exception? exception { get; set; } = default!;

        private ChatOptionsSelector? optionsSelector = default!;

        public Task CompletionRequestedAsync(ChatGPTCompletionRequest completionRequest)
        {
            gptCompletionRequest = completionRequest;
            return Task.CompletedTask;
        }

        public Task ProcessCompletionResponseAsync(ChatGPTCompletionResponse completionResponse)
        {
            gptCompletionResponse = completionResponse;

            string? rawResponse = gptCompletionResponse.GetCompletionText();

            if (rawResponse is not null)
            {
                PromptResponse = FormatPromptResponse(rawResponse);
            }

            completionUsage = gptCompletionResponse.Usage;

            if (completionUsage is not null)
            {
                AppState.UpdateTokenUsage(completionUsage);
            }

            OnCompletionResponseAsync.InvokeAsync(completionResponse);

            return Task.CompletedTask;
        }

        private static MarkupString FormatResponse(string response)
        {

            return (MarkupString)response.Replace(Environment.NewLine, "<br/>");
        }

        private void ProcessException(Exception ex)
        {
            exception = ex;
        }

        public CompletionOptions GetCompletionOptions()
        {
            CompletionOptions compOptions = new()
            {
                SelectedModel = optionsSelector is null ? ChatGPT35Models.Davinci003 : optionsSelector.SelectedModel,
                MaxTokens = optionsSelector is null ? 200 : optionsSelector.MaxTokens,
                Temperature = optionsSelector is null ? 0.1f : optionsSelector.Temperature
            };

            return compOptions;
        }

    }
}
