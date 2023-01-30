﻿
using Blazorise.Bootstrap5;
using Microsoft.AspNetCore.Components;
using System.Net.NetworkInformation;
using Whetstone.ChatGPT.Blazor.App.State;




namespace Whetstone.ChatGPT.Blazor.App.Components
{
    public partial class AuthenticationStatus
    {

        [CascadingParameter]
        public ApplicationState AppState { get; set; }

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
