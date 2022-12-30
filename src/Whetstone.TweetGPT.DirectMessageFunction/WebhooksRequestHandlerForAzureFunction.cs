using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using System.Web;
using Microsoft.Azure.Functions.Worker.Http;
using Azure;
using System.Diagnostics.Eventing.Reader;

namespace Whetstone.TweetGPT.DirectMessageFunction
{
    public class WebhooksRequestHandlerForAzureFunction : IWebhooksRequest
    {
        private readonly HttpRequestData _request;
        private readonly HttpResponseData _response;
        private string _requestBody;
        private bool _isBodyRead = false;

        public WebhooksRequestHandlerForAzureFunction(HttpRequestData request)
        {
            _request = request;
            _response = request.CreateResponse();
        }

        public async Task<string> GetJsonFromBodyAsync()
        {
            if (!_isBodyRead)
            {
                _requestBody = await new StreamReader(_request.Body).ReadToEndAsync().ConfigureAwait(false);
                _isBodyRead = true;
            }

            return _requestBody;
        }

        public string GetPath()
        {
            return _request.Url.AbsolutePath;
        }

        public IDictionary<string, string[]> GetHeaders()
        {
            return _request.Headers.ToDictionary(x => x.Key.ToLowerInvariant(), x => x.Value.ToArray());
        }

        public IDictionary<string, string[]> GetQuery()
        {
            var queryNameValuePairs = HttpUtility.ParseQueryString(_request.Url.Query);
            var query = new Dictionary<string, string[]>();

            queryNameValuePairs.AllKeys.ForEach(key => query.Add(key, new[] { queryNameValuePairs[key] }));

            return query;
        }

        public void SetResponseStatusCode(int statusCode)
        {
            _response.StatusCode = (HttpStatusCode)statusCode;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task WriteInResponseAsync(string content, string contentType)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            await _response.WriteStringAsync(content);
            _response.Headers.Add("Content-Type", "application/json");
        }

        public HttpResponseData GetHttpResponseMessage()
        {
            return _response;
        }
    }
}

