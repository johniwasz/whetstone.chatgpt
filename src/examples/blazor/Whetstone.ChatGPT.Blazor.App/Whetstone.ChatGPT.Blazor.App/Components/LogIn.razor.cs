using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System.Reflection;
using Whetstone.ChatGPT.Blazor.App.State;
using Whetstone.ChatGPT.Models;
using Blazorise.Bootstrap5;
using Blazorise;

namespace Whetstone.ChatGPT.Blazor.App.Components
{
    public partial class LogIn
    {

        [CascadingParameter]
        public ApplicationState AppState { get; set; } 

        private ChatGPTCredentials credentials { get; set; } = new();

        private bool authenticationSucceeded = false;

        public Exception? exception { get; set; } = default!;

        private Blazorise.Bootstrap5.Modal? loginDialog { get; set; } = default!;

        private Validations? validations = default!;

        private Blazorise.Bootstrap5.Button submitButton = default!;


        protected override void OnInitialized()
        {
            AppState.OnChange += StateHasChanged;

            base.OnInitialized();
        }

        public void Show()
        {
            if (loginDialog is not null)
            {
                validations?.ClearAll();

                authenticationSucceeded = AppState.IsOpenAIAuthenticated;

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

                AppState.IsOpenAIAuthenticated = false;
                
                if (submitButton is not null)
                    submitButton.Loading = true;

                try
                {
                    ChatClient.Credentials = credentials;
                    var fineTunes = await ChatClient.ListFineTunesAsync();
                    AppState.IsOpenAIAuthenticated = true;
                    authenticationSucceeded = true;
                }
                catch (ChatGPTException chatEx)
                {
                    exception = chatEx;
                    AppState.IsOpenAIAuthenticated = false;
                    authenticationSucceeded = false;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    AppState.IsOpenAIAuthenticated = false;
                    authenticationSucceeded = false;
                }
                finally
                {
                    if (submitButton is not null)
                        submitButton.Loading = false;
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
