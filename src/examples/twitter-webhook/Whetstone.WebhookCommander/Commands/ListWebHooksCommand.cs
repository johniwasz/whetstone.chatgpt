using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tweetinvi.Core.Models;
using Whetstone.TweetGPT.WebHookManager;

namespace Whetstone.WebhookCommander.Commands;

internal class ListWebHooksCommand : Command, ICommand
{
    public ListWebHooksCommand(IWebhookManager manager, IConfigurationRoot config, ILogger logger) : base(manager, config, logger)
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
#pragma warning disable CS8604 // Possible null reference argument.
                var webHookResponses = await WebhookManager.GetWebHooksAsync(environmentValue);
#pragma warning restore CS8604 // Possible null reference argument.

                if (webHookResponses is not null)
                {

                    if (webHookResponses.Any())
                    {
                        foreach (var webHook in webHookResponses)
                        {
                            Logger.LogInformation($"{webHook.Id}: {webHook.Url} --- {webHook.Valid}");
                        }
                    }
                    else
                    {
                        Logger.LogInformation("No webhooks found");
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
