using Blazorise.LoadingIndicator;
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
        public string Title { get; set; }  = "Playground";

        [Parameter]
        public string Prompt { get; set; } = default!;

        [CascadingParameter]
        public ApplicationState AppState { get; set; } = default!;

        private string placeholderText { get; set; } = "Write a tagline for an ice cream shop.";

        private Blazorise.Bootstrap5.Button? submitButton = default!;

        private MarkupString? PromptResponse { get; set; } = default!;
        
        private ChatGPTUsage? completionUsage { get; set; } = default!;

        private Exception? exception { get; set; } = default!;

        private bool isLoading { get; set; } = false;

        private async Task SubmitPromptAsync()
        {
            exception = null;

            ChatGPTCompletionRequest promptRequest = new()
            {
                Prompt = Prompt,
                Model = ChatGPTCompletionModels.Davinci,
                MaxTokens = 200
            };

            try
            {
                if (submitButton is not null)
                {
                    isLoading = true;
                }

                ChatGPTCompletionResponse? completionResponse = await ChatClient.CreateCompletionAsync(promptRequest);

                if (completionResponse is not null)
                {
                    string? rawResponse = completionResponse.GetCompletionText();

                    if (rawResponse is not null)
                    {
                        PromptResponse = (MarkupString)rawResponse.Replace(Environment.NewLine, "<br/>");
                    }

                    completionUsage = completionResponse.Usage;

                    AppState.UpdateTokenUsage(completionUsage);

                }
            }
            catch (ChatGPTException chatEx)
            {
                exception = chatEx;
            }
            finally
            {
                if (submitButton is not null)
                {
                    isLoading = false;
                }
            }

        }
    }
}
