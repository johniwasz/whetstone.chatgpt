namespace Whetstone.ChatGPT.Blazor.App.State
{
    public class ApplicationState
    {
        private bool isOpenAIAuthenticated { get; set; }

        private string? savedString;

        public bool IsOpenAIAuthenticated
        {
            get => isOpenAIAuthenticated;
            set
            {
                isOpenAIAuthenticated = value;
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
