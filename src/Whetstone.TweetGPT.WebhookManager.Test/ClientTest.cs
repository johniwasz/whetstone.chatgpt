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

    public class ClientTest
    {

        [Fact]
        public async Task ClientConnect()
        {

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())               
                .AddUserSecrets("2d062944-d0f9-4fab-bed7-d43b4637c783")
                .AddEnvironmentVariables()
                .Build();

            string? consumerKey = config["WebhookCredentials:ConsumerKey"];
            string? consumerSecret = config["WebhookCredentials:ConsumerSecret"];
            string? accessToken = config["WebhookCredentials:AccessToken"];
            string? accessTokenSecret = config["WebhookCredentials:AccessTokenSecret"];


            var consumerOnlyCredentials = new ConsumerOnlyCredentials(consumerKey, consumerSecret);
            ReadOnlyTwitterCredentials twitCreds = new ReadOnlyTwitterCredentials(consumerOnlyCredentials, accessToken, accessTokenSecret);

            var appClientWithoutBearer = new TwitterClient(twitCreds);

            await appClientWithoutBearer.Auth.InitializeClientBearerTokenAsync();

            var bearerToken = await appClientWithoutBearer.Auth.CreateBearerTokenAsync();

            Assert.True(!string.IsNullOrWhiteSpace(bearerToken));

        }
    }
}