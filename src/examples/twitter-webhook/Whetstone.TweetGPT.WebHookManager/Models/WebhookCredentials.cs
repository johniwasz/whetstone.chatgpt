using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;

namespace Whetstone.TweetGPT.WebHookManager.Models
{
    public class WebhookCredentials
    {
        public WebhookCredentials()
        {

        }

        public WebhookCredentials(string? accessToken, string? accessTokenSecret, string? consumerKey, string? consumerSecret)
        {
            this.AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));

            this.AccessTokenSecret = accessTokenSecret ?? throw new ArgumentNullException(nameof(accessTokenSecret));

            this.ConsumerSecret= consumerSecret ?? throw new ArgumentNullException(nameof(consumerKey));

            this.ConsumerKey = consumerKey ?? throw new ArgumentNullException(nameof(consumerSecret));
        }

        public string? AccessToken
        {
            get; set;
        }

        public string? AccessTokenSecret
        {
            get; set;
        }

        public string? ConsumerKey
        {
            get; set;
        }

        public string? ConsumerSecret
        {
            get; set;
        }
    }
}
