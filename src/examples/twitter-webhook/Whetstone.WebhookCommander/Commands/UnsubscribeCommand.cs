using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Whetstone.TweetGPT.WebHookManager;


namespace Whetstone.WebhookCommander.Commands;

internal class UnsubscribeCommand : Command, ICommand
{
    public UnsubscribeCommand(IWebhookManager manager, IConfigurationRoot config, ILogger logger) : base(manager, config, logger)
    {
    }

    public async Task ExecuteAsync()
    {
        bool isValid = true;

        long userIdVal = 0;

        if (!TryGetSwitchValue(CommandSwitches.ENVIRONMENT, out string? environmentValue))
        {
            Logger.LogError("Environment is required");
            isValid = false;
        }

        if (TryGetSwitchValue(CommandSwitches.USERID, out string? userId))
        {
            if (!long.TryParse(userId, out userIdVal))
            {
                Logger.LogError("UserId must be a valid long");
                isValid = false;
            }

        }
        else 
        {
            Logger.LogError("User id is required");
            isValid = false;
        }

      
        if (isValid)
        {
            try
            {
                await WebhookManager.UnsubscribeAsync(environmentValue, userIdVal);

                Logger.LogInformation("Unsubscribed");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error registering webhook", ex);
            }

        }
    }
}