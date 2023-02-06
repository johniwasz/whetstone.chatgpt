using System.Net.NetworkInformation;
using System.Net;
using System;
using Blazorise;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Blazor.App.Components
{
    public partial class ChatOptionsSelector
    {

        public int MaxTokens { get; set; } = 200;

        public float Temperature { get; set; } = 0.1f;

        public string? SelectedModel { get; set; } = null;

        // <Select @ref = "ModelList" @bind-SelectedValue="@SelectedModel">

        private Select<string?> modelList = default!;

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
            catch (ChatGPTException chatEx)
            {
            //    exception = chatEx;
                
            }
            catch (Exception ex)
            {
              //  exception = ex;
            }
            finally
            {
            
            }

            await base.OnInitializedAsync();
        }

    }
}
