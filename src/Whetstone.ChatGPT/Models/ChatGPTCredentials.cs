using System.Text.Json.Serialization;

#if NET6_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
#endif

namespace Whetstone.ChatGPT.Models
{

    /// <summary>
    /// Supplies the GPT-3 API key and organization of the user.
    /// </summary>
    /// <remarks>
    /// Organization is only required if the user belongs to more than one organization. See <see href="https://beta.openai.com/docs/api-reference/authentication">https://beta.openai.com/docs/api-reference/authentication</see> for more information.
    /// </remarks>
    public class ChatGPTCredentials
    {
        /// <summary>
        /// Creates a new instance of <see cref="ChatGPTCredentials"/>. This is incuded to support serialization.
        /// </summary>
        /// <remarks>
        /// For explicit instantions use <see cref="ChatGPTCredentials(String)">ChatGPTCredentials(string apiKey)</see> or <see cref="ChatGPTCredentials(String, String)">ChatGPTCredentials(string apiKey, string? organization)</see>
        /// </remarks>
        public ChatGPTCredentials()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ChatGPTCredentials"/>.
        /// </summary>
        /// <param name="apiKey">The OpenAI API uses API keys for authentication. Visit your <see href="https://beta.openai.com/account/api-keys">API Keys</see> page to retrieve the API key you'll use in your requests./param>
        public ChatGPTCredentials(string apiKey)
        {
            ApiKey = apiKey;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ChatGPTCredentials"/>.
        /// </summary>
        /// <param name="apiKey">The OpenAI API uses API keys for authentication. Visit your <see href="https://beta.openai.com/account/api-keys">API Keys</see> page to retrieve the API key you'll use in your requests./param>
        /// <param name="organization">For users who belong to multiple organizations, you can pass a header to specify which organization is used for an API request. Usage from these API requests will count against the specified organization's subscription quota.</param>
        public ChatGPTCredentials(string apiKey, string? organization)
        {
            ApiKey = apiKey;
            Organization = organization;
        }

        /// <summary>
        /// The OpenAI API uses API keys for authentication. Visit your <see href="https://beta.openai.com/account/api-keys">API Keys</see> page to retrieve the API key you'll use in your requests.
        /// </summary>
        /// <remarks>Required for authentication.</remarks>
#if NET6_0_OR_GREATER
        [Required]
        [DataType(DataType.Password)]
#endif
        [JsonPropertyName("apiKey")]
        public string? ApiKey
        {
            get;
            set;
        }

        /// <summary>
        /// For users who belong to multiple organizations, you can pass a header to specify which organization is used for an API request. Usage from these API requests will count against the specified organization's subscription quota.
        /// </summary>
        [JsonPropertyName("organization")]
        public string? Organization
        {
            get;
            set;
        }

    }
}
