using Microsoft.AspNetCore.Components;
using Whetstone.ChatGPT.Blazor.App.State;
using Whetstone.ChatGPT.Models;
using Blazorise;

namespace Whetstone.ChatGPT.Blazor.App.Components
{
    public partial class LogIn
    {

        [CascadingParameter]
        public ApplicationState? AppState { get; set; } = default!;

        private ChatGPTCredentials credentials { get; set; } = new();

        private bool authenticationSucceeded = false;

        public Exception? exception { get; set; } = default!;

        private Blazorise.Bootstrap5.Modal? loginDialog { get; set; } = default!;

        private Validations? validations = default!;
        private bool isLoading { get; set; } = false;

        protected override void OnInitialized()
        {
            if (AppState is not null)
            {
                AppState.OnChange += StateHasChanged;
            }
            
            base.OnInitialized();
        }

        public void Show()
        {
            if (loginDialog is not null)
            {
                validations?.ClearAll();

                authenticationSucceeded = AppState is null ? false : AppState.IsOpenAIAuthenticated;

                this.exception = null;

                loginDialog.Show();
            }
        }

        public void Hide()
        {
            if (loginDialog is not null)
            {
                loginDialog.Hide();

            }
        }

        private async Task ValidateCredentialsAsync()
        {
            exception = null;

            if (validations is not null && await validations.ValidateAll())
            {

                if (AppState is not null)
                {
                    AppState.IsOpenAIAuthenticated = false;
                }

                isLoading = true;

                try
                {
                    ChatClient.Credentials = credentials;
                    var fineTunes = await ChatClient.ListFineTunesAsync();
                    if (AppState is not null)
                    {
                        AppState.IsOpenAIAuthenticated = true;
                    }
                    
                    authenticationSucceeded = true;
                }
                catch (ChatGPTException chatEx)
                {
                    exception = chatEx;
                    if (AppState is not null)
                    {
                        AppState.IsOpenAIAuthenticated = false;
                    }
                    authenticationSucceeded = false;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    if (AppState is not null)
                    {
                        AppState.IsOpenAIAuthenticated = false;
                    }
                    authenticationSucceeded = false;
                }
                finally
                {
                    isLoading = false;
                }
            }
        }

        public void OnErrorNotificationClosed()
        {
            exception = null;
        }

        ~LogIn()
        {
            AppState.OnChange -= StateHasChanged;
        }

    }
}
