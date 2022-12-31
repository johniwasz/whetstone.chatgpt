using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Whetstone.TweetGPT.WebHookManager;

namespace Whetstone.WebhookCommander.Commands
{
    internal class AddWebhookCommand : Command, ICommand
    {
        public AddWebhookCommand(IWebhookManager manager, IConfigurationRoot config, ILogger logger) : base(manager, config, logger)
        {
        }

        public async Task ExecuteAsync()
        {
            bool isValid = true;
            Uri? webhookUri = default;

            if (!TryGetSwitchValue(CommandSwitches.WEBHOOK, out string? webhookValue))
            {
                Logger.LogError("Webhook URL is required");
                isValid = false;
            }
            else
            {
                if (!Uri.TryCreate(webhookValue, UriKind.Absolute, out Uri? webhookUriOut))
                {
                    Logger.LogError("webhook is not an absolute URI");
                    isValid = false;
                }
                else
                {
                    webhookUri = webhookUriOut;
                }
            }

            if (!TryGetSwitchValue(CommandSwitches.ENVIRONMENT, out string? environmentValue))
            {
                Logger.LogError("Environment is required");
                isValid = false;
            }

            if (isValid)
            {

                try
                {
                    var webHookResponse = await WebhookManager.RegisterWebhookAsync(environmentValue, webhookUri);

                    if (webHookResponse is not null)
                    {
                        Logger.LogInformation($"Webhook Id: {webHookResponse.Id}");
                        Logger.LogInformation($"Webhook Url: {webHookResponse.Url}");
                        Logger.LogInformation($"Webhook Valid: {webHookResponse.Valid}");
                    }

                }
                catch(WebhookManagerException manEx)
                {
                    Logger.LogError("Error registering webhook", manEx);
                }

                catch (Exception ex)
                {
                    Logger.LogError("Error registering webhook", ex);
                }

            }

        }

    }

}
