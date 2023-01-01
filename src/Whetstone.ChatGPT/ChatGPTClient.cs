using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
#if NET6_0_OR_GREATER
using System.Net.Http.Json;
#endif
using System.Text;
using System.Text.Json;
using System.Threading;
using Whetstone.ChatGPT.Models;
using static System.Net.WebRequestMethods;

namespace Whetstone.ChatGPT;


/// <summary>
/// A client for the OpenAI GPT-3 API.
/// </summary>
public class ChatGPTClient : IChatGPTClient
{
    private readonly string _apiKey;

    private readonly HttpClient _client;

    private readonly bool _isHttpClientProvided = true;

    private bool _isDisposed;

    #region Constructors
    /// <summary>
    /// Creates a new instance of the <see cref="ChatGPTClient"/> class.
    /// </summary>
    /// <param name="apiKey">The OpenAI API uses API keys for authentication. Visit your <see href="https://beta.openai.com/account/api-keys">API Keys</see> page to retrieve the API key you'll use in your requests./param>
    /// <exception cref="ArgumentException"></exception>
    public ChatGPTClient(string apiKey) : this(new ChatGPTCredentials(apiKey), null)
    {
    }


    /// <summary>
    /// Creates a new instance of the <see cref="ChatGPTClient"/> class.
    /// </summary>
    /// <param name="apiKey">The OpenAI API uses API keys for authentication. Visit your <see href="https://beta.openai.com/account/api-keys">API Keys</see> page to retrieve the API key you'll use in your requests./param>
    /// <param name="organization">For users who belong to multiple organizations, you can pass a header to specify which organization is used for an API request. Usage from these API requests will count against the specified organization's subscription quota.</param>
    /// <exception cref="ArgumentException"></exception>
    public ChatGPTClient(string apiKey, string organization) : this(new ChatGPTCredentials(apiKey, organization), null)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ChatGPTClient"/> class.
    /// </summary>
    /// <param name="apiKey">The OpenAI API uses API keys for authentication. Visit your <see href="https://beta.openai.com/account/api-keys">API Keys</see> page to retrieve the API key you'll use in your requests./param>
    /// <param name="httpClient">This HttpClient will be used to make requests to the GPT-3 API. The caller is responsible for disposing the HttpClient instance.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public ChatGPTClient(string apiKey, HttpClient httpClient) : this(new ChatGPTCredentials(apiKey), httpClient)
    {
        if (httpClient is null)
        {
            throw new ArgumentNullException(nameof(httpClient));
        }
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ChatGPTClient"/> class.
    /// </summary>
    /// <param name="apiKey">The OpenAI API uses API keys for authentication. Visit your <see href="https://beta.openai.com/account/api-keys">API Keys</see> page to retrieve the API key you'll use in your requests./param>
    /// <param name="organization">For users who belong to multiple organizations, you can pass a header to specify which organization is used for an API request. Usage from these API requests will count against the specified organization's subscription quota.</param>
    /// <param name="httpClient">This HttpClient will be used to make requests to the GPT-3 API. The caller is responsible for disposing the HttpClient instance.</param>
    /// <exception cref="ArgumentException"></exception>
    public ChatGPTClient(string apiKey, string organization, HttpClient httpClient) : this(new ChatGPTCredentials(apiKey, organization), httpClient)
    {
    }


    /// <summary>
    /// Creates a new instance of the <see cref="ChatGPTClient"/> class.
    /// </summary>
    /// <param name="credentials">Supplies the GPT-3 API key and the organization. The organization is only needed if the caller belongs to more than one organziation. See <see cref="https://beta.openai.com/docs/api-reference/requesting-organization">Requesting Organization</see>.</param>
    /// <exception cref="ArgumentException"></exception>
    public ChatGPTClient(ChatGPTCredentials credentials) : this(credentials, null)
    {
    }


    /// <summary>
    /// Creates a new instance of the <see cref="ChatGPTClient"/> class.
    /// </summary>
    /// <param name="credentials">Supplies the GPT-3 API key and the organization. The organization is only needed if the caller belongs to more than one organziation. See <see cref="https://beta.openai.com/docs/api-reference/requesting-organization">Requesting Organization</see>.</param>
    /// <param name="httpClient">This HttpClient will be used to make requests to the GPT-3 API. The caller is responsible for disposing the HttpClient instance.</param>
    /// <exception cref="ArgumentException"></exception>
    public ChatGPTClient(ChatGPTCredentials credentials, HttpClient? httpClient)
    {
        if (string.IsNullOrWhiteSpace(credentials.ApiKey))
        {
            throw new ArgumentException("ApiKey preoperty cannot be null or whitespace.", nameof(credentials));
        }
        _apiKey = credentials.ApiKey;

        if (httpClient is null)
        {
            _client = new HttpClient();
            _isHttpClientProvided = false;
        }
        else
        {
            _client = httpClient;
            _isHttpClientProvided = true;
        }

        InitializeClient(_client, credentials);
    }


    private void InitializeClient(HttpClient client, ChatGPTCredentials creds)
    {

        client.BaseAddress = new Uri("https://api.openai.com/v1/");

        if (!string.IsNullOrWhiteSpace(_client.DefaultRequestHeaders.Authorization?.Parameter))
        {
            throw new ArgumentException("HttpClient already has authorization token.", nameof(client));
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", creds.ApiKey);

        if (!string.IsNullOrWhiteSpace(creds.Organization))
        {
            client.DefaultRequestHeaders.Add("OpenAI-Organization", creds.Organization);
        }
    }

    #endregion

    #region Completions

    /// <summary>
    /// Creates a completion for the provided prompt and parameters
    /// </summary>
    /// <remarks>
    /// <para>See <seealso cref="https://beta.openai.com/docs/api-reference/completions/create">Create completion</seealso>.</para>
    /// </remarks>
    /// <param name="completionRequest">A well-defined prompt for requesting a completion.</param>
    /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
    /// <returns>A completion populated by the GPT-3 API.</returns>
    /// <exception cref="ArgumentNullException">completionRequest is required.</exception>
    /// <exception cref="ArgumentException">Model is required.</exception>
    /// <returns><see cref="ChatGPTModelsResponse"/>Exception generated while processing request.</returns>
    public async Task<ChatGPTCompletionResponse?> CreateCompletionAsync(ChatGPTCompletionRequest completionRequest, CancellationToken? cancellationToken = null)
    {
        if (completionRequest is null)
        {
            throw new ArgumentNullException(nameof(completionRequest));
        }

        if (string.IsNullOrWhiteSpace(completionRequest.Model))
        {
            throw new ArgumentException("Model is required", nameof(completionRequest));
        }

        return await SendRequestAsync<ChatGPTCompletionRequest, ChatGPTCompletionResponse>(HttpMethod.Post, "completions", completionRequest, cancellationToken);
    }

    /// <summary>
    /// Lists the currently available models, and provides basic information about each one such as the owner and availability.
    /// </summary>
    /// <remarks>
    /// <para>For a recommended list of models see <see cref="ChatGPTCompletionModels">ChatGPTCompletionModels</see></para>
    /// <para>See <seealso cref="https://beta.openai.com/docs/api-reference/models/list">List Models</seealso>.</para>
    /// </remarks>
    /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
    /// <returns><see cref="ChatGPTModel">A list of available models.</see></returns>
    /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
    public async Task<ChatGPTListResponse<ChatGPTModel>?> ListModelsAsync(CancellationToken? cancellationToken = null)
    {
        return await SendRequestAsync<ChatGPTListResponse<ChatGPTModel>>(HttpMethod.Get, "models", cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a model instance, providing basic information about the model such as the owner and permissioning.
    /// </summary>
    /// <remarks>
    /// <para>See <seealso cref="https://beta.openai.com/docs/api-reference/models/retrieve">Retrieve Models</seealso>.</para>
    /// </remarks>
    /// <param name="modelId">The ID of the model to use for this request. See <see cref="ChatGPTCompletionModels">ChatGPTCompletionModels</see></param>
    /// <param name="cancellationToken">Optional. Propagates notifications that opertions should be cancelled.</param>
    /// <returns>A generated completion.</returns>
    /// <exception cref="ArgumentException">modelId is required.</exception>
    /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
    public async Task<ChatGPTModel?> RetrieveModelAsync(string modelId, CancellationToken? cancellationToken = null)
    {

        if (string.IsNullOrWhiteSpace(modelId))
        {
            throw new ArgumentException("Cannot be null or whitespace.", nameof(modelId));
        }

        return await SendRequestAsync<ChatGPTModel>(HttpMethod.Get, $"models/{modelId}", cancellationToken).ConfigureAwait(false);
    }

    #endregion Completions

    /// <summary>
    /// Given a prompt and an instruction, the model will return an edited version of the prompt.
    /// </summary>
    /// <remarks>
    /// <para>If <c>Model</c> is not provided, the default of "text-davinci-edit-001" is used. <see cref="ChatGPTEditModels.Davinci">Davinci</see></para>
    /// <para>An example of using this would be to fix any spelling mistakes in the prompt by setting <c>Instruction</c> to "Fix the spelling mistakes".</para>
    /// <para>See <seealso cref="https://beta.openai.com/docs/api-reference/edits/create">Create Edits</seealso></para>
    /// </remarks>
    /// <param name="createEditRequest">Submit an instruction to edit a propmpt.</param>
    /// <param name="cancellationToken">Optional. Propagates notifications that opertions should be cancelled.</param>
    /// <returns>Response to the <c>CreateEditRequest</c> request.</returns>
    /// <exception cref="ArgumentException"><c>CreateEditRequest</c> requires an <c>Instruction</c></exception>
    /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
    public async Task<ChatGPTCreateEditResponse?> CreateEditAsync(ChatGPTCreateEditRequest createEditRequest, CancellationToken? cancellationToken = null)
    {

        if (createEditRequest is null)
        {
            throw new ArgumentNullException(nameof(createEditRequest));
        }

        if (string.IsNullOrWhiteSpace(createEditRequest.Model))
        {
            createEditRequest.Model = ChatGPTEditModels.Davinci;
        }

        if (string.IsNullOrWhiteSpace(createEditRequest.Instruction))
        {
            throw new ArgumentException("Instruction is required", nameof(createEditRequest));
        }

        return await SendRequestAsync<ChatGPTCreateEditRequest, ChatGPTCreateEditResponse>(HttpMethod.Post, "edits", createEditRequest, cancellationToken).ConfigureAwait(false);
    }

    #region File Operations

    /// <summary>
    /// Upload a file that contains document(s) to be used across various endpoints/features. Currently, the size of all the files uploaded by one organization can be up to 1 GB. Please contact us if you need to increase the storage limit.
    /// </summary>
    /// <param name="fileRequest">Defines the file name, contents, and purpose.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">File contents and Purpose are required</exception>
    /// <exception cref="ArgumentException">File contents and Purpose are required</exception>
    public async Task<ChatGPTFileInfo?> UploadFileAsync(ChatGPTUploadFileRequest? fileRequest, CancellationToken? cancellationToken = null)
    {
        if (fileRequest is null)
        {
            throw new ArgumentNullException(nameof(fileRequest));
        }

        if (fileRequest.File is null)
        {
            throw new ArgumentNullException(nameof(fileRequest), "File is required");
        }

        if (string.IsNullOrWhiteSpace(fileRequest.File.FileName))
        {
            throw new ArgumentException("File.FileName is required", nameof(fileRequest));
        }

        if (fileRequest.File.Content is null)
        {
            throw new ArgumentException("File.Content is required", nameof(fileRequest));
        }

        if (string.IsNullOrWhiteSpace(fileRequest.Purpose))
        {
            throw new ArgumentException("Purpose is required", nameof(fileRequest));
        }

        MultipartFormDataContent formContent = new()
        {
            { new StringContent(fileRequest.Purpose), "purpose" },

            { new ByteArrayContent(fileRequest.File.Content), "file", fileRequest.File.FileName }
        };

        using (var httpReq = new HttpRequestMessage(HttpMethod.Post, "files"))
        {
            httpReq.Content = formContent;

            httpReq.Headers.Add("Authorization", $"Bearer {_apiKey}");

            using (HttpResponseMessage? httpResponse = cancellationToken is null ?
                await _client.SendAsync(httpReq).ConfigureAwait(false) :
                 await _client.SendAsync(httpReq, cancellationToken.Value).ConfigureAwait(false))
            {
                return await ProcessResponseAsync<ChatGPTFileInfo>(httpResponse, cancellationToken);
            }
        }
    }

    /// <summary>
    /// Returns a list of files that belong to the user's organization.
    /// </summary>
    /// <remarks>
    /// <para>See <seealso cref="https://beta.openai.com/docs/api-reference/files/list">List Files</seealso>.</para>
    /// </remarks>
    /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
    /// <returns><see cref="ChatGPTFileInfo">A list of available models.</see></returns>
    /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
    public async Task<ChatGPTListResponse<ChatGPTFileInfo>?> ListFilesAsync(CancellationToken? cancellationToken = null)
    {
        return await SendRequestAsync<ChatGPTListResponse<ChatGPTFileInfo>>(HttpMethod.Get, "files", cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Delete a file.
    /// </summary>
    /// <param name="fileId">Id of the file to delete</param>
    /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
    /// <returns>Confirmation the file was deleted.</returns>
    /// <exception cref="ArgumentException">fileId cannot be null or whitespace</exception>
    /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
    public async Task<ChatGPTDeleteResponse?> DeleteFileAsync(string? fileId, CancellationToken? cancellationToken = null)
    {
        if (string.IsNullOrWhiteSpace(fileId))
        {
            throw new ArgumentException("Cannot be null or whitespace.", nameof(fileId));
        }

        return await SendRequestAsync<ChatGPTDeleteResponse>(HttpMethod.Delete, $"files/{fileId}", cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Returns information about a specific file.
    /// </summary>
    /// <param name="fileId">Id of the file to retrieve for its info.</param>
    /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
    /// <returns>Confirmation the file was deleted.</returns>
    /// <exception cref="ArgumentException">fileId cannot be null or whitespace</exception>
    /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
    public async Task<ChatGPTFileInfo?> RetrieveFileAsync(string? fileId, CancellationToken? cancellationToken = null)
    {
        if (string.IsNullOrWhiteSpace(fileId))
        {
            throw new ArgumentException("Cannot be null or whitespace.", nameof(fileId));
        }

        return await SendRequestAsync<ChatGPTFileInfo>(HttpMethod.Get, $"files/{fileId}", cancellationToken).ConfigureAwait(false);
    }


    /// <summary>
    /// Returns the contents of the specified file
    /// </summary>
    /// <param name="fileId">The ID of the file to use for this request</param>
    /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<ChatGPTFileContent?> RetrieveFileContentAsync(string? fileId, CancellationToken? cancellationToken = null)
    {
        ChatGPTFileContent fileContent;

        if (string.IsNullOrWhiteSpace(fileId))
        {
            throw new ArgumentException("Cannot be null or whitespace.", nameof(fileId));
        }

        using (var httpReq = new HttpRequestMessage(HttpMethod.Get, $"files/{fileId}/content"))
        {

            httpReq.Headers.Add("Authorization", $"Bearer {_apiKey}");

            using (HttpResponseMessage? httpResponse = cancellationToken is null ?
                await _client.SendAsync(httpReq) :
                await _client.SendAsync(httpReq, cancellationToken.Value))
            {

                if (httpResponse.IsSuccessStatusCode)
                {

                    fileContent = new();

#if NETSTANDARD2_1
                    fileContent.Content = await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
#else
                    fileContent.Content = cancellationToken is null ?
                                await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false) :
                                await httpResponse.Content.ReadAsByteArrayAsync(cancellationToken.Value).ConfigureAwait(false);
#endif

                    fileContent.FileName = httpResponse.Content?.Headers?.ContentDisposition?.FileName?.Replace(@"""", "");

                    return fileContent;
                }
                else
                {

                    string responseString = await GetResponseStringAsync(httpResponse, cancellationToken).ConfigureAwait(false);

                    ChatGPTErrorResponse? errResponse = JsonSerializer.Deserialize<ChatGPTErrorResponse>(responseString);
                    throw new ChatGPTException(errResponse?.Error, httpResponse.StatusCode);
                }
            }
        }

    }

    #endregion

    #region Generic Request and Response Processors
    private async Task<TR?> SendRequestAsync<T, TR>(HttpMethod method, string url, T requestMessage, CancellationToken? cancellationToken)
        where T : class
        where TR : class
    {
        using (var httpReq = new HttpRequestMessage(method, url))
        {

            httpReq.Headers.Add("Authorization", $"Bearer {_apiKey}");

            string requestString = JsonSerializer.Serialize(requestMessage);

            httpReq.Content = new StringContent(requestString, Encoding.UTF8, "application/json");

            using (HttpResponseMessage? httpResponse = cancellationToken is null ?
                await _client.SendAsync(httpReq) :
                 await _client.SendAsync(httpReq, cancellationToken.Value))
            {
                return await ProcessResponseAsync<TR>(httpResponse, cancellationToken).ConfigureAwait(false);
            }
        }

    }


    private async Task<T?> SendRequestAsync<T>(HttpMethod method, string url, CancellationToken? cancellationToken) where T : class
    {
        using (var request = new HttpRequestMessage(method, url))
        {
            request.Headers.Add("Authorization", $"Bearer {_apiKey}");

            using (HttpResponseMessage? httpResponse = cancellationToken is null ?
                 await _client.SendAsync(request).ConfigureAwait(false) :
                 await _client.SendAsync(request, cancellationToken.Value).ConfigureAwait(false))
            {
                return await ProcessResponseAsync<T>(httpResponse, cancellationToken).ConfigureAwait(false);

            }
        }

    }

    private async static Task<T?> ProcessResponseAsync<T>(HttpResponseMessage responseMessage, CancellationToken? cancellationToken) where T : class
    {
        string responseString = await GetResponseStringAsync(responseMessage, cancellationToken).ConfigureAwait(false);

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
            throw new ChatGPTException(errResponse?.Error, responseMessage.StatusCode);
        }

        return default(T);
    }

    private static async Task<string> GetResponseStringAsync(HttpResponseMessage responseMessage, CancellationToken? cancellationToken)
    {
#if NETSTANDARD2_1
        string responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
#else
        string responseString = cancellationToken is null ?
                await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false) :
                await responseMessage.Content.ReadAsStringAsync(cancellationToken.Value).ConfigureAwait(false);
#endif

        return responseString;
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
