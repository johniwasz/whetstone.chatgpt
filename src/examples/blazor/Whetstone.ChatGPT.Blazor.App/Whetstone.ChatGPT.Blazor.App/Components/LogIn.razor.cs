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
        private ChatGPTCredentials credentials { get; set; } = new();

        private bool authenticationSucceeded = false;

        public Exception? exception { get; set; } = default!;

        private Blazorise.Bootstrap5.Modal? loginDialog { get; set; } = default!;

        private Validations? validations = default!;

        public void Show()
        {
            if (loginDialog is not null)
            {
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
                try
                {
                    ChatClient.Credentials = credentials;
                    var fineTunes = await ChatClient.ListFineTunesAsync();
                    ApplicationState.IsOpenAIAuthenticated = true;
                    authenticationSucceeded = true;
                }
                catch (ChatGPTException chatEx)
                {
                    exception = chatEx;
                    ApplicationState.IsOpenAIAuthenticated = false;
                    authenticationSucceeded = false;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    ApplicationState.IsOpenAIAuthenticated = false;
                    authenticationSucceeded = false;
                }
            }
        }

        public void OnErrorNotificationClosed()
        {
            exception = null;
        }

    }
}
