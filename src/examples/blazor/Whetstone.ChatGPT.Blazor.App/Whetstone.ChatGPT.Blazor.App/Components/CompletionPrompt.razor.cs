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

        private MarkupString? PromptResponse { get; set; } = default!;
        
        private ChatGPTUsage? completionUsage { get; set; } = default!;

        private Exception? exception { get; set; } = default!;

        private bool isLoading { get; set; } = false;
        
        private CompletionPromptRequest completionRequest = new();
        
        private ChatOptionsSelector? optionsSelector = default!;

        protected override void OnParametersSet()
        {
            completionRequest.Prompt = Prompt;
            base.OnParametersSet();
        }

        private async Task HandleSubmitAsync()
        {
            exception = null;

            ChatGPTCompletionRequest gptPromptRequest = new()
            {
                Prompt = completionRequest.Prompt,
                Model = optionsSelector is null ? ChatGPTCompletionModels.Davinci : optionsSelector.SelectedModel,
                MaxTokens = optionsSelector is null ? 200 : optionsSelector.MaxTokens,
                Temperature = optionsSelector is null ? 0.1f : optionsSelector.Temperature
            };

            try
            {
                isLoading = true;

                ChatGPTCompletionResponse? completionResponse = await ChatClient.CreateCompletionAsync(gptPromptRequest);

                if (completionResponse is not null)
                {
                    string? rawResponse = completionResponse.GetCompletionText();

                    if (rawResponse is not null)
                    {
                        PromptResponse = (MarkupString)rawResponse.Replace(Environment.NewLine, "<br/>");
                    }

                    completionUsage = completionResponse.Usage;

                    if (completionUsage is not null)
                    {
                        AppState.UpdateTokenUsage(completionUsage);
                    }
                }
            }
            catch (ChatGPTException chatEx)
            {
                exception = chatEx;
            }
            finally
            {
                isLoading = false;
            }
        }

        private void ProcessOptionsException(Exception ex)
        {
            exception = ex;
        }

    }
}
