using Blazorise;
using Blazorise.Localization;
using Microsoft.AspNetCore.Components;
using System.Net;
using Whetstone.ChatGPT.Blazor.App.State;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Blazor.App.Layouts
{
    public partial class MainLayout
    {
        [CascadingParameter]
        public ApplicationState AppState { get; set; } = new();

        
        [CascadingParameter] 
        protected Theme? Theme { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await credentialValidator.ValidateStoredCredentialsAsync(AppState);
            
            await base.OnInitializedAsync();
        }
    }
}