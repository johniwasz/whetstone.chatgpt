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
using System.Diagnostics.CodeAnalysis;

namespace Whetstone.TweetGPT.DirectMessageFunction
{


    /// <summary>
    /// This class is a wrapper around the Azure Function HttpRequestData class which is used by Tweetinvi internals
    /// to process the WebHook request.
    /// </summary>
    public class WebhooksRequestHandlerForAzureFunction : IWebhooksRequest
    {
        private readonly HttpRequestData _request;
        private readonly HttpResponseData _response;
        private bool _isBodyRead = false;
        private string? _requestBody;

        public WebhooksRequestHandlerForAzureFunction(HttpRequestData? request)
        {
            _request = request ?? throw new ArgumentNullException(nameof(request));
            _response = request.CreateResponse();
        }

        public async Task<string?> GetJsonFromBodyAsync()
        {
            
            if (!_isBodyRead)
            {
                using (StreamReader reader = new(_request.Body))
                {
                    string requestBody = await reader.ReadToEndAsync().ConfigureAwait(false);
                    if (requestBody is null)
                        throw new NullReferenceException("Request body is null.");

                    _isBodyRead = true;

                    _requestBody = requestBody;
                }
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

            if (queryNameValuePairs is not null)
            {
                queryNameValuePairs.AllKeys.ForEach(key =>
                {
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        var queryNameValues = queryNameValuePairs.GetValues(key);

                        if (queryNameValues is not null)
                        {
                            query.Add(key, queryNameValues);
                        }
                    }
                });
            }
            
            return query;
        }

        public void SetResponseStatusCode(int statusCode)
        {
            _response.StatusCode = (HttpStatusCode)statusCode;
        }

        public async Task WriteInResponseAsync(string content, string contentType)
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

