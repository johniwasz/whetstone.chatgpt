// See https://aka.ms/new-console-template for more information


using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Whetstone.TweetGPT.WebHookManager;


namespace Whetstone.WebhookCommander.Commands;

internal class SubscribeCommand : Command, ICommand
{
    public SubscribeCommand(IWebhookManager manager, IConfigurationRoot config, ILogger logger) : base(manager, config, logger)
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
                await WebhookManager.SubscribeAsync(environmentValue);
#pragma warning restore CS8604 // Possible null reference argument.

                Logger.LogInformation("Subscribed");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error registering webhook", ex);
            }

        }
    }
}