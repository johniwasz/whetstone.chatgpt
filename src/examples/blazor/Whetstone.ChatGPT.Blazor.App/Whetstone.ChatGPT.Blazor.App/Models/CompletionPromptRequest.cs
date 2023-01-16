using System.ComponentModel.DataAnnotations;

namespace Whetstone.ChatGPT.Blazor.App.Models
{
    public class CompletionPromptRequest
    {
        [Required]
        public string Prompt { get; set; } = default!;
    }
}
