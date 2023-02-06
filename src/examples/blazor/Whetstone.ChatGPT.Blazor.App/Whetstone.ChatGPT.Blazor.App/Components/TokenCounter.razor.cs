
using Microsoft.AspNetCore.Components;
using System.Text;
using Whetstone.ChatGPT.Blazor.App.State;

namespace Whetstone.ChatGPT.Blazor.App.Components
{
    public partial class TokenCounter
    {
        [CascadingParameter]
        public ApplicationState AppState { get; set; } = default!;

        public string? TokenBreakdownText { 
            get
            {
                string breakDown = $"Prompt Tokens: {AppState.PromptTokens}<br/>Completion Tokens: {AppState.CompletionTokens}";
                return breakDown;
            }
        } 


        protected override void OnInitialized()
        {
            AppState.OnChange += StateHasChanged;
            base.OnInitialized();
        }


        ~TokenCounter()
        {
            AppState.OnChange -= StateHasChanged;
        }

    }
}
