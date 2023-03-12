using Microsoft.AspNetCore.Components;
using Whetstone.ChatGPT.Blazor.App.State;
using Whetstone.ChatGPT.Models;
using Blazorise;

namespace Whetstone.ChatGPT.Blazor.App.Components
{
    public partial class LogIn
    {

        [CascadingParameter]
        public ApplicationState AppState { get; set; } = default!;

        private ChatGPTCredentials credentials { get; set; } = new();

        private bool authenticationSucceeded = false;

        public Exception? exception { get; set; } = default!;
        
        private Blazorise.Bootstrap5.Modal? loginDialog { get; set; } = default!;

        private Validations? validations = default!;
        private bool isLoading { get; set; } = false;

        private bool rememberMe = false;

        protected override async Task OnInitializedAsync()
        {
            if (AppState is not null)
            {
                AppState.OnChange += StateHasChanged;
            }

            rememberMe = await credentialValidator.GetStoreCredentialsLocalOption();

            ChatGPTCredentials foundCreds = await credentialValidator.GetCredentialsAsync();

            if(foundCreds is not null)
            {
                credentials = foundCreds;
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

                isLoading = true;

                try
                {
                    authenticationSucceeded = await credentialValidator.ValidateCredentialsAsync(credentials, AppState!, rememberMe);
                }
                catch (ChatGPTException chatEx)
                {
                    exception = chatEx;
                    authenticationSucceeded = false;
                }
                catch (Exception ex)
                {
                    exception = ex;
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
            if (AppState is not null)
            {
                AppState.OnChange -= StateHasChanged;
            }
        }

    }
}
