﻿// SPDX-License-Identifier: MIT
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
#if NET6_0_OR_GREATER
using System.Net.Http.Json;
#endif
using System.Text;
using System.Text.Json;
using Whetstone.ChatGPT.Models;
using Whetstone.ChatGPT.Models.Audio;
using Whetstone.ChatGPT.Models.Embeddings;
using Whetstone.ChatGPT.Models.File;
using Whetstone.ChatGPT.Models.FineTuning;
using Whetstone.ChatGPT.Models.Image;
using Whetstone.ChatGPT.Models.Moderation;
using Whetstone.ChatGPT.Models.Vision;

namespace Whetstone.ChatGPT;

/// <summary>
/// A client for the OpenAI GPT-3 API.
/// </summary>
public class ChatGPTClient : IChatGPTClient
{
    private const string ResponseLinePrefix = "data: ";

    private readonly HttpClient _client;

    private readonly bool _isHttpClientProvided = true;

    private ChatGPTCredentials? _chatCredentials;

    private bool _isDisposed;

    public ChatGPTCredentials? Credentials { set => _chatCredentials = value; }

    #region Constructors

    /// <summary>
    /// Creates a new instance of the <see cref="ChatGPTClient"/> class.
    /// </summary>
    /// <param name="apiKey">The OpenAI API uses API keys for authentication. Visit your <see href="https://beta.openai.com/account/api-keys">API Keys</see> page to retrieve the API key you'll use in your requests./param>
    /// <exception cref="ArgumentException"></exception>
    public ChatGPTClient(string apiKey) : this(credentials: new ChatGPTCredentials(apiKey), httpClient: new HttpClient())
    {
    }


    /// <summary>
    /// Creates a new instance of the <see cref="ChatGPTClient"/> class.
    /// </summary>
    /// <param name="apiKey">The OpenAI API uses API keys for authentication. Visit your <see href="https://beta.openai.com/account/api-keys">API Keys</see> page to retrieve the API key you'll use in your requests./param>
    /// <param name="organization">For users who belong to multiple organizations, you can pass a header to specify which organization is used for an API request. Usage from these API requests will count against the specified organization's subscription quota.</param>
    /// <exception cref="ArgumentException"></exception>
    public ChatGPTClient(string apiKey, string organization) : this(new ChatGPTCredentials(apiKey, organization), httpClient: new HttpClient())
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ChatGPTClient"/> class.
    /// </summary>
    /// <param name="credentials">Supplies the GPT-3 API key and the organization. The organization is only needed if the caller belongs to more than one organziation. See <see cref="https://beta.openai.com/docs/api-reference/requesting-organization">Requesting Organization</see>.</param>
    /// <exception cref="ArgumentException"></exception>
    public ChatGPTClient(ChatGPTCredentials credentials) : this(credentials: credentials, httpClient: new HttpClient())
    {
    }


    /// <summary>
    /// Creates a new instance of the <see cref="ChatGPTClient"/> class.
    /// </summary>
    /// <param name="credentialsOptions">Supplies the GPT-3 API key and the organization. The organization is only needed if the caller belongs to more than one organziation. See <see cref="https://beta.openai.com/docs/api-reference/requesting-organization">Requesting Organization</see>.</param>    
    /// <exception cref="ArgumentException">API Key is required.</exception>
    public ChatGPTClient(IOptions<ChatGPTCredentials> credentialsOptions) : this(credentials: credentialsOptions.Value, httpClient: new HttpClient())
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ChatGPTClient"/> class.
    /// </summary>
    /// <param name="credentialsOptions">Supplies the GPT-3 API key and the organization. The organization is only needed if the caller belongs to more than one organziation. See <see cref="https://beta.openai.com/docs/api-reference/requesting-organization">Requesting Organization</see>.</param>
    /// <param name="httpClient">This HttpClient will be used to make requests to the GPT-3 API. The caller is responsible for disposing the HttpClient instance.</param>
    /// <exception cref="ArgumentException">API Key is required.</exception>
    public ChatGPTClient(IOptions<ChatGPTCredentials> credentialsOptions, HttpClient httpClient) : this(credentials: credentialsOptions.Value, httpClient: httpClient)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ChatGPTClient"/> class.
    /// </summary>
    /// <param name="credentials">Supplies the GPT-3 API key and the organization. The organization is only needed if the caller belongs to more than one organziation. See <see cref="https://beta.openai.com/docs/api-reference/requesting-organization">Requesting Organization</see>.</param>
    /// <param name="httpClient">This HttpClient will be used to make requests to the GPT-3 API. The caller is responsible for disposing the HttpClient instance.</param>
    /// <exception cref="ArgumentException">API Key is required.</exception>
    private ChatGPTClient(ChatGPTCredentials credentials, HttpClient httpClient)
    {
        _chatCredentials = credentials;

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

        InitializeClient(_client);
    }
    private void InitializeClient(HttpClient client)
    {

        client.BaseAddress = new Uri("https://api.openai.com/v1/");

        if (!string.IsNullOrWhiteSpace(_client.DefaultRequestHeaders.Authorization?.Parameter))
        {
            throw new ArgumentException("HttpClient already has authorization token.", nameof(client));
        }
    }

    #endregion

    #region Chat Completions
    public async Task<ChatGPTChatCompletionResponse?> CreateChatCompletionAsync(ChatGPTChatCompletionRequest completionRequest, CancellationToken? cancellationToken = null)
    {
        if (completionRequest is null)
        {
            throw new ArgumentNullException(nameof(completionRequest));
        }

        if (string.IsNullOrWhiteSpace(completionRequest.Model))
        {
            throw new ArgumentException("Model is required", nameof(completionRequest));
        }

        if (completionRequest.Messages is null || completionRequest.Messages.Count == 0)
        {
            throw new ArgumentException("Message is required", nameof(completionRequest));
        }

        completionRequest.Stream = false;

        return await SendRequestAsync<ChatGPTChatCompletionRequest, ChatGPTChatCompletionResponse>(HttpMethod.Post, "chat/completions", completionRequest, cancellationToken);
    }

