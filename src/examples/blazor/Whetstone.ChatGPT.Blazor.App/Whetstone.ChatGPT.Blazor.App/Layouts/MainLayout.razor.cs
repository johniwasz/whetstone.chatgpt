using Blazorise;
using Blazorise.Localization;
using Microsoft.AspNetCore.Components;
using Whetstone.ChatGPT.Blazor.App.State;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Blazor.App.Layouts
{
    public partial class MainLayout
    {

        [CascadingParameter] 
        protected Theme? Theme { get; set; }


        [CascadingParameter]
        protected ApplicationState AppState { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}