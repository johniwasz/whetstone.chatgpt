using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whetstone.TweetGPT.WebHookManager
{
    public class WebhookManagerException : Exception
    {

        public WebhookManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public WebhookManagerException(string message) : base(message)
        {
        }
    }
}
