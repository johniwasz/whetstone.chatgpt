using System.Diagnostics;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Whetstone.TweetGPT.WebHookManager;
using Whetstone.TweetGPT.WebHookManager.Models;
using Microsoft.Extensions.Options;
using Tweetinvi.Models;
using Tweetinvi;
using System.Diagnostics.Eventing.Reader;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Streams;
using Tweetinvi.AspNet;
using Tweetinvi.AspNet.Core.Logic;
using Tweetinvi.Streaming;
using Tweetinvi.Events;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Logic.Wrapper;
using Azure.Core;
using Tweetinvi.Parameters;

namespace Whetstone.TweetGPT.DirectMessageFunction
{


    public class ChatCPTDirectMessageFunction
    {
        private readonly List<long> _trackedStreams = new List<long>();

        private readonly ILogger _logger;
        private readonly WebhookCredentials _creds;        
        private readonly TwitterClient _client;

        public ChatCPTDirectMessageFunction(IOptions<WebhookCredentials> creds, ILogger<ChatCPTDirectMessageFunction> logger)
        {            
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _creds = creds?.Value ?? throw new ArgumentNullException(nameof(creds));

            _client = GetClient();            
        }

        [Function("chatgptdm")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
             FunctionContext executionContext)
        {

            WebhooksRequestHandlerForAzureFunction request = new WebhooksRequestHandlerForAzureFunction(req);
            
            IAccountActivityRequestHandler activityHandler = _client.AccountActivity.CreateRequestHandler();

            long? userId = await GetUserIdAsync(request).ConfigureAwait(false);

            if (userId.HasValue)
            {
                IAccountActivityStream activityStream = activityHandler.GetAccountActivityStream(userId.Value, "devchatgpt");

                if (!_trackedStreams.Contains(activityStream.AccountUserId))
                {
                    _trackedStreams.Add(activityStream.AccountUserId);
                    activityStream.MessageReceived += MessageReceived;
                }
            }


            var isRequestManagedByTweetinvi = await activityHandler.IsRequestManagedByTweetinviAsync(request).ConfigureAwait(false);

            if (isRequestManagedByTweetinvi)
            {
                var routeHandled = await activityHandler.TryRouteRequestAsync(request).ConfigureAwait(false);
                if (routeHandled)
                {
                    return request.GetHttpResponseMessage();
                }
            }

            /*
             string? crcToken;
            string? nonce;

            if (req.FunctionContext.BindingContext.BindingData.ContainsKey("crc_token"))
            {
                crcToken = (string?) req.FunctionContext.BindingContext.BindingData["crc_token"];
                nonce = (string?) req.FunctionContext.BindingContext.BindingData["nonce"];                

                var crcResponse = _webhookVerifier.GenerateCrcResponse(crcToken, _creds.ConsumerSecret);
                var httpCrcResp = req.CreateResponse(HttpStatusCode.OK);

                await httpCrcResp.WriteAsJsonAsync(crcResponse);

                return httpCrcResp;
            }

          
            var accountActivityHandler = _client.AccountActivity.CreateRequestHandler();

            IWebhooksRequest webhookRequest = new WebhooksRequestHandlerForAzureFunction(req);

            bool isRouted = await accountActivityHandler.TryRouteRequestAsync(webhookRequest);

            //accountActivityHandler.Web

         

            Stopwatch funcTime = Stopwatch.StartNew();


            this._logger.LogInformation($"Alexa request processing time: {funcTime.ElapsedMilliseconds} milliseconds");
            */
            var httpResp = req.CreateResponse(HttpStatusCode.OK);
            return httpResp;
        }

        private void MessageReceived(object sender, MessageReceivedEvent e)
        {

            IPublishMessageParameters publishParameters = new PublishMessageParameters("Hello back from the webs", e.Sender.Id);

            TwitterClient client = EnsureBearerTokenAsync(_client).Result;

            client.Messages.PublishMessageAsync(publishParameters).Wait();
        }

        private async Task<TwitterClient> EnsureBearerTokenAsync(TwitterClient twitClient)
        {
            TwitterClient retClient = _client;
            if (string.IsNullOrWhiteSpace(twitClient.Credentials.BearerToken))
            {
                string bearerToken = await twitClient.Auth.CreateBearerTokenAsync();
                retClient = GetClient(bearerToken);
            }

            return retClient;
        }

        private async Task<long?> GetUserIdAsync(IWebhooksRequest request)
        {
            IJObjectStaticWrapper wrapper = TweetinviContainer.Resolve<IJObjectStaticWrapper>();
            long? userId = null;

            var jsonObjectEvent = wrapper.GetJobjectFromJson(await request.GetJsonFromBodyAsync().ConfigureAwait(false));

            if (jsonObjectEvent is not null)
            {
                bool? isUserRequest = jsonObjectEvent.ContainsKey("for_user_id");

                if (isUserRequest.GetValueOrDefault(false))
                {
                    userId = (long?)jsonObjectEvent["for_user_id"];
                }
            }
            return userId;
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
