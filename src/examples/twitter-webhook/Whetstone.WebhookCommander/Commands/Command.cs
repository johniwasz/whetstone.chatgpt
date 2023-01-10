using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;
using Whetstone.TweetGPT.WebHookManager;

namespace Whetstone.WebhookCommander.Commands;

internal abstract class Command
{
    internal enum WebhookCommandEnum
    {
        AddWebhook = 0,
        RemoveWebhook = 1,
        List = 2,
        Subscribe = 3,
        Unsubscribe = 4
    }

    internal enum WebhookListCommandEnum
    {
        Webhooks = 1,
        Subscriptions = 2,
        Environments = 3
    }

    private readonly IConfigurationRoot _config;

    private readonly ILogger _logger;

    private readonly IWebhookManager _webhookManager;

    internal Command(IWebhookManager manager, IConfigurationRoot config, ILogger logger)
    {
        _webhookManager = manager ?? throw new ArgumentNullException(nameof(manager));
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected ILogger Logger => _logger;

    protected IWebhookManager WebhookManager => _webhookManager;


    protected bool TryGetSwitchValue(string[] switchNames, out string? switchValue)
    {
        if (switchNames is null)
            throw new ArgumentNullException(nameof(switchNames));

        if (switchNames.Length == 0)
            throw new ArgumentException($"{nameof(switchNames)} cannot be an empty array");

        bool isFound = false;

        int index = 0;

        switchValue = default;

        while (!isFound && index < switchNames.Length)
        {
            string? configValue = _config[switchNames[index]];

            if (!string.IsNullOrWhiteSpace(configValue))
            {
                switchValue = configValue;
                isFound = true;
            }
            else
            {
                index++;
            }
        }

        return isFound;
    }

}