    public async Task<ChatGPTChatCompletionResponse?> CreateVisionCompletionAsync(ChatGPTCompletionVisionRequest completionRequest, CancellationToken? cancellationToken = null)
    {
        if (completionRequest is null)
        {
            throw new ArgumentNullException(nameof(completionRequest));
        }

        if (string.IsNullOrWhiteSpace(completionRequest.Model))
        {
            throw new ArgumentException("Model is required", nameof(completionRequest));
        }

        if (completionRequest.Messages is null || !completionRequest.Messages.Any())
        {
            throw new ArgumentException("Message is required", nameof(completionRequest));
        }

        return await SendRequestAsync<ChatGPTCompletionVisionRequest, ChatGPTChatCompletionResponse>(HttpMethod.Post, "chat/completions", completionRequest, cancellationToken);
    }

    /// <inheritdoc cref="IChatGPTClient.StreamChatCompletionAsync"/>
    public async IAsyncEnumerable<ChatGPTChatCompletionStreamResponse?> StreamChatCompletionAsync(ChatGPTChatCompletionRequest completionRequest, CancellationToken? cancellationToken = null)
    {

        if (completionRequest is null)
        {
            throw new ArgumentNullException(nameof(completionRequest));
        }

        if (string.IsNullOrWhiteSpace(completionRequest.Model))
        {
            throw new ArgumentException("Model is required", nameof(completionRequest));
        }

        if (completionRequest.Messages is null || completionRequest.Messages.Count == 0)
        {
            throw new ArgumentException("Message is required", nameof(completionRequest));
        }

        completionRequest.Stream = true;

        using HttpRequestMessage httpReq = CreateRequestMessage(HttpMethod.Post, "chat/completions");

        CancellationToken cancelToken = cancellationToken ?? CancellationToken.None;

        string requestString = JsonSerializer.Serialize(completionRequest);

        httpReq.Content = new StringContent(requestString, Encoding.UTF8, "application/json");

        HttpResponseMessage responseMessage = await _client.SendAsync(httpReq, HttpCompletionOption.ResponseHeadersRead, cancelToken).ConfigureAwait(false);


        if (responseMessage.IsSuccessStatusCode)
        {

#if NETSTANDARD2_1 || NETSTANDARD2_0
            using Stream responseStream = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
#else
            using Stream responseStream = await responseMessage.Content.ReadAsStreamAsync(cancelToken).ConfigureAwait(false);
#endif
            using StreamReader reader = new(responseStream);
            string? line = null;

            while ((line = await reader.ReadLineAsync()) is not null)
            {

                if (line.StartsWith(ResponseLinePrefix, System.StringComparison.OrdinalIgnoreCase))
                {
                    line = line.Substring(ResponseLinePrefix.Length).Trim();

                    if (!string.IsNullOrWhiteSpace(line) && line != "[DONE]")
                    {
                        ChatGPTChatCompletionStreamResponse? streamedResponse = null;
                        try
                        {
                            streamedResponse = JsonSerializer.Deserialize<ChatGPTChatCompletionStreamResponse?>(line);
                        }
                        catch (JsonException jsonEx)
                        {
                            throw new ChatGPTException($"Error deserializing ChatGPT streamed chat response: {line}",
                                responseMessage.StatusCode,
                                jsonEx);
                        }

                        ChatGPTStreamedChatChoice? foundChoice = streamedResponse?.GetChoice();

                        if (foundChoice is not null)
                        {
                            if (!(string.Equals(foundChoice.FinishReason, "stop", StringComparison.OrdinalIgnoreCase) || foundChoice.Delta is null || foundChoice.Delta.Content is null))
                            {
                                yield return streamedResponse;
                            }
                        }
                    }
                }
            }
        }
        else
        {
#if NETSTANDARD2_1 || NETSTANDARD2_0
            string? responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
#else
            string? responseString = await responseMessage.Content.ReadAsStringAsync(cancelToken).ConfigureAwait(false);
#endif
            ProcessErrorResponse(responseMessage.StatusCode, responseString);
        }
    }

    #endregion

    #region Completions

    /// <inheritdoc cref="IChatGPTClient.CreateCompletionAsync"/>
    [Obsolete("Use CreateChatCompletionAsync instead")]
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

        if (string.IsNullOrWhiteSpace(completionRequest.Prompt))
        {
            throw new ArgumentException("Prompt is required", nameof(completionRequest));
        }

        completionRequest.Stream = false;

