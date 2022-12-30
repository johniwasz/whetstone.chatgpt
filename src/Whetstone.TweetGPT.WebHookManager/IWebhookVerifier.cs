using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whetstone.TweetGPT.WebHookManager.Models;

namespace Whetstone.TweetGPT.WebHookManager
{
    public interface IWebhookVerifier
    {
        WebhookCrcResponse GenerateCrcResponse(string? crcToken, string? consumerSecret);
    }
}
