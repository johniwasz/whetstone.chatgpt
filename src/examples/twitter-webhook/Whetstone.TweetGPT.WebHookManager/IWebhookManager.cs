// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;

namespace Whetstone.TweetGPT.WebHookManager
{
    public interface IWebhookManager
    {
        Task<IEnumerable<IWebhook>?> GetWebHooksAsync(string environment);

        Task<IWebhookDTO?> RegisterWebhookAsync(string? environment, Uri? webhookUri);

        Task RemoveWebhookAsync(string? environment, string? webhookId);

        Task<IWebhookEnvironmentSubscriptionsDTO> GetSubscriptionsAsync(string environment);

        Task<IEnumerable<IWebhookEnvironment>> GetEnvironmentsAsync();

        Task SubscribeAsync(string environment);

        Task UnsubscribeAsync(string? environment, long userId);

        Task ResendWebhookValidationAsync(string environment, string webhookId);
    }
}
