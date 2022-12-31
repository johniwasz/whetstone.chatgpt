using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using Tweetinvi;
using Tweetinvi.Client;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.V2;
using Whetstone.TweetGPT.WebHookManager;
using Whetstone.TweetGPT.WebHookManager.Models;

namespace Whetstone.TweetGPT.WebhookManager.Test
{

    public class TwitterClientTest
    {

        [Fact]
        public async Task TwitterClientConnect()
        {

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets("2d062944-d0f9-4fab-bed7-d43b4637c783")
                .AddEnvironmentVariables()
                .Build();

            string? consumerKey = config["WebhookCredentials:ConsumerKey"] is not null 
                ? config["WebhookCredentials:ConsumerKey"]
                : config[EnvironmentSettings.ENV_TWITTER_CONSUMER_KEY];

            string? consumerSecret = config["WebhookCredentials:ConsumerSecret"] is not null
                ? config["WebhookCredentials:ConsumerSecret"]
                : config[EnvironmentSettings.ENV_TWITTER_CONSUMER_SECRET];

            string? accessToken = config["WebhookCredentials:AccessToken"] is not null
                ? config["WebhookCredentials:AccessToken"]
                : config[EnvironmentSettings.ENV_TWITTER_ACCESS_TOKEN];

            string? accessTokenSecret = config["WebhookCredentials:AccessTokenSecret"] is not null
                ? config["WebhookCredentials:AccessTokenSecret"]
                : config[EnvironmentSettings.ENV_TWITTER_ACCESS_TOKEN_SECRET];


            var consumerOnlyCredentials = new ConsumerOnlyCredentials(consumerKey, consumerSecret);
            ReadOnlyTwitterCredentials twitCreds = new ReadOnlyTwitterCredentials(consumerOnlyCredentials, accessToken, accessTokenSecret);

            var appClientWithoutBearer = new TwitterClient(twitCreds);

            await appClientWithoutBearer.Auth.InitializeClientBearerTokenAsync();

            var bearerToken = await appClientWithoutBearer.Auth.CreateBearerTokenAsync();

            Assert.True(!string.IsNullOrWhiteSpace(bearerToken));

        }
    }
}