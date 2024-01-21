// SPDX-License-Identifier: MIT

using System.ComponentModel.DataAnnotations;

namespace Whetstone.ChatGPT.Models.File
{
    public class ChatGPTUploadFileRequest
    {
        public ChatGPTFileContent? File { get; set; }

        /// <summary>
        /// The intended purpose of the uploaded file.        
        /// </summary>
        /// <remarks>Use "fine-tune" for Fine-tuning and "assistants" for Assistants and Messages.This allows us to validate the format of the uploaded file is correct for fine-tuning.</remarks>
        public string Purpose { get; set; } = "fine-tune";
    }
}
