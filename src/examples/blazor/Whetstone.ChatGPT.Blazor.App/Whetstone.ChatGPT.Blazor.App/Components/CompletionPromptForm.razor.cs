// SPDX-License-Identifier: MIT
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;
using System;
using Whetstone.ChatGPT.Blazor.App.Models;
using Whetstone.ChatGPT.Models;
using System.Runtime.CompilerServices;

namespace Whetstone.ChatGPT.Blazor.App.Components
{
    public partial class CompletionPromptForm : IDisposable
    {

        [CascadingParameter]
        public string Prompt
        {
            get;
            set;
        } = default!;

        [Parameter]
        public EventCallback<Exception> OnException { get; set; } = default!;


        [Parameter]
        public EventCallback<ChatGPTChatCompletionRequest> OnChatCompletionRequestedAsync { get; set; } = default!;

        [Parameter]
        public EventCallback<ChatGPTChatCompletionResponse> OnChatCompletionResponseAsync { get; set; } = default!;

        [Parameter]
        public Func<CompletionOptions> CompletionOptionsRetriever { get; set; } = default!;

        private CompletionPromptRequest completionRequest = new();

        private readonly CancellationTokenSource cancelTokenSource = new();

        private string placeholderText { get; set; } = "Write a tagline for an ice cream shop.";

        private bool isDisposed = false;
        private bool isLoading = false;


        protected override Task OnInitializedAsync()
        {
            if (!string.IsNullOrWhiteSpace(Prompt))
            {
                completionRequest.Prompt = Prompt;
            }
            return base.OnInitializedAsync();
        }

        private async Task HandleSubmitAsync()
        {
            CompletionOptions compOptions = CompletionOptionsRetriever();

            try
            {
                isLoading = true;

                if (cancelTokenSource.TryReset())
                {
                    if(compOptions.SelectedModel!.StartsWith("gpt-4") || compOptions.SelectedModel.StartsWith("gpt-3.5"))
                    {
                        ChatGPTChatCompletionRequest gptChatCompletionRequest = new()
                        {
                            Messages = new List<ChatGPTChatCompletionMessage>()
                            {
                                new ChatGPTChatCompletionMessage()
                                {
                                    Content = completionRequest.Prompt,
                                    Role = ChatGPTMessageRoles.System
                                }
                            },
                            Model = compOptions.SelectedModel,
                            MaxTokens = compOptions.MaxTokens,
                            Temperature = compOptions.Temperature,
                        };

                        await OnChatCompletionRequestedAsync.InvokeAsync(gptChatCompletionRequest);

                        ChatGPTChatCompletionResponse gptChatCompletionResponse = (await ChatClient.CreateChatCompletionAsync(gptChatCompletionRequest))!;

                        await OnChatCompletionResponseAsync.InvokeAsync(gptChatCompletionResponse);
                    }                   

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
