using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Blazor.App.State
{
    public class ApplicationState
    {
        public int TokensUsed { get; set; }

        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }

        private bool isOpenAIAuthenticated;

        public void UpdateTokenUsage(ChatGPTUsage usage)
        {
            TokensUsed += usage.TotalTokens;
            PromptTokens += usage.PromptTokens;
            CompletionTokens += usage.CompletionTokens;
            NotifyStateChanged();
        }

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
