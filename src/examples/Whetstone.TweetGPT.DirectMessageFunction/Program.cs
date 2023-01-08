using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Whetstone.TweetGPT.WebHookManager;
using Whetstone.TweetGPT.WebHookManager.Models;
using Tweetinvi;
using Tweetinvi.AspNet;
using Tweetinvi.Models;
using System.Text.Json;
using Whetstone.ChatGPT;
using Whetstone.ChatGPT.Models;
using Polly.Extensions.Http;
using Polly;

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

                services.Configure<ChatGPTCredentials>(options =>
                {
                    IConfiguration config = context.Configuration;
                    
                    string? gptCredentialsText = config["OPENAI_API_CREDS"];

                    if (string.IsNullOrWhiteSpace(gptCredentialsText))
                    {
                        throw new InvalidOperationException("OPENAI_API_CREDS is not set.");
                    }
                    
                    ChatGPTCredentials? gptCredentials = JsonSerializer.Deserialize<ChatGPTCredentials>(gptCredentialsText);

                    if (gptCredentials == null)
                    {
                        throw new InvalidOperationException("OPENAI_API_CREDS is not set.");
                    }
                    
                    options.ApiKey = gptCredentials.ApiKey;
                    options.Organization = gptCredentials.Organization;

                });


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


                /// services.AddHttpClient();

                
                services.AddHttpClient<IChatGPTClient, ChatGPTClient>()
                    .SetHandlerLifetime(TimeSpan.FromSeconds(150))
                    .AddPolicyHandler(GetRetryPolicy());
                

                services.AddLogging();

                //services.AddScoped<IChatGPTClient, ChatGPTClient>();
            });

        var host = hostBuilder.Build();

        host.Run();
    }


    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
