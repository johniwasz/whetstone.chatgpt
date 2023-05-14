// SPDX-License-Identifier: MIT
namespace Whetstone.ChatGPT.Blazor.App.Models
{
    public class CompletionOptions
    {
        public int MaxTokens { get; set; } = 200;    
        public float Temperature { get; set; } = 0.1f;
        public string? SelectedModel { get; set; } = null;
    }
}
