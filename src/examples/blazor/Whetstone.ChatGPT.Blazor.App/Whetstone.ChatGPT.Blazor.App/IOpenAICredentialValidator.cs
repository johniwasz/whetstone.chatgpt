using System.Diagnostics.CodeAnalysis;
using Whetstone.ChatGPT.Blazor.App.State;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Blazor.App
{
    internal interface IOpenAICredentialValidator
    {   
        Task<bool> ValidateCredentialsAsync(ChatGPTCredentials? credentials, ApplicationState? appState, bool storeLocal);

        Task<bool> ValidateStoredCredentialsAsync(ApplicationState? appState);

        Task PurgeCredentialsAsync(ApplicationState? appState);

        Task<bool> GetStoreCredentialsLocalOption();

        Task<ChatGPTCredentials> GetCredentialsAsync();
    }
}