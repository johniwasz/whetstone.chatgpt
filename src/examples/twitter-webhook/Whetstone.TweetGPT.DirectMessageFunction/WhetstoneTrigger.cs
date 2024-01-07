// SPDX-License-Identifier: MIT
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
using Whetstone.ChatGPT;
using Whetstone.ChatGPT.Models;
using Tweetinvi.Exceptions;
using System.Text;
using Tweetinvi.Models.V2;
using System.Threading;
using Tweetinvi.Core.Events;

namespace Whetstone.TweetGPT.DirectMessageFunction
{


    public class ChatCPTDirectMessageFunction
    {
        private readonly List<long> _trackedStreams = new List<long>();

        private readonly ILogger _logger;
        private readonly WebhookCredentials _creds;
        private readonly TwitterClient _client;
        private readonly IChatGPTClient _chatClient;
        private readonly string _basePrompt;

        public ChatCPTDirectMessageFunction(IChatGPTClient chatGPT, IOptions<WebhookCredentials> creds, ILogger<ChatCPTDirectMessageFunction> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _creds = creds?.Value ?? throw new ArgumentNullException(nameof(creds));

            _chatClient = chatGPT ?? throw new ArgumentNullException(nameof(chatGPT));

            _client = GetClient();

            StringBuilder promptBuilder = new();
            promptBuilder.AppendLine("Monty is an arrogant, rich, CEO that reluctantly answers questions with condescending responses:");
            promptBuilder.AppendLine();
            promptBuilder.AppendLine("You: How many pounds are in a kilogram?");
            promptBuilder.AppendLine("Monty: This again? There are 2.2 pounds in a kilogram, you ninny.");
            promptBuilder.AppendLine("You: What does HTML stand for?");
            promptBuilder.AppendLine("Monty: I'm too busy for this? Hypertext Markup Language. The T is for try to get a job.");
            promptBuilder.AppendLine("You: When did the first airplane fly?");
            promptBuilder.AppendLine("Monty: On December 17, 1903, Wilbur and Orville Wright made the first flights. I funded their construction.");
            promptBuilder.AppendLine("You: What is the meaning of life?");
            promptBuilder.AppendLine("Monty: To get rich. Family, religion, friendship. These are the three demons you must slay if you wish to succeed in busniess.");
            _basePrompt = promptBuilder.ToString();

        }

        [Function("chatgptdm")]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            WebhooksRequestHandlerForAzureFunction request = new WebhooksRequestHandlerForAzureFunction(req);

            IAccountActivityRequestHandler activityHandler = _client.AccountActivity.CreateRequestHandler();

            long? userId = await GetUserIdAsync(request).ConfigureAwait(false);

            if (userId.HasValue)
            {
                _logger.LogInformation($"Processing message for user {userId.Value}");

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
                _logger.LogInformation("Request is managed by Tweetinvi.");
                var routeHandled = await activityHandler.TryRouteRequestAsync(request).ConfigureAwait(false);
                if (routeHandled)
                {
                    return request.GetHttpResponseMessage();
                }
            }

            var httpResp = req.CreateResponse(HttpStatusCode.OK);
            return httpResp;
        }

        private void MessageReceived(object? sender, MessageReceivedEvent? e)
        {
            if (e is not null)
            {

                string messageFromSender = e.Message.Text;

                long senderId = e.Message.SenderId;

                _logger.LogInformation($"Received message from {senderId}.");
                _logger.LogInformation($"Received message text: {messageFromSender}");

                string userInput = $"You: {messageFromSender}\nMonty: ";

                string userPrompt = string.Concat(_basePrompt, userInput);

                _logger.LogInformation($"User prompt: {userPrompt}");

                ChatGPTCompletionRequest completionRequest = new()
                {
                    Temperature = 1.0f,
                    Model = ChatGPT35Models.Gpt35TurboInstruct,
                    Prompt = userPrompt,
                    MaxTokens = 200,
                    User = senderId.ToString()
                };

                try
                {
                    ChatGPTCompletionResponse? gptResponse = _chatClient.CreateCompletionAsync(completionRequest).GetAwaiter().GetResult();

                    if (gptResponse?.Choices is not null)
                    {
                        string? responseMessage = gptResponse.GetCompletionText();

                        if (!string.IsNullOrWhiteSpace(responseMessage))
                        {
                            SendResponseAsync(e, responseMessage).Wait();
                        }
                    }
                }
                catch (ChatGPTException chatEx)
                {
                    _logger.LogError(chatEx, $"ChatGPT had an error processing prompt: {messageFromSender}");

                    _logger.LogError(chatEx, $"  ChatGPT Status: {chatEx.StatusCode}");

                    if (chatEx.ChatGPTError is not null)
                    {
                        _logger.LogError(chatEx, $"  ChatGPT error details: {chatEx.ChatGPTError.Message}, Code: {chatEx.ChatGPTError.Code}, Type: {chatEx.ChatGPTError.Type}");
                    }

                    SendResponseAsync(e, "I'm attending to important business. Await your turn, plebian.").Wait();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred processing prompt: {messageFromSender}");
                }
            }
        }

        private async Task SendResponseAsync(MessageReceivedEvent? e, string responseMessage)
        {
            if (e is not null)
            {
                try
                {
                    _logger.LogInformation($"Sending Response: {responseMessage}");

                    IPublishMessageParameters publishParameters = new PublishMessageParameters(responseMessage, e.Sender.Id);

                    TwitterClient client = await EnsureBearerTokenAsync(_client).ConfigureAwait(false);

                    await client.Messages.PublishMessageAsync(publishParameters).ConfigureAwait(false);
                }
                catch (TwitterAuthException authEx)
                {
                    _logger.LogError(authEx, $"Error authenticating with Twitter credentials while sending response: {responseMessage}");
                }
                catch (TwitterResponseException respEx)
                {
                    _logger.LogError(respEx, $"Error sending Twitter message while sending response: {responseMessage}");
                }
            }
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
