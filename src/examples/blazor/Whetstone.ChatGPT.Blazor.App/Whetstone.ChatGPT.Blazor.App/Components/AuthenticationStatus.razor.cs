// SPDX-License-Identifier: MIT

using Microsoft.AspNetCore.Components;
using Whetstone.ChatGPT.Blazor.App.State;

namespace Whetstone.ChatGPT.Blazor.App.Components
{
    public partial class AuthenticationStatus
    {

        [CascadingParameter]
        public ApplicationState AppState { get; set; } = default!;

        private LogIn? loginModal;

        protected override void OnInitialized()
        {
            AppState.OnChange += StateHasChanged;
            base.OnInitialized();
        }

        private void ShowLogin()
        {
            if (loginModal is not null)
            {
                loginModal?.Show();
            }
        }

        public async Task PurgeCredentialsAsync()
        {
            await credentialValidator.PurgeCredentialsAsync(AppState);
            Navigation.NavigateTo(Navigation.BaseUri);
        }

        ~AuthenticationStatus()
        {
            AppState.OnChange -= StateHasChanged;
        }
        
    }
}
