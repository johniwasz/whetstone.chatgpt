using Blazored.LocalStorage;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using Whetstone.ChatGPT.Blazor.App.State;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Blazor.App
{
    public sealed class OpenAICredentialValidator : IOpenAICredentialValidator
    {
        private IChatGPTClient _chatClient;
        private ILocalStorageService _localStorage;
        private ILogger<OpenAICredentialValidator> _logger;

        private const string CREDSTORE = "openaicredentials";

        private const string REMEMBERSELECT = "remembercrendentials";

        public OpenAICredentialValidator(IChatGPTClient chatClient, ILocalStorageService localStorage, ILogger<OpenAICredentialValidator> logger)
        {
            _chatClient = chatClient ?? throw new ArgumentNullException(nameof(chatClient));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        public async Task<bool> GetStoreCredentialsLocalOption()
        {
            return await GetStoredValueAsync<bool>(REMEMBERSELECT);
        }

        public async Task<ChatGPTCredentials> GetCredentialsAsync()
        {
            return await GetStoredValueAsync<ChatGPTCredentials>(CREDSTORE);
        }


        public async Task<bool> ValidateCredentialsAsync(ChatGPTCredentials? credentials, ApplicationState? appState, bool storeLocal)
        {
            if(credentials is null)
            {
                throw new ArgumentNullException(nameof(credentials));
            }
            
            if(appState is null)
            {
                throw new ArgumentNullException(nameof(appState));
            }

            bool isValid = false;

            appState.IsOpenAIAuthenticated = false;

            _chatClient.Credentials = credentials;

            await _chatClient.ListFineTunesAsync();

            isValid = true;

            appState.IsOpenAIAuthenticated = isValid;

            if(storeLocal)
            {
                await _localStorage.SetItemAsync(CREDSTORE, credentials);
                await _localStorage.SetItemAsync(REMEMBERSELECT, storeLocal);
            }
            return isValid;
        }

        public async Task<bool> ValidateStoredCredentialsAsync(ApplicationState? appState)
        {
            if (appState is null)
            {
                throw new ArgumentNullException(nameof(appState));
            }

            bool isValid = false;

            ChatGPTCredentials credentials = await GetStoredValueAsync<ChatGPTCredentials>(CREDSTORE);

            appState.IsOpenAIAuthenticated = false;

            if (credentials is null)
            {
                _chatClient.Credentials = null;
                _logger.LogInformation("Credentials not found in local storage {StoreName}", CREDSTORE);
            }
            else
            {
                _chatClient.Credentials = credentials;
                try
                {
                    await _chatClient.ListFineTunesAsync();
                    isValid = true;
                    appState.IsOpenAIAuthenticated = isValid;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Error validating credentials in local storage at {StoreName}: {Exception}", CREDSTORE, ex);
                }
            }

            return isValid;
        }

        public async Task PurgeCredentialsAsync(ApplicationState? appState)
        {
            if (appState is null)
            {
                throw new ArgumentNullException(nameof(appState));
            }

            appState.IsOpenAIAuthenticated = false;
            await _localStorage.RemoveItemAsync(CREDSTORE);
            _chatClient.Credentials = null;

        }

        private async Task<T> GetStoredValueAsync<T>(string storeName)
        {
            T retVal = default!;
            try
            {
                retVal = await _localStorage.GetItemAsync<T>(storeName);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error getting {StoreName}: {Exception}", storeName, ex);
            }

            return retVal;
        }
    }
}
