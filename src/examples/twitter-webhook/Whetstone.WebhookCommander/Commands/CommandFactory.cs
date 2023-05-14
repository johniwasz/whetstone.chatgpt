// SPDX-License-Identifier: MIT
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whetstone.TweetGPT.WebHookManager;

namespace Whetstone.WebhookCommander.Commands;

internal class CommandFactory : ICommandFactory
{
    private readonly IConfigurationRoot _config;
    private readonly ILogger<ICommandFactory> _logger;

    private readonly IWebhookManager _webhookManager;

    public CommandFactory(IWebhookManager manger, IConfigurationRoot config, ILogger<ICommandFactory> logger)
    {
        _webhookManager = manger ?? throw new ArgumentNullException(nameof(manger));
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public ICommand GetCommand<T>() where T : ICommand?
    {
        ICommand? command = (ICommand?) Activator.CreateInstance(typeof(T), _webhookManager, _config, _logger);

        if (command is null)
        {
            throw new InvalidOperationException("Unable to create command");
        }

        return command;
    }
}
