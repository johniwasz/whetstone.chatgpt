// SPDX-License-Identifier: MIT

// See https://aka.ms/new-console-template for more information


using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Tweetinvi.Client.Requesters;
using Whetstone.TweetGPT.WebHookManager;
using Whetstone.TweetGPT.WebHookManager.Models;
using Whetstone.WebhookCommander;
using Whetstone.WebhookCommander.Commands;
using static Whetstone.WebhookCommander.Commands.Command;


if(args.Length ==0)
{
    Console.WriteLine("help placeholder");
    Environment.Exit(0);
}

string command = args[0].ToLowerInvariant();



var builder = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddUserSecrets("117cec77-f121-4f75-9054-584941f6df04")
    .AddCommandLine(args);

var config = builder.Build();

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});


// bin\Debug\net6.0\twithookconfig addwebhook --e devchatgpt --webhookurl https://49fe-108-2-209-91.ngrok.io/api/chatgptdm

var serviceBuilder = new ServiceCollection()
    .Configure<WebhookCredentials>(options =>
        {
            options.AccessToken = config["WebhookCredentials:AccessToken"];

            options.AccessTokenSecret = config["WebhookCredentials:AccessTokenSecret"];

            options.ConsumerSecret = config["WebhookCredentials:ConsumerSecret"];

            options.ConsumerKey = config["WebhookCredentials:ConsumerKey"];
        })

    .AddLogging(configure => {
        configure.SetMinimumLevel(LogLevel.Trace);
        configure.AddConsole();
    })
    .AddSingleton<ICommandFactory, CommandFactory>()
    .AddSingleton<IConfigurationRoot>(config)
    .AddSingleton<IWebhookManager, WebhookManager>();




var services = serviceBuilder.BuildServiceProvider();

if (Enum.TryParse<WebhookCommandEnum>(command, true, out WebhookCommandEnum toolCommand))
{    
    try
    {
        ICommandFactory commandFactory = services.GetRequiredService<ICommandFactory>();

        switch (toolCommand)
        {
            case WebhookCommandEnum.AddWebhook:
                ICommand addWebHookCommand = commandFactory.GetCommand<AddWebhookCommand>();

                

                await addWebHookCommand.ExecuteAsync().ConfigureAwait(false);

                break;
            case WebhookCommandEnum.RemoveWebhook:
                ICommand removeWebHookCommand = commandFactory.GetCommand<RemoveWebhookCommand>();

                await removeWebHookCommand.ExecuteAsync().ConfigureAwait(false);

                break;

            case WebhookCommandEnum.List:

                if (args.Length > 1)
                {
                    string listArg = args[1].ToLowerInvariant();


                    if (Enum.TryParse<WebhookListCommandEnum>(listArg, true, out WebhookListCommandEnum listCommand))
                    {
                        switch (listCommand)
                        {
                            case WebhookListCommandEnum.Subscriptions:
                                ICommand listSubscriptionsCommand = commandFactory.GetCommand<ListSubscriptionsCommand>();

                                await listSubscriptionsCommand.ExecuteAsync().ConfigureAwait(false);
                                break;
                            case WebhookListCommandEnum.Environments:
                                ICommand listEnvironmentsCommand = commandFactory.GetCommand<ListEnvironmentsCommand>();

                                await listEnvironmentsCommand.ExecuteAsync().ConfigureAwait(false);
                                break;
                            case WebhookListCommandEnum.Webhooks:
                                ICommand listWebhooksCommand = commandFactory.GetCommand<ListWebHooksCommand>();

                                await listWebhooksCommand.ExecuteAsync().ConfigureAwait(false);
                                break;
                            default:
                                throw new NotImplementedException();
                        }


                    }
                    else
                    {
                        Console.WriteLine("Invalid command");
                    }
                }
                else
                {
                    Console.WriteLine("List command requires a subcommand");
                }

                break;
            case WebhookCommandEnum.Subscribe:
                ICommand subscribeCommand = commandFactory.GetCommand<SubscribeCommand>();

                await subscribeCommand.ExecuteAsync().ConfigureAwait(false);
                break;

            case WebhookCommandEnum.Unsubscribe:
                ICommand unsubscribeCommand = commandFactory.GetCommand<UnsubscribeCommand>();

                await unsubscribeCommand.ExecuteAsync().ConfigureAwait(false);
                break;
        }
        

    }
    catch(Exception ex)
    {
        Console.WriteLine(ex);
    }
}




/*
WebhookCredentials GetWebhookCredentials(IConfigurationRoot config)
{
    string? consumerKey = config[EnvironmentSettings.ENV_TWITTER_CONSUMER_KEY];
    string? consumerSecret = config[EnvironmentSettings.ENV_TWITTER_CONSUMER_SECRET];
    string? accessToken = config[EnvironmentSettings.ENV_TWITTER_ACCESS_TOKEN];
    string? accessTokenSecret = config[EnvironmentSettings.ENV_TWITTER_ACCESS_TOKEN_SECRET];

    List<Exception> credExceptions = new List<Exception>();    

    if (string.IsNullOrWhiteSpace(consumerKey))
    {
        credExceptions.Add(new WebhookCommanderException("ConsumerKey not provided"));
    }

    if (string.IsNullOrWhiteSpace(consumerSecret))
    {
        credExceptions.Add(new WebhookCommanderException("ConsumerSecret not provided"));
    }

    if (string.IsNullOrWhiteSpace(accessToken))
    {
        credExceptions.Add(new WebhookCommanderException("AccessToken not provided"));
    }

    if (string.IsNullOrWhiteSpace(accessTokenSecret))
    {
        credExceptions.Add(new WebhookCommanderException("AccessTokenSecret not provided"));
    }

    if (credExceptions.Count > 0)
    {
        throw new AggregateException(credExceptions);
    }

    return new WebhookCredentials(accessToken, accessTokenSecret, consumerKey, consumerSecret);  
}

*/