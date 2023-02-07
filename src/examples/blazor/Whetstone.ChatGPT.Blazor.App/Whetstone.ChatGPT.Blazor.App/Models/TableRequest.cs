using System.ComponentModel.DataAnnotations;

namespace Whetstone.ChatGPT.Blazor.App.Models
{
    public class TableRequest
    {
        [Required]
        public string Category { get; set; } = string.Empty;

        [Range(5, 30)]
        public int MaxItems { get; set; } = 10;
    }
}
