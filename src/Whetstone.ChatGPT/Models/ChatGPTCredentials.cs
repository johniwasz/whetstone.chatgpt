using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
{
    public class ChatGPTCredentials
    {

        public ChatGPTCredentials()
        {
            
        }

        public ChatGPTCredentials(string apiKey)
        {
            ApiKey = apiKey;
        }

        public ChatGPTCredentials(string apiKey, string? organization)
        {
            ApiKey = apiKey;
            Organization = organization;
        }

        public string? ApiKey
        {
            get;
            set;
        }

        public string? Organization
        {
            get;
            set;
        }

    }
}
