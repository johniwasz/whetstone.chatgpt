using System.Net.NetworkInformation;
using System.Net;
using System;
using Blazorise;
using Whetstone.ChatGPT.Models;
using Microsoft.AspNetCore.Components;

namespace Whetstone.ChatGPT.Blazor.App.Components
{
    public partial class ChatOptionsSelector
    {

        public int MaxTokens { get; set; } = 200;

        public float Temperature { get; set; } = 0.1f;

        public string? SelectedModel { get; set; } = null;

        [Parameter]
        public EventCallback<Exception> OnException { get; set; }

        private IEnumerable<ChatGPTModel>? models = default!;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ChatGPTListResponse<ChatGPTModel>? listResponse = await ChatClient.ListModelsAsync();

                if(listResponse?.Data is not null)
                {
                    models = listResponse.Data.OrderBy(x => x.Id);
                }

                SelectedModel = ChatGPTCompletionModels.Davinci;
                
            }
            catch (Exception ex)
            {
                await OnException.InvokeAsync(ex);
            }

            await base.OnInitializedAsync();
        }

    }
}
