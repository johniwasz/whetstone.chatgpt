using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Whetstone.TweetGPT.WebHookManager;
using Whetstone.TweetGPT.WebHookManager.Models;
using Tweetinvi;
using Tweetinvi.AspNet;
using Tweetinvi.Models;
using System.Text.Json;
using Grpc.Core;

public class Program
{
    public static IAccountActivityRequestHandler? AccountActivityRequestHandler { get; set; }
    public static ITwitterClient? WebhookClient { get; set; }

    public static void Main(string[] args)
    {

        Plugins.Add<AspNetPlugin>();

        var hostBuilder = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureAppConfiguration((hostingContext, configuration) =>
            {
                // Do NOT clear prior configurations.
                // configuration.Sources.Clear();
#if DEBUG
                configuration.AddUserSecrets("117cec77-f121-4f75-9054-584941f6df04");
#endif
                configuration.AddEnvironmentVariables();
                
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<WebhookCredentials>(options =>
                    {
                        IConfiguration config = context.Configuration;
                        string? twitterCredString = config["TWITTER_CREDS"];

                        WebhookCredentials? twitterCreds = string.IsNullOrWhiteSpace(twitterCredString) ? 
                            null :  JsonSerializer.Deserialize<WebhookCredentials>(twitterCredString);

                        if (twitterCreds is not null)
                        {
                            options.AccessToken = twitterCreds.AccessToken;
                            options.AccessTokenSecret = twitterCreds.AccessTokenSecret;
                            options.ConsumerSecret = twitterCreds.ConsumerSecret;
                            options.ConsumerKey = twitterCreds.ConsumerKey;
                        }

#if DEBUG
                        if (twitterCreds is null)
                        {
                            options.AccessToken = config["WebhookCredentials:AccessToken"];
                            options.AccessTokenSecret = config["WebhookCredentials:AccessTokenSecret"];
                            options.ConsumerSecret = config["WebhookCredentials:ConsumerSecret"];
                            options.ConsumerKey = config["WebhookCredentials:ConsumerKey"];
                        }
#endif
                    });              
            });        

        var host = hostBuilder.Build();

        host.Run();
    }

}
