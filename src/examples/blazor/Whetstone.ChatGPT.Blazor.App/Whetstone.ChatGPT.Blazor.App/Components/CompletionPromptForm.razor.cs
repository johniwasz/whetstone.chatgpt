using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;
using System;
using Whetstone.ChatGPT.Blazor.App.Models;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Blazor.App.Components
{
    public partial class CompletionPromptForm : IDisposable
    {
        [Parameter]
        public string Prompt { 
            
            get
            {
                return completionRequest.Prompt;
            }
            set
            {
                completionRequest.Prompt = value;
            }
        }

        [Parameter]
        public EventCallback<Exception> OnException { get; set; } = default!;

        [Parameter]
        public EventCallback<ChatGPTCompletionRequest> OnCompletionRequestedAsync { get; set; } = default!;

        [Parameter]
        public EventCallback<ChatGPTCompletionResponse> OnCompletionResponseAsync { get; set; } = default!;
        
        [Parameter]
        public Func<CompletionOptions> CompletionOptionsRetriever { get; set; } = default!;

        private CompletionPromptRequest completionRequest = new();

        private ChatGPTCompletionRequest gptCompletionRequest = new();

        private ChatGPTCompletionResponse gptCompletionResponse = new();

        private readonly CancellationTokenSource cancelTokenSource = new();

        private string placeholderText { get; set; } = "Write a tagline for an ice cream shop.";

        private bool isDisposed = false;
        private bool isLoading = false; 

        private async Task HandleSubmitAsync()
        {
            CompletionOptions compOptions = CompletionOptionsRetriever();

            ChatGPTCompletionRequest gptCompletionRequest = new()
            {
                Prompt = completionRequest.Prompt,
                Model = compOptions.SelectedModel,
                MaxTokens = compOptions.MaxTokens,
                Temperature = compOptions.Temperature,
            };

            await OnCompletionRequestedAsync.InvokeAsync(gptCompletionRequest);

            try
            {
                isLoading = true;

                if (cancelTokenSource.TryReset())
                {
                    ChatGPTCompletionResponse gptCompletionResponse = (await ChatClient.CreateCompletionAsync(gptCompletionRequest))!;

                    await OnCompletionResponseAsync.InvokeAsync(gptCompletionResponse);
                }
            }
            catch (ThreadAbortException)
            {
                Logger.LogInformation("Completion request is cancelled.");
            }
            catch (Exception ex)
            {
                await OnException.InvokeAsync(ex);
            }
            finally
            {
                isLoading = false;
            }
        }
        protected override void OnParametersSet()
        {
            completionRequest.Prompt = Prompt;
            base.OnParametersSet();
        }

        private void CancelRequest()
        {
            cancelTokenSource.Cancel();
        }

        #region Clean Up
        ~CompletionPromptForm()
        {
            Dispose(true);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                cancelTokenSource.Cancel();
                cancelTokenSource.Dispose();
                isDisposed = true;
            }
        }
        #endregion

    }
}
