using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;
using Tweetinvi.Parameters;
using Whetstone.TweetGPT.WebHookManager.Models;
using Microsoft.Extensions.Options;
using Tweetinvi.Core.DTO.Webhooks;

namespace Whetstone.TweetGPT.WebHookManager
{
    public class WebhookManager : IWebhookManager
    {
        private TwitterClient _client;

        private WebhookCredentials _creds;


        public WebhookManager(IOptions<WebhookCredentials> creds) 
        {
            _creds = creds.Value;

            _client = GetClient();
        }


        public async Task<IWebhookDTO?> RegisterWebhookAsync(string? environment, Uri? webhookUri)
        {
            if (string.IsNullOrWhiteSpace(environment)) 
                throw new ArgumentNullException(nameof(environment));

            if(webhookUri is null)
                throw new ArgumentNullException(nameof(webhookUri));

            ICreateAccountActivityWebhookParameters accountParms = new CreateAccountActivityWebhookParameters(environment, webhookUri.ToString());

            IWebhook result = default;

            try
            {
                await EnsureBearerTokenAsync();
                await _client.AccountActivity.CreateAccountActivityWebhookAsync(accountParms).ConfigureAwait(false);
            }
            catch(TwitterException twitEx)
            {
                if (twitEx.StatusCode == 400)
                {
                    if (twitEx.Content.Contains("Non-200 response code during CRC GET request"))
                    {
                        throw new WebhookManagerException("Webhook endpoint returned a non-200 response to CRC GET request. Check webhook availability.", twitEx);
                    }
                    else if (twitEx.Content.Contains("Too many resources already created."))
                    {
                        throw new WebhookManagerException("Webhook already registered", twitEx);
                    }
                }
                if(twitEx.StatusCode == 401)
                {
                    throw new WebhookManagerException("Unauthorized. Check tokens.", twitEx);
                }

                if(twitEx.StatusCode == 403)
                {
                    throw new WebhookManagerException($"Forbidden request. Check if environment {environment} is valid.", twitEx);
                }

                throw new WebhookManagerException(twitEx.Message, twitEx);
            }


            return result?.WebhookDTO;
        }

        public async Task RemoveWebhookAsync(string? environment, string? webhookId)
        {


            await EnsureBearerTokenAsync();

            DeleteAccountActivityWebhookParameters delParams = new DeleteAccountActivityWebhookParameters(environment, webhookId);

            await _client.AccountActivity.DeleteAccountActivityWebhookAsync(delParams).ConfigureAwait(false);

        }

        public async Task<IEnumerable<IWebhook>> GetWebHooksAsync(string environment)
        {
            if (string.IsNullOrWhiteSpace(environment))
                throw new ArgumentNullException(nameof(environment));

            List<IWebhook>? webhooks = new();

            try
            {
                await EnsureBearerTokenAsync();

                IWebhook[] response = await _client.AccountActivity.GetAccountActivityEnvironmentWebhooksAsync(environment).ConfigureAwait(false);

                webhooks = response.ToList();
            }
            catch(TwitterException twitEx) 
            {
                throw new WebhookManagerException(twitEx.Message, twitEx);
            }

            return webhooks;
        }

        public async Task<IEnumerable<IWebhookEnvironment>> GetEnvironmentsAsync()
        {
            List<IWebhookEnvironment>? environments = new();

            try
            {
                await EnsureBearerTokenAsync();

                IWebhookEnvironment[] response = await _client.AccountActivity.GetAccountActivityWebhookEnvironmentsAsync().ConfigureAwait(false);
                environments = response.ToList();
            }
            catch (TwitterException twitEx)
            {
                throw new WebhookManagerException(twitEx.Message, twitEx);
            }

            return environments;
        }

        public Task ResendWebhookValidationAsync(string environment, string webhookId)
        {
            throw new NotImplementedException();
        }

        public async Task SubscribeAsync(string environment)
        {
            try
            {
                await EnsureBearerTokenAsync();

                SubscribeToAccountActivityParameters subscribeParameters = new SubscribeToAccountActivityParameters(environment);

                await _client.AccountActivity.SubscribeToAccountActivityAsync(subscribeParameters).ConfigureAwait(false);
            }
            catch (TwitterException twitEx)
            {
                throw new WebhookManagerException(twitEx.Message, twitEx);
            }
        }

        public async Task<IWebhookEnvironmentSubscriptionsDTO> GetSubscriptionsAsync(string environment)
        {
            IWebhookEnvironmentSubscriptionsDTO subDTO = default;
            try
            {
                await EnsureBearerTokenAsync();

                IGetAccountActivitySubscriptionsParameters subParams = new GetAccountActivitySubscriptionsParameters(environment);

                var response = await _client.AccountActivity.GetAccountActivitySubscriptionsAsync(environment).ConfigureAwait(false);

                subDTO = response.WebhookEnvironmentSubscriptionsDTO;
            }
            catch (TwitterException twitEx)
            {
                throw new WebhookManagerException(twitEx.Message, twitEx);
            }

            return subDTO;
        }

        public async Task UnsubscribeAsync(string environment, long userId)
        {
            try
            {
                await EnsureBearerTokenAsync();

                IUnsubscribeFromAccountActivityParameters unsubscribeParameters = new UnsubscribeFromAccountActivityParameters(environment, userId);

                await _client.AccountActivity.UnsubscribeFromAccountActivityAsync(unsubscribeParameters).ConfigureAwait(false);
            }
            catch (TwitterException twitEx)
            {
                throw new WebhookManagerException(twitEx.Message, twitEx);
            }
        }

        private async Task EnsureBearerTokenAsync()
        {                       
            if(string.IsNullOrWhiteSpace(_client.Credentials.BearerToken))
            {
                string bearerToken = await _client.Auth.CreateBearerTokenAsync();
                _client = GetClient(bearerToken);
            }            
        }

        private TwitterClient GetClient()
        {
            ConsumerOnlyCredentials consumerCreds = new(_creds.ConsumerKey, _creds.ConsumerSecret);
            TwitterClient client = new TwitterClient(consumerCreds);
            return client;
        }

        private TwitterClient GetClient(string bearerToken)
        {
            ConsumerOnlyCredentials consumerCreds = new(_creds.ConsumerKey, _creds.ConsumerSecret, bearerToken);
            ReadOnlyTwitterCredentials readonlyCreds = new(consumerCreds, _creds.AccessToken, _creds.AccessTokenSecret);
            TwitterClient client = new TwitterClient(readonlyCreds);
            return client;
        }
    }
}
