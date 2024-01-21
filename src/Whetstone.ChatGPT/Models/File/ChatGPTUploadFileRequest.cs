// SPDX-License-Identifier: MIT

namespace Whetstone.ChatGPT.Models.File
{
    public class ChatGPTUploadFileRequest
    {
        public ChatGPTFileContent? File { get; set; }

        public string Purpose { get; set; } = "fine-tune";
    }
}
