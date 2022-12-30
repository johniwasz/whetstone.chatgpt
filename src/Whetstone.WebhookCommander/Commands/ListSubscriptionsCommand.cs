using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whetstone.TweetGPT.WebHookManager;

namespace Whetstone.WebhookCommander.Commands;

internal class ListSubscriptionsCommand : Command, ICommand
{
    public ListSubscriptionsCommand(IWebhookManager manager, IConfigurationRoot config, ILogger logger) : base(manager, config, logger)
    {
    }

    public async Task ExecuteAsync()
    {
        bool isValid = true;

        if (!TryGetSwitchValue(CommandSwitches.ENVIRONMENT, out string? environmentValue))
        {
            Logger.LogError("Environment is required");
            isValid = false;
        }

        if (isValid)
        {
            try
            {
                var webHookResponse = await WebhookManager.GetSubscriptionsAsync(environmentValue);

                Logger.LogInformation($"ApplicationId:  {webHookResponse.ApplicationId}  Environment: {webHookResponse.Environment}");

                if (webHookResponse.Subscriptions.Length == 0)
                {
                    Logger.LogInformation("No subscriptions found");
                }
                else
                {
                    foreach (var reSubs in webHookResponse.Subscriptions)
                    {
                        Logger.LogInformation($"UserId: {reSubs.UserId}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error registering webhook", ex);
            }

        }

    }
}
