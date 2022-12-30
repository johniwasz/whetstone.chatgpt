using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whetstone.TweetGPT.WebHookManager;

namespace Whetstone.WebhookCommander.Commands;

internal class ListEnvironmentsCommand : Command, ICommand
{

    public ListEnvironmentsCommand(IWebhookManager manager, IConfigurationRoot config, ILogger logger) : base(manager, config, logger)
    {
    }

    public async Task ExecuteAsync()
    {
        try
        {
            var environments = await WebhookManager.GetEnvironmentsAsync();

            foreach (var retEnv in environments)
            {                    
                Logger.LogInformation(retEnv.Name);
                foreach (var retHook in retEnv.Webhooks)
                {
                    Logger.LogInformation($"   {retHook.Id}: {retHook.Url} --- {retHook.Valid}");
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error listing environments");
        }
    }

}
