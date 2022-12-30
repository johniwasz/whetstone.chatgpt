using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT;

public class ChatGPTClient : IDisposable
{
    private string _apiKey;

    private HttpClient _client;

    private bool _isHttpClientProvided = true;

    private bool _isDisposed = false;

    public ChatGPTClient(string apiKey) : this(new ChatGPTCredentials(apiKey), null)
    {
    }

    public ChatGPTClient(string apiKey, string organization) : this(new ChatGPTCredentials(apiKey, organization), null)
    {
    }

    public ChatGPTClient(string apiKey, HttpClient client) : this(new ChatGPTCredentials(apiKey), client)
    {
        if (client is null)
        {
            throw new ArgumentNullException(nameof(client));
        }
    }

    public ChatGPTClient(string apiKey, string organization, HttpClient client) : this(new ChatGPTCredentials(apiKey, organization), client)
    {
    }

    public ChatGPTClient(ChatGPTCredentials creds, HttpClient? client)
    {
        if (string.IsNullOrWhiteSpace(creds.ApiKey))
        {
            throw new ArgumentException("Cannot be null or whitespace.", nameof(creds.ApiKey));
        }
        _apiKey = creds.ApiKey;

        if (client is null)
        {
            _client = new HttpClient();
            _isHttpClientProvided = false;
        }
        else
        {
            _client = client;
            _isHttpClientProvided = true;
        }

        InitializeClient(_client, creds);
    }


    private void InitializeClient(HttpClient client, ChatGPTCredentials creds)
    {

        client.BaseAddress = new Uri("https://api.openai.com/v1/");

        if (!string.IsNullOrWhiteSpace(_client.DefaultRequestHeaders.Authorization?.Parameter))
        {
            throw new ArgumentException("HttpClient already has authorization token.", nameof(client));
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", creds.ApiKey);

        if(!string.IsNullOrWhiteSpace(creds.Organization))
        {
            client.DefaultRequestHeaders.Add("OpenAI-Organization", creds.Organization);
        }
    }

    public async Task<ChatGPTCompletionResponse> GetResponseAsync(ChatGPTCompletionRequest completionRequest, CancellationToken? cancelToken = null)
    {
        if (completionRequest is null)
        {
            throw new ArgumentNullException(nameof(completionRequest));
        }

        return await SendRequestAsync<ChatGPTCompletionRequest, ChatGPTCompletionResponse>(HttpMethod.Post, "completions", completionRequest, cancelToken);
    }

    public async Task<ChatGPTModelsResponse> GetModelsAsync(CancellationToken? cancelToken = null)
    {       
        return await SendRequestAsync<ChatGPTModelsResponse>(HttpMethod.Get, "models", cancelToken);
    }

    public async Task<ChatGPTModel?> GetModelAsync(string modelId, CancellationToken? cancelToken = null)
    {

        if (string.IsNullOrWhiteSpace(modelId))
        {
            throw new ArgumentException("Cannot be null or whitespace.", nameof(modelId));
        }
        
        return await SendRequestAsync<ChatGPTModel>(HttpMethod.Get, $"models/{modelId}", cancelToken);
    }

    #region Generic Request and Response Processors
    private async Task<TR> SendRequestAsync<T, TR>(HttpMethod method, string url, T requestMessage, CancellationToken? cancellationToken) 
        where T : class 
        where TR : class
    {
        using (var httpReq = new HttpRequestMessage(HttpMethod.Post, url))
        {

            httpReq.Headers.Add("Authorization", $"Bearer {_apiKey}");

            string requestString = JsonSerializer.Serialize(requestMessage);

            httpReq.Content = new StringContent(requestString, Encoding.UTF8, "application/json");

            using (HttpResponseMessage? httpResponse = cancellationToken is null ?
                await  _client.SendAsync(httpReq) :
                 await _client.SendAsync(httpReq, cancellationToken.Value))
            {
                return await ProcessResponseAsync<TR>(httpResponse, cancellationToken);
            }
        }
        throw new ChatGPTException("Unxpected code path");
    }

    
    private async Task<T> SendRequestAsync<T>(HttpMethod method, string url, CancellationToken? cancellationToken)  where T : class
    {
        var request = new HttpRequestMessage(method, url);

        request.Headers.Add("Authorization", $"Bearer {_apiKey}");

        using (HttpResponseMessage? httpResponse = cancellationToken is null ?
             await _client.SendAsync(request) :
             await _client.SendAsync(request, cancellationToken.Value))
        {
            return await ProcessResponseAsync<T>(httpResponse, cancellationToken);

        }
        
        throw new ChatGPTException("Unxpected code path");
    }

    private async Task<T> ProcessResponseAsync<T>(HttpResponseMessage responseMessage, CancellationToken? cancellationToken) where T : class
    {
        string responseString = cancellationToken is null ?
                await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false) :
                await responseMessage.Content.ReadAsStringAsync(cancellationToken.Value).ConfigureAwait(false);

        if (responseMessage.IsSuccessStatusCode)
        {
            if (!string.IsNullOrWhiteSpace(responseString))
            {
                T? response = JsonSerializer.Deserialize<T>(responseString);

                if (response is null)
                {
                    throw new ChatGPTException("Unable to deserialize response.");
                }
                else
                {
                    return response;
                }
            }
        }
        else
        {
            ChatGPTErrorResponse? errResponse = JsonSerializer.Deserialize<ChatGPTErrorResponse>(responseString);
            throw new ChatGPTException(errResponse?.Error);
        }

        throw new ChatGPTException("Unxpected code path");
    }
    #endregion

    #region Clean Up    
    ~ChatGPTClient()
    {     
        Dispose(true);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (!_isHttpClientProvided)
            {
                _client.Dispose();
            }
            _isDisposed = true;
        }
    }
    #endregion
}
