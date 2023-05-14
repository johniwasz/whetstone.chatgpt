// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
{
    /// <summary>
    /// Represents contents of a file for use with the GPT-3 file endpoint.
    /// </summary>
    /// <remarks>
    /// Both FileName and Content are required.
    /// </remarks>
    public class ChatGPTFileContent
    {
        public ChatGPTFileContent()
        {
        }

        public ChatGPTFileContent(byte[] content, string fileName)
        {
            if (content == null || content.Length == 0)
            {
                throw new ArgumentException($"{nameof(content)} is required");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException($"{nameof(fileName)} is required");
            }

            Content = content;
            FileName = fileName;
        }

        /// <summary>
        /// Binary contents of the file
        /// </summary>
        public byte[]? Content { get; set; }

        public string? FileName { get; set; }

        public static ChatGPTFileContent Load(string file)
        {
            var bytes = File.ReadAllBytes(file);

            var name = Path.GetFileName(file);
            
            return new ChatGPTFileContent(bytes, name);
        }
        
    }
}
