using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Whetstone.TweetGPT.WebHookManager.Models;

namespace Whetstone.TweetGPT.WebHookManager
{
    public class WebhookVerifier : IWebhookVerifier
    {

        /// <summary>
        /// This is used to create the response that verifies that the Twitter webhook is owned by the same organization that 
        /// registered the web hook.
        /// </summary>
        /// <param name="crcToken"></param>
        /// <param name="consumerSecret"></param>
        /// <returns></returns>
        public WebhookCrcResponse GenerateCrcResponse(string? crcToken, string? consumerSecret)
        {

            WebhookCrcResponse? resp = null;
            var encoding = new ASCIIEncoding();

            byte[] keyBytes = encoding.GetBytes(consumerSecret);

            byte[] messageBytes = encoding.GetBytes(crcToken);

            string? crcResponseToken = null;

            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                crcResponseToken = Convert.ToBase64String(hashmessage);
            }

            resp = new WebhookCrcResponse
            {
                ResponseToken = $"sha256={crcResponseToken}"
            };

            return resp;
        }

    }
}
