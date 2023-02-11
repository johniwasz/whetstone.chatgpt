

namespace Whetstone.ChatGPT.Models
{
    public class ChatGPTUploadFileRequest
    {
        public ChatGPTFileContent? File { get; set; }

        public string Purpose { get; set; } = "fine-tune";
    }
}
