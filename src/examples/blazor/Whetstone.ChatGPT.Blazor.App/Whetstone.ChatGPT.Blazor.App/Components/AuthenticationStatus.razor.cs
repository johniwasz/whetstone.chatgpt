
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

        public void PurgeCredentials()
        {
            AppState.IsOpenAIAuthenticated = false;
            ChatClient.Credentials = null;
        }

        ~AuthenticationStatus()
        {
            AppState.OnChange -= StateHasChanged;
        }
        
    }
}
