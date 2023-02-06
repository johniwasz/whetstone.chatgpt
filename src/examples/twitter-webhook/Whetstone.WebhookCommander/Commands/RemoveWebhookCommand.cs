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
    internal class RemoveWebhookCommand : Command, ICommand
    {
        public RemoveWebhookCommand(IWebhookManager webhookManager, IConfigurationRoot config, ILogger logger) : base(webhookManager, config, logger)
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

            if (!TryGetSwitchValue(CommandSwitches.WEBHOOKID, out string? webhookId))
            {
                Logger.LogError("Webhook URL is required");
                isValid = false;
            }


            if (isValid)
            {
                try
                {
                    await WebhookManager.RemoveWebhookAsync(environmentValue, webhookId).ConfigureAwait(false);

                    Logger.LogInformation("Webhook removed");
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error registering webhook", ex);
                }

            }


        }
    }
}
