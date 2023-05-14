// SPDX-License-Identifier: MIT
using Blazorise;
using Microsoft.AspNetCore.Components;
using Whetstone.ChatGPT.Blazor.App.State;

namespace Whetstone.ChatGPT.Blazor.App.Components.Layout
{
    public partial class SideMenu
    {
        [CascadingParameter]
        public ApplicationState AppState { get; set; } = default!;

        public Visibility IsVisible
        {
            get
            {
                return AppState.IsOpenAIAuthenticated ? Visibility.Visible : Visibility.Invisible;
            }
            set
            {
            }
        }

        protected override void OnInitialized()
        {
            if (AppState is not null)
            {
                AppState.OnChange += StateHasChanged;
            }

            base.OnInitialized();
        }

        ~SideMenu()
        {
            AppState.OnChange -= StateHasChanged;
        }
    }
}
