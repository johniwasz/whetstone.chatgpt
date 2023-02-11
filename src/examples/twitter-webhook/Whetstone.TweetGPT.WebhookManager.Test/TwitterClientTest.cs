using Microsoft.Extensions.Configuration;
using Tweetinvi;
using Tweetinvi.Models;
using Whetstone.TweetGPT.WebHookManager.Models;
using System.Text.Json;
using xRetry;

namespace Whetstone.TweetGPT.WebhookManager.Test
{

    public class TwitterClientTest
    {

        [RetryFact]
        public async Task TwitterClientConnect()
        {

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets("2d062944-d0f9-4fab-bed7-d43b4637c783")
                .AddEnvironmentVariables()
                .Build();

            string? twitterCredString = config["TWITTER_CREDS"];

            WebhookCredentials? webhookCreds = string.IsNullOrWhiteSpace(twitterCredString) ?
                null : JsonSerializer.Deserialize<WebhookCredentials>(twitterCredString);


            ConsumerOnlyCredentials consumerCreds = webhookCreds is not null ?
                new ConsumerOnlyCredentials(webhookCreds.ConsumerKey, webhookCreds.ConsumerSecret) :
                new ConsumerOnlyCredentials(config["WebhookCredentials:ConsumerKey"], config["WebhookCredentials:ConsumerSecret"]);


            ReadOnlyTwitterCredentials twitCreds = webhookCreds is not null ?
                new ReadOnlyTwitterCredentials(consumerCreds, webhookCreds.AccessToken, webhookCreds.AccessTokenSecret) :
                new ReadOnlyTwitterCredentials(consumerCreds, config["WebhookCredentials:AccessToken"], config["WebhookCredentials:AccessTokenSecret"]);

            var appClientWithoutBearer = new TwitterClient(twitCreds);

            await appClientWithoutBearer.Auth.InitializeClientBearerTokenAsync();

            var bearerToken = await appClientWithoutBearer.Auth.CreateBearerTokenAsync();

            Assert.True(!string.IsNullOrWhiteSpace(bearerToken));

        }
    }
}