using System;
using System.Diagnostics;
using Whetstone.ChatGPT.Blazor.App.Components;

namespace Whetstone.ChatGPT.Blazor.App.Pages.Prompts
{
    public partial class TestDataGenerator
    {
        private ChatOptionsSelector? optionsSelector = default!;

        private Exception? exception = default;
        
        private void ProcessOptionsException(Exception ex)
        {
            exception = ex;
        }
    }
}
