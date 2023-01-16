
using Blazorise.Bootstrap5;
using Whetstone.ChatGPT.Blazor.App.State;

namespace Whetstone.ChatGPT.Blazor.App.Components
{
    public partial class AuthenticationStatus
    {
        private LogIn? loginModal;

        private void ShowLogin()
        {
           if (loginModal is not null)
           {
                loginModal?.Show();
           }
        }
        
        public void PurgeCredentials()
        {
            ChatClient.Credentials = null;
            CurrentState.IsOpenAIAuthenticated = false;
        }
    }
}
