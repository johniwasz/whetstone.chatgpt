using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT
{
    public class ChatGPTConfigurationException : Exception
    {
        internal ChatGPTConfigurationException(string message) : base(message)
        {
        }
    }
}