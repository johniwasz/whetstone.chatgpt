using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
{
    public class ChatGPTUploadFileRequest
    {

        public ChatGPTFileContent? File { get; set; }
        
        public string Purpose { get; set; } = "fine-tune";

    }
}
