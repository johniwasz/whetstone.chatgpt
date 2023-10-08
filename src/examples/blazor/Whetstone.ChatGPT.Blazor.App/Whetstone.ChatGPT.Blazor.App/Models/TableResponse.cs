using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Blazor.App.Models
{
    public class TableResponse
    {

        public string? Content { get; set; }
        
        public ChatGPTUsage Usage { get; set; }
    }
}
