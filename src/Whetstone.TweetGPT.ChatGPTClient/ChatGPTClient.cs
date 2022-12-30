using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using Whetstone.TweetGPT.ChatGPTClient.Models;

namespace Whetstone.TweetGPT.ChatGPTClient;

public class ChatClient
{
    private string _sessionToken;

    public ChatClient([NotNull] string sessionToken)
    {
        if (string.IsNullOrWhiteSpace(sessionToken))
        {
            throw new ArgumentException("Cannot be null or whitespace.", nameof(sessionToken));
        }
        _sessionToken = sessionToken;
    }

    public async Task<ChatGPTCompletionResponse> GetResponseAsync(ChatGPTCompletionRequest completionRequest, CancellationToken? cancelToken = null)
    {
        if (completionRequest is null)
        {
            throw new ArgumentNullException(nameof(completionRequest));
        }

        return await SendRequestAsync<ChatGPTCompletionRequest, ChatGPTCompletionResponse>(HttpMethod.Post, "https://api.openai.com/v1/completions", completionRequest, cancelToken);
    }

    public async Task<ChatGPTModelsResponse> GetModelsAsync(CancellationToken? cancelToken = null)
    {       
        return await SendRequestAsync<ChatGPTModelsResponse>(HttpMethod.Get, "https://api.openai.com/v1/models", cancelToken);
    }

    public async Task<ChatGPTModel?> GetModelAsync(string modelId, CancellationToken? cancelToken = null)
    {

        if (string.IsNullOrWhiteSpace(modelId))
        {
            throw new ArgumentException("Cannot be null or whitespace.", nameof(modelId));
        }
        
        return await SendRequestAsync<ChatGPTModel>(HttpMethod.Get, $"https://api.openai.com/v1/models/{modelId}", cancelToken);
    }

    private async Task<TR> SendRequestAsync<T, TR>(HttpMethod method, string url, T requestMessage, CancellationToken? cancellationToken) 
        where T : class 
        where TR : class
    {

        var client = new HttpClient();
        
        using (var httpReq = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/completions"))
        {

            httpReq.Headers.Add("Authorization", $"Bearer {_sessionToken}");

            string requestString = JsonSerializer.Serialize(requestMessage);

            httpReq.Content = new StringContent(requestString, Encoding.UTF8, "application/json");

            using (HttpResponseMessage? httpResponse = cancellationToken is null ?
                await client.SendAsync(httpReq) :
                 await client.SendAsync(httpReq, cancellationToken.Value))
            {
                return await ProcessResponseAsync<TR>(httpResponse, cancellationToken);
            }
        }
        
        throw new ChatGPTException("Unxpected code path");
    }

    
    private async Task<T> SendRequestAsync<T>(HttpMethod method, string url, CancellationToken? cancellationToken)  where T : class
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(method, url);

        request.Headers.Add("Authorization", $"Bearer {_sessionToken}");

        using (HttpResponseMessage? httpResponse = cancellationToken is null ?
             await client.SendAsync(request) :
             await client.SendAsync(request, cancellationToken.Value))
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

}