        return await SendRequestAsync<ChatGPTCompletionRequest, ChatGPTCompletionResponse>(HttpMethod.Post, "completions", completionRequest, cancellationToken);
    }

    /// <inheritdoc cref="IChatGPTClient.ListModelsAsync"/>
    public async Task<ChatGPTListResponse<ChatGPTModel>?> ListModelsAsync(CancellationToken? cancellationToken = null)
    {
        return await SendRequestAsync<ChatGPTListResponse<ChatGPTModel>>(HttpMethod.Get, "models", cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IChatGPTClient.RetrieveModelAsync"/>
    public async Task<ChatGPTModel?> RetrieveModelAsync(string modelId, CancellationToken? cancellationToken = null)
    {

        if (string.IsNullOrWhiteSpace(modelId))
        {
            throw new ArgumentException("Cannot be null or whitespace.", nameof(modelId));
        }

        return await SendRequestAsync<ChatGPTModel>(HttpMethod.Get, $"models/{modelId}", cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IChatGPTClient.DeleteModelAsync"/>
    public async Task<ChatGPTDeleteResponse?> DeleteModelAsync(string? modelId, CancellationToken? cancellationToken = null)
    {
        if (string.IsNullOrWhiteSpace(modelId))
        {
            throw new ArgumentException("Cannot be null or whitespace.", nameof(modelId));
        }

        return await SendRequestAsync<ChatGPTDeleteResponse>(HttpMethod.Delete, $"models/{modelId}", cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IChatGPTClient.StreamCompletionAsync"/>
    [Obsolete("Use StreamChatCompletion instead")]
    public async IAsyncEnumerable<ChatGPTCompletionStreamResponse?> StreamCompletionAsync(ChatGPTCompletionRequest completionRequest, CancellationToken? cancellationToken = null)
    {

        if (completionRequest is null)
        {
            throw new ArgumentNullException(nameof(completionRequest));
        }

        if (string.IsNullOrWhiteSpace(completionRequest.Model))
        {
            throw new ArgumentException("Model is required", nameof(completionRequest));
        }

        if (string.IsNullOrWhiteSpace(completionRequest.Prompt))
        {
            throw new ArgumentException("Prompt is required", nameof(completionRequest));
        }

        completionRequest.Stream = true;

        using HttpRequestMessage httpReq = CreateRequestMessage(HttpMethod.Post, "completions");

        CancellationToken cancelToken = cancellationToken ?? CancellationToken.None;

        string requestString = JsonSerializer.Serialize(completionRequest);

        httpReq.Content = new StringContent(requestString, Encoding.UTF8, "application/json");

        HttpResponseMessage responseMessage = await _client.
            SendAsync(httpReq, HttpCompletionOption.ResponseHeadersRead, cancelToken).
            ConfigureAwait(false);

        if (responseMessage.IsSuccessStatusCode)
        {

#if NETSTANDARD2_1 || NETSTANDARD2_0
            using Stream responseStream = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
#else
            using Stream responseStream = await responseMessage.Content.ReadAsStreamAsync(cancelToken).ConfigureAwait(false);
#endif
            using StreamReader reader = new(responseStream);
            string? line = null;


            while ((line = await reader.ReadLineAsync()) is not null)
            {

                if (line.StartsWith(ResponseLinePrefix, System.StringComparison.OrdinalIgnoreCase))
                    line = line.Substring(ResponseLinePrefix.Length);

                if (!string.IsNullOrWhiteSpace(line) && line != "[DONE]")
                {
                    ChatGPTCompletionStreamResponse? streamedResponse;

                    string trimmedLine = line.Trim();
                    try
                    {
                        streamedResponse = JsonSerializer.Deserialize<ChatGPTCompletionStreamResponse>(trimmedLine);
                    }
                    catch (JsonException jsonEx)
                    {
                        throw new ChatGPTException($"Error deserializing ChatGPT streamed chat response: {trimmedLine}",
                            responseMessage.StatusCode,
                            jsonEx);
                    }

                    yield return streamedResponse;
                }
            }
        }
        else
        {
#if NETSTANDARD2_1 || NETSTANDARD2_0
            string? responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
#else
            string? responseString = await responseMessage.Content.ReadAsStringAsync(cancelToken).ConfigureAwait(false);
#endif
            ProcessErrorResponse(responseMessage.StatusCode, responseString);
        }
    }

    #endregion Completions

    #region File Operations

    /// <inheritdoc cref="IChatGPTClient.UploadFileAsync"/>
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

        using (HttpRequestMessage httpReq = CreateRequestMessage(HttpMethod.Post, "files"))
        {
            httpReq.Content = formContent;

            using (HttpResponseMessage? httpResponse = cancellationToken is null ?
                await _client.SendAsync(httpReq).ConfigureAwait(false) :
                 await _client.SendAsync(httpReq, cancellationToken.Value).ConfigureAwait(false))
            {
                return await ProcessResponseAsync<ChatGPTFileInfo>(httpResponse, cancellationToken);
            }
        }
    }

    /// <inheritdoc cref="IChatGPTClient.ListFilesAsync"/>
    public async Task<ChatGPTListResponse<ChatGPTFileInfo>?> ListFilesAsync(CancellationToken? cancellationToken = null)
    {
        return await SendRequestAsync<ChatGPTListResponse<ChatGPTFileInfo>>(HttpMethod.Get, "files", cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IChatGPTClient.DeleteFileAsync"/>
    public async Task<ChatGPTDeleteResponse?> DeleteFileAsync(string? fileId, CancellationToken? cancellationToken = null)
    {
        if (string.IsNullOrWhiteSpace(fileId))
        {
            throw new ArgumentException("Cannot be null or whitespace.", nameof(fileId));
        }

        return await SendRequestAsync<ChatGPTDeleteResponse>(HttpMethod.Delete, $"files/{fileId}", cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IChatGPTClient.RetrieveFileAsync"/>
    public async Task<ChatGPTFileInfo?> RetrieveFileAsync(string? fileId, CancellationToken? cancellationToken = null)
    {
        if (string.IsNullOrWhiteSpace(fileId))
        {
            throw new ArgumentException("Cannot be null or whitespace.", nameof(fileId));
        }

        return await SendRequestAsync<ChatGPTFileInfo>(HttpMethod.Get, $"files/{fileId}", cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IChatGPTClient.RetrieveFileContentAsync"/>
    public async Task<ChatGPTFileContent?> RetrieveFileContentAsync(string? fileId, CancellationToken? cancellationToken = null)
    {
        ChatGPTFileContent fileContent;

        if (string.IsNullOrWhiteSpace(fileId))
        {
            throw new ArgumentException("Cannot be null or whitespace.", nameof(fileId));
        }

        CancellationToken cancelToken = cancellationToken ?? CancellationToken.None;

        using HttpRequestMessage httpReq = CreateRequestMessage(HttpMethod.Get, $"files/{fileId}/content");

        using HttpResponseMessage? httpResponse = await _client.SendAsync(httpReq, cancelToken).ConfigureAwait(false);

        if (httpResponse.IsSuccessStatusCode)
        {

            fileContent = new();
#if NETSTANDARD2_1 || NETSTANDARD2_0
            fileContent.Content = await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
#else
            fileContent.Content = await httpResponse.Content.ReadAsByteArrayAsync(cancelToken).ConfigureAwait(false);
#endif

            fileContent.FileName = httpResponse.Content?.Headers?.ContentDisposition?.FileName?.Replace(@"""", "");

            return fileContent;
        }
        else
        {
            string responseString = await GetResponseStringAsync(httpResponse, cancelToken).ConfigureAwait(false);
            ProcessErrorResponse(httpResponse.StatusCode, responseString);
            return null;
        }
    }

    #endregion

    #region Fine Tunes

    /// <inheritdoc cref="IChatGPTClient.CreateFineTuneAsync"/>
    public async Task<ChatGPTFineTuneJob?> CreateFineTuneAsync(ChatGPTCreateFineTuneRequest? createFineTuneRequest, CancellationToken? cancellationToken = null)
    {
        if (createFineTuneRequest is null)
        {
            throw new ArgumentNullException(nameof(createFineTuneRequest));
        }

        if (string.IsNullOrWhiteSpace(createFineTuneRequest.Model))
        {
            throw new ArgumentNullException(nameof(createFineTuneRequest), "Model property cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(createFineTuneRequest.TrainingFileId))
        {
            throw new ArgumentException("TrainingFileId cannot be null or whitespace.", nameof(createFineTuneRequest));
        }

        return await SendRequestAsync<ChatGPTCreateFineTuneRequest, ChatGPTFineTuneJob>(HttpMethod.Post, "fine_tuning/jobs", createFineTuneRequest, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IChatGPTClient.ListFineTuneJobsAsync"/>
    public async Task<ChatGPTListResponse<ChatGPTFineTuneJob>?> ListFineTuneJobsAsync(int limit = 20, string? after = null, CancellationToken? cancellationToken = null)
    {
        string route = string.IsNullOrEmpty(after) ?
            $"fine_tuning/jobs?limit={limit}" :
            $"fine_tuning/jobs?limit={limit}&after={after}";
        
        return await SendRequestAsync<ChatGPTListResponse<ChatGPTFineTuneJob>>(HttpMethod.Get, route, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IChatGPTClient.RetrieveFineTuneAsync"/>
    public async Task<ChatGPTFineTuneJob?> RetrieveFineTuneAsync(string? fineTuneId, CancellationToken? cancellationToken = null)
    {
        if (string.IsNullOrWhiteSpace(fineTuneId))
        {
            throw new ArgumentException("Cannot be null or whitespace.", nameof(fineTuneId));
        }

        return await SendRequestAsync<ChatGPTFineTuneJob>(HttpMethod.Get, $"fine_tuning/jobs/{fineTuneId}", cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IChatGPTClient.CancelFineTuneAsync"/>
    public async Task<ChatGPTFineTuneJob?> CancelFineTuneAsync(string? fineTuneId, CancellationToken? cancellationToken = null)
    {
        if (string.IsNullOrWhiteSpace(fineTuneId))
        {
            throw new ArgumentException("Cannot be null or whitespace.", nameof(fineTuneId));
        }

        return await SendRequestAsync<ChatGPTFineTuneJob>(HttpMethod.Post, $"fine_tuning/jobs/{fineTuneId}/cancel", cancellationToken).ConfigureAwait(false);
    }


    /// <inheritdoc cref="IChatGPTClient.ListFineTuneEventsAsync"/>
    public async Task<ChatGPTListResponse<ChatGPTEvent>?> ListFineTuneEventsAsync(string? fineTuneId, int limit = 20, string? after=null, CancellationToken? cancellationToken = null)
    {
        string route = string.IsNullOrEmpty(after) ?
            $"fine_tuning/jobs/{fineTuneId}/events?limit={limit}" :
            $"fine_tuning/jobs/{fineTuneId}/events?limit={limit}&after={after}";
        
        if (string.IsNullOrWhiteSpace(fineTuneId))
        {
            throw new ArgumentException("Cannot be null or whitespace.", nameof(fineTuneId));
        }

        return await SendRequestAsync<ChatGPTListResponse<ChatGPTEvent>>(HttpMethod.Get, route, cancellationToken).ConfigureAwait(false);
    }

    #endregion

    /// <inheritdoc cref="IChatGPTClient.CreateFineTuneAsync"/>
    public async Task<ChatGPTCreateModerationResponse?> CreateModerationAsync(ChatGPTCreateModerationRequest? createModerationRequest, CancellationToken? cancellationToken = null)
    {
        if (createModerationRequest is null)
        {
            throw new ArgumentNullException(nameof(createModerationRequest));
        }

        if (createModerationRequest.Inputs is null)
        {
            throw new ArgumentException($"Inputs cannot be null", nameof(createModerationRequest));
        }


        if (!createModerationRequest.Inputs.Any())
        {
            throw new ArgumentException("Inputs must have one or more items", nameof(createModerationRequest));
        }

        return await SendRequestAsync<ChatGPTCreateModerationRequest, ChatGPTCreateModerationResponse>(HttpMethod.Post, "moderations", createModerationRequest, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IChatGPTClient.CreateFineTuneAsync"/>
    public async Task<ChatGPTCreateEmbeddingsResponse?> CreateEmbeddingsAsync(ChatGPTCreateEmbeddingsRequest? createEmbeddingsRequest, CancellationToken? cancellationToken = null)
    {
        if (createEmbeddingsRequest is null)
        {
            throw new ArgumentNullException(nameof(createEmbeddingsRequest));
        }

        if (createEmbeddingsRequest.Inputs is null)
        {
            throw new ArgumentException($"Inputs cannot be null", nameof(createEmbeddingsRequest));
        }

        if (string.IsNullOrWhiteSpace(createEmbeddingsRequest.Model))
        {
            throw new ArgumentException($"Model cannot be null or empty", nameof(createEmbeddingsRequest));
        }

        return await SendRequestAsync<ChatGPTCreateEmbeddingsRequest, ChatGPTCreateEmbeddingsResponse>(HttpMethod.Post, "embeddings", createEmbeddingsRequest, cancellationToken).ConfigureAwait(false);
    }


    #region Image Operations
    /// <inheritdoc cref="IChatGPTClient.CreateFineTuneAsync"/>
    public async Task<ChatGPTImageResponse?> CreateImageAsync(ChatGPTCreateImageRequest? createImageRequest, CancellationToken? cancellationToken = null)
    {
        if (createImageRequest is null)
        {
            throw new ArgumentNullException(nameof(createImageRequest));
        }

        if (string.IsNullOrWhiteSpace(createImageRequest.Prompt))
        {
            throw new ArgumentException($"Prompt cannot be null or empty", nameof(createImageRequest));
        }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        if (createImageRequest.Prompt.Length > 1000)
        {
            throw new ArgumentException($"Prompt cannot be longer than 1000 characters", nameof(createImageRequest));
        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        if (createImageRequest.NumberOfImagesToGenerate < 0)
        {
            throw new ArgumentException($"NumberOfImagesToGenerate must be between 1 and 10", nameof(createImageRequest));
        }

        if (createImageRequest.NumberOfImagesToGenerate > 10)
        {
            throw new ArgumentException($"NumberOfImagesToGenerate must be between 1 and 10", nameof(createImageRequest));
        }

        return await SendRequestAsync<ChatGPTCreateImageRequest, ChatGPTImageResponse>(HttpMethod.Post, "images/generations", createImageRequest, cancellationToken).ConfigureAwait(false);
    }


    /// <inheritdoc cref="IChatGPTClient.CreateFineTuneAsync"/>
    public async Task<ChatGPTImageResponse?> CreateImageVariationAsync(ChatGPTCreateImageVariationRequest? imageVariationRequest, CancellationToken? cancellationToken = null)
    {
        if (imageVariationRequest is null)
        {
            throw new ArgumentNullException(nameof(imageVariationRequest));
        }

        if (imageVariationRequest.NumberOfImagesToGenerate < 0)
        {
            throw new ArgumentException($"NumberOfImagesToGenerate must be between 1 and 10", nameof(imageVariationRequest));
        }

        if (imageVariationRequest.NumberOfImagesToGenerate > 10)
        {
            throw new ArgumentException($"NumberOfImagesToGenerate must be between 1 and 10", nameof(imageVariationRequest));
        }

        if (imageVariationRequest.Image is null)
        {
            throw new ArgumentException($"Image cannot be null", nameof(imageVariationRequest));
        }

        if (string.IsNullOrWhiteSpace(imageVariationRequest.Image.FileName))
        {
            throw new ArgumentException($"Image.FileName cannot be null or empty", nameof(imageVariationRequest));
        }

        if (imageVariationRequest.Image.Content is null)
        {
            throw new ArgumentException($"Image.Content cannot be null", nameof(imageVariationRequest));
        }

        if (imageVariationRequest.Image.Content.Length == 0)
        {
            throw new ArgumentException($"Image.Content.Length cannot be 0", nameof(imageVariationRequest));
        }

        MultipartFormDataContent formContent = new()
        {
            { new ByteArrayContent(imageVariationRequest.Image.Content), "image", imageVariationRequest.Image.FileName },
            { new StringContent(imageVariationRequest.Size.GetDescriptionFromEnumValue<CreatedImageSize>()), "size" },
            { new StringContent(imageVariationRequest.ResponseFormat.GetDescriptionFromEnumValue<CreatedImageFormat>()), "response_format" }
        };


        if (imageVariationRequest.NumberOfImagesToGenerate != 1)
            formContent.Add(new StringContent(imageVariationRequest.NumberOfImagesToGenerate.ToString(CultureInfo.InvariantCulture)), "n");


        if (!string.IsNullOrWhiteSpace(imageVariationRequest.User))
            formContent.Add(new StringContent(imageVariationRequest.User), "user");


        HttpRequestMessage httpReq = CreateRequestMessage(HttpMethod.Post, "images/variations");

        httpReq.Content = formContent;

        using HttpResponseMessage? httpResponse = cancellationToken is null ?
            await _client.SendAsync(httpReq).ConfigureAwait(false) :
            await _client.SendAsync(httpReq, cancellationToken.Value).ConfigureAwait(false);

        return await ProcessResponseAsync<ChatGPTImageResponse>(httpResponse, cancellationToken);
    }


    /// <inheritdoc cref="IChatGPTClient.CreateFineTuneAsync"/>
    public async Task<ChatGPTImageResponse?> CreateImageEditAsync(ChatGPTCreateImageEditRequest? imageEditRequest, CancellationToken? cancellationToken = null)
    {
        if (imageEditRequest is null)
        {
            throw new ArgumentNullException(nameof(imageEditRequest));
        }

        if (string.IsNullOrWhiteSpace(imageEditRequest.Prompt))
        {
            throw new ArgumentException($"Prompt cannot be null or empty", nameof(imageEditRequest));
        }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        if (imageEditRequest.Prompt.Length > 1000)
        {
            throw new ArgumentException($"Prompt cannot be longer than 1000 characters", nameof(imageEditRequest));
        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        if (imageEditRequest.NumberOfImagesToGenerate < 0)
        {
            throw new ArgumentException($"NumberOfImagesToGenerate must be between 1 and 10", nameof(imageEditRequest));
        }

        if (imageEditRequest.NumberOfImagesToGenerate > 10)
        {
            throw new ArgumentException($"NumberOfImagesToGenerate must be between 1 and 10", nameof(imageEditRequest));
        }

        if (imageEditRequest.Image is null)
        {
            throw new ArgumentException($"Image cannot be null", nameof(imageEditRequest));
        }

        if (string.IsNullOrWhiteSpace(imageEditRequest.Image.FileName))
        {
            throw new ArgumentException($"Image.FileName cannot be null or empty", nameof(imageEditRequest));
        }

        if (imageEditRequest.Image.Content is null)
        {
            throw new ArgumentException($"Image.Content cannot be null", nameof(imageEditRequest));
        }

        if (imageEditRequest.Image.Content.Length == 0)
        {
            throw new ArgumentException($"Image.Content.Length cannot be 0", nameof(imageEditRequest));
        }

        ByteArrayContent? maskContent = null;

        if (imageEditRequest.Mask is not null)
        {
            if (imageEditRequest.Mask.Content is null)
            {
                throw new ArgumentException($"If Mask is provided, then Mask.Content cannot be null", nameof(imageEditRequest));
            }

            if (imageEditRequest.Mask.Content.Length == 0)
            {
                throw new ArgumentException($"If Mask is provided, then Mask.Content.Length cannot be 0", nameof(imageEditRequest));
            }

            if (string.IsNullOrWhiteSpace(imageEditRequest.Mask.FileName))
            {
                throw new ArgumentException($"If Mask is provided, then Mask.FileName cannot be null or empty", nameof(imageEditRequest));
            }

            maskContent = new ByteArrayContent(imageEditRequest.Mask.Content);
        }


        MultipartFormDataContent formContent = new()
        {
            { new ByteArrayContent(imageEditRequest.Image.Content), "image", imageEditRequest.Image.FileName },
            { new StringContent(imageEditRequest.Prompt), "prompt" },
            { new StringContent(imageEditRequest.Size.GetDescriptionFromEnumValue<CreatedImageSize>()), "size" },
            { new StringContent(imageEditRequest.ResponseFormat.GetDescriptionFromEnumValue<CreatedImageFormat>()), "response_format" }
        };


        if (maskContent is not null)
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
            formContent.Add(maskContent, "mask", imageEditRequest.Mask.FileName);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.


        if (imageEditRequest.NumberOfImagesToGenerate != 1)
            formContent.Add(new StringContent(imageEditRequest.NumberOfImagesToGenerate.ToString(CultureInfo.InvariantCulture)), "n");


        if (!string.IsNullOrWhiteSpace(imageEditRequest.User))
            formContent.Add(new StringContent(imageEditRequest.User), "user");


        using HttpRequestMessage httpReq = CreateRequestMessage(HttpMethod.Post, "images/edits");
        httpReq.Content = formContent;

        using HttpResponseMessage? httpResponse = await _client.SendAsync(
            httpReq,
            cancellationToken is null ? CancellationToken.None : cancellationToken.Value).ConfigureAwait(false);

        return await ProcessResponseAsync<ChatGPTImageResponse>(httpResponse, cancellationToken);
    }

    /// <inheritdoc cref="IChatGPTClient.CreateFineTuneAsync"/>
    public async Task<byte[]?> DownloadImageAsync(GeneratedImage generatedImage, CancellationToken? cancellationToken = null)
    {
        byte[]? retVal = null;

        CancellationToken cancelToken = cancellationToken is null ? CancellationToken.None : cancellationToken.Value;

        if (generatedImage is null)
        {
            throw new ArgumentNullException(nameof(generatedImage));
        }

        if (string.IsNullOrWhiteSpace(generatedImage.Url) && string.IsNullOrWhiteSpace(generatedImage.Base64))
        {
            throw new ArgumentException($"Either Url or Base64 properties must be provided", nameof(generatedImage));
        }

        if (!string.IsNullOrWhiteSpace(generatedImage.Base64))
        {
            retVal = Convert.FromBase64String(generatedImage.Base64);
        }
        else
        {
            if (!Uri.TryCreate(generatedImage.Url, UriKind.Absolute, out Uri? uri))
            {
                throw new ArgumentException($"Url is not a valid Uri", nameof(generatedImage));
            }

            // Creating the message bypasses the authentication header for this request by design. 
            // The image is public and does not require authentication. An error is generated if 
            // the Authorization header is present.
            using HttpRequestMessage requestMessage = new()
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            };

            using HttpResponseMessage? httpResponse = await _client.SendAsync(requestMessage, cancelToken).ConfigureAwait(false);

            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
#if NETSTANDARD2_1 || NETSTANDARD2_0
                    retVal = await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
#else
                    retVal = await httpResponse.Content.ReadAsByteArrayAsync(cancelToken).ConfigureAwait(false);
#endif
                }
                else
                {
                    throw new ChatGPTException($"Error downloading image {generatedImage.Url}: {httpResponse.ReasonPhrase}", httpResponse.StatusCode);
                }
            }
        }

        return retVal;
    }

    #endregion

    /// <inheritdoc cref="IChatGPTClient.CreateTranscriptionAsync(ChatGPTAudioTranscriptionRequest, bool, CancellationToken?)"/>
    public async Task<ChatGPTAudioResponse?> CreateTranscriptionAsync(ChatGPTAudioTranscriptionRequest transcriptionRequest, bool verbose = false, CancellationToken? cancellationToken = null)
    {
        MultipartFormDataContent formContent = BuildTranscriptionRequest(transcriptionRequest);

        formContent.Add(new StringContent(verbose ? "verbose_json" : "json"), "response_format");

        using (HttpRequestMessage httpReq = CreateRequestMessage(HttpMethod.Post, "audio/transcriptions"))
        {
            httpReq.Content = formContent;

            using HttpResponseMessage? httpResponse = cancellationToken is null ?
                await _client.SendAsync(httpReq).ConfigureAwait(false) :
                await _client.SendAsync(httpReq, cancellationToken.Value).ConfigureAwait(false);

            return await ProcessResponseAsync<ChatGPTAudioResponse>(httpResponse, cancellationToken);
        }
    }

    /// <inheritdoc cref="IChatGPTClient.CreateTranscriptionAsync(ChatGPTAudioTranscriptionRequest, AudioResponseFormatText, CancellationToken?)"/>
    public async Task<string?> CreateTranscriptionAsync(ChatGPTAudioTranscriptionRequest transcriptionRequest, AudioResponseFormatText textFormat = AudioResponseFormatText.Text, CancellationToken? cancellationToken = null)
    {
        MultipartFormDataContent formContent = BuildTranscriptionRequest(transcriptionRequest);

        formContent.Add(new StringContent(textFormat.GetDescriptionFromEnumValue()), "response_format");

        using (HttpRequestMessage httpReq = CreateRequestMessage(HttpMethod.Post, "audio/transcriptions"))
        {
            httpReq.Content = formContent;

            using HttpResponseMessage? httpResponse = cancellationToken is null ?
                await _client.SendAsync(httpReq).ConfigureAwait(false) :
                await _client.SendAsync(httpReq, cancellationToken.Value).ConfigureAwait(false);

            return await GetResponseStringAsync(httpResponse, cancellationToken);
        }
    }

    private static MultipartFormDataContent BuildTranscriptionRequest(ChatGPTAudioTranscriptionRequest transcriptionRequest)
    {
        if (transcriptionRequest is null)
        {
            throw new ArgumentNullException(nameof(transcriptionRequest));
        }

        if (transcriptionRequest.File is null)
        {
            throw new ArgumentNullException(nameof(transcriptionRequest), "File is required");
        }

        if (string.IsNullOrWhiteSpace(transcriptionRequest.File.FileName))
        {
            throw new ArgumentException("File.FileName is required", nameof(transcriptionRequest));
        }

        if (transcriptionRequest.File.Content is null)
        {
            throw new ArgumentException("File.Content is required", nameof(transcriptionRequest));
        }

        if (string.IsNullOrWhiteSpace(transcriptionRequest.Model))
        {
            throw new ArgumentException("Model is required", nameof(transcriptionRequest));
        }

        MultipartFormDataContent formContent = new()
        {
            { new StringContent(transcriptionRequest.Model), "model" },
            { new ByteArrayContent(transcriptionRequest.File.Content), "file", transcriptionRequest.File.FileName },
            { new StringContent(transcriptionRequest.Temperature.ToString("0.0", CultureInfo.InvariantCulture)), "temperature" }
        };

        if (!string.IsNullOrEmpty(transcriptionRequest.Language))
        {
            formContent.Add(new StringContent(transcriptionRequest.Language), "language");
        }

        if (!string.IsNullOrEmpty(transcriptionRequest.Prompt))
        {
            formContent.Add(new StringContent(transcriptionRequest.Prompt), "prompt");
        }

        return formContent;
    }

    /// <inheritdoc cref="IChatGPTClient.CreateTranslationAsync(ChatGPTAudioTranslationRequest, bool, CancellationToken?)"/>
    public async Task<ChatGPTAudioResponse?> CreateTranslationAsync(ChatGPTAudioTranslationRequest translationRequest, bool verbose = false, CancellationToken? cancellationToken = null)
    {
        MultipartFormDataContent formContent = BuildTranslationRequest(translationRequest);

        formContent.Add(new StringContent(verbose ? "verbose_json" : "json"), "response_format");

        using (HttpRequestMessage httpReq = CreateRequestMessage(HttpMethod.Post, "audio/translations"))
        {
            httpReq.Content = formContent;

            using HttpResponseMessage? httpResponse = cancellationToken is null ?
                await _client.SendAsync(httpReq).ConfigureAwait(false) :
                await _client.SendAsync(httpReq, cancellationToken.Value).ConfigureAwait(false);

            return await ProcessResponseAsync<ChatGPTAudioResponse>(httpResponse, cancellationToken);
        }
    }

    /// <inheritdoc cref="IChatGPTClient.CreateTranslationAsync(ChatGPTAudioTranslationRequest, AudioResponseFormatText, CancellationToken?)"/>
    public async Task<string?> CreateTranslationAsync(ChatGPTAudioTranslationRequest translationRequest, AudioResponseFormatText textFormat = AudioResponseFormatText.Text, CancellationToken? cancellationToken = null)
    {
        MultipartFormDataContent formContent = BuildTranslationRequest(translationRequest);

        formContent.Add(new StringContent(textFormat.GetDescriptionFromEnumValue()), "response_format");

        using (HttpRequestMessage httpReq = CreateRequestMessage(HttpMethod.Post, "audio/translations"))
        {
            httpReq.Content = formContent;

            using HttpResponseMessage? httpResponse = cancellationToken is null ?
                await _client.SendAsync(httpReq).ConfigureAwait(false) :
                await _client.SendAsync(httpReq, cancellationToken.Value).ConfigureAwait(false);

            return await GetResponseStringAsync(httpResponse, cancellationToken);
        }
    }

    private static MultipartFormDataContent BuildTranslationRequest(ChatGPTAudioTranslationRequest translationRequest)
    {
        if (translationRequest is null)
        {
            throw new ArgumentNullException(nameof(translationRequest));
        }

        if (translationRequest.File is null)
        {
            throw new ArgumentNullException(nameof(translationRequest), "File is required");
        }

        if (string.IsNullOrWhiteSpace(translationRequest.File.FileName))
        {
            throw new ArgumentException("File.FileName is required", nameof(translationRequest));
        }

        if (translationRequest.File.Content is null)
        {
            throw new ArgumentException("File.Content is required", nameof(translationRequest));
        }

        if (string.IsNullOrWhiteSpace(translationRequest.Model))
        {
            throw new ArgumentException("Model is required", nameof(translationRequest));
        }

        MultipartFormDataContent formContent = new()
        {
            { new StringContent(translationRequest.Model), "model" },
            { new ByteArrayContent(translationRequest.File.Content), "file", translationRequest.File.FileName },
            { new StringContent(translationRequest.Temperature.ToString("0.0", CultureInfo.InvariantCulture)), "temperature" }
        };

        if (!string.IsNullOrEmpty(translationRequest.Prompt))
        {
            formContent.Add(new StringContent(translationRequest.Prompt), "prompt");
        }

        return formContent;
    }


    #region Generic Request and Response Processors
    private async Task<TR?> SendRequestAsync<T, TR>(HttpMethod method, string url, T requestMessage, CancellationToken? cancellationToken)
    where T : class
    where TR : class
    {
        using HttpRequestMessage httpReq = CreateRequestMessage(method, url);

        string requestString = JsonSerializer.Serialize(requestMessage);

        httpReq.Content = new StringContent(requestString, Encoding.UTF8, "application/json");

        using HttpResponseMessage? httpResponse = cancellationToken is null ?
            await _client.SendAsync(httpReq).ConfigureAwait(false) :
            await _client.SendAsync(httpReq, cancellationToken.Value).ConfigureAwait(false);

        return await ProcessResponseAsync<TR>(httpResponse, cancellationToken).ConfigureAwait(false);
    }


    private async Task<T?> SendRequestAsync<T>(HttpMethod method, string url, CancellationToken? cancellationToken) where T : class
    {
        using HttpRequestMessage request = CreateRequestMessage(method, url);

        using HttpResponseMessage? httpResponse = cancellationToken is null ?
             await _client.SendAsync(request).ConfigureAwait(false) :
             await _client.SendAsync(request, cancellationToken.Value).ConfigureAwait(false);

        return await ProcessResponseAsync<T>(httpResponse, cancellationToken).ConfigureAwait(false);
    }

    private async static Task<T?> ProcessResponseAsync<T>(HttpResponseMessage responseMessage, CancellationToken? cancellationToken) where T : class
    {
        string responseString = await GetResponseStringAsync(responseMessage, cancellationToken).ConfigureAwait(false);

        if (responseMessage.IsSuccessStatusCode)
        {
            if (!string.IsNullOrWhiteSpace(responseString))
            {
                T? response;
                try
                {
                    response = JsonSerializer.Deserialize<T>(responseString);
                }
                catch(JsonException jsonEx)
                {
                    throw new ChatGPTException(
                        $"Error deserializing response. Possible issues with connection. Response: {responseString}",
                        responseMessage.StatusCode,
                        jsonEx);
                }

                if (response is null)
                {
                    throw new ChatGPTException("Unable to deserialize response. Response is empty.", responseMessage.StatusCode);
                }
                else
                {
                    return response;
                }
            }
        }
        else
        {
            ProcessErrorResponse(responseMessage.StatusCode, responseString);
        }

        return default(T);
    }


    private static void ProcessErrorResponse(HttpStatusCode statusCode, string response)
    {
        ChatGPTErrorResponse? errResponse;

        try
        {
            errResponse = JsonSerializer.Deserialize<ChatGPTErrorResponse>(response);
        }
        catch (JsonException jsonEx)
        {
            throw new ChatGPTException(
                $"Error deserializing error response. Possible issues with connection. Response: {response}",
                statusCode,
                jsonEx);
        }

        throw new ChatGPTException(errResponse?.Error, statusCode);

    }

    private static async Task<string> GetResponseStringAsync(HttpResponseMessage responseMessage, CancellationToken? cancellationToken)
    {
#if NETSTANDARD2_1 || NETSTANDARD2_0
        string responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
#else
        string responseString = cancellationToken is null ?
                await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false) :
                await responseMessage.Content.ReadAsStringAsync(cancellationToken.Value).ConfigureAwait(false);
#endif

        return responseString;
    }

    private HttpRequestMessage CreateRequestMessage(HttpMethod method, string url)
    {
        HttpRequestMessage request = new(method, url);

        if (_chatCredentials is null)
        {
            throw new ChatGPTConfigurationException("ChatGPTCredentials are null.");
        }

        if (string.IsNullOrWhiteSpace(_chatCredentials.ApiKey))
        {
            throw new ChatGPTConfigurationException("ApiKey property cannot be null or whitespace.");
        }

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _chatCredentials.ApiKey);

        if (!string.IsNullOrWhiteSpace(_chatCredentials.Organization))
        {
            request.Headers.Add("OpenAI-Organization", _chatCredentials.Organization);
        }

        return request;
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
