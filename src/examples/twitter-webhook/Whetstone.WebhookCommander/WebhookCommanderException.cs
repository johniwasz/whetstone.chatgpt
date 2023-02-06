using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whetstone.WebhookCommander
{
    internal class WebhookCommanderException : Exception
    {

        public WebhookCommanderException(string message, Exception innerException) : base(message, innerException)
        { 
        }

        public WebhookCommanderException(string message) : base(message)
        {
        }
    }
}
