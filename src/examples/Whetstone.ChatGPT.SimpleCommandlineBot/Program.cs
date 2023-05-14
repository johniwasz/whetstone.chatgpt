// SPDX-License-Identifier: MIT


using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Text.Json;


var builder = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddCommandLine(args);

IConfigurationRoot configuration = builder.Build();


string? apiKey = CliUtilities.GetChatGPTAPIKey(configuration);


if (string.IsNullOrEmpty(apiKey))
{
    Console.WriteLine(CliUtilities.GetUsage());
    Environment.Exit(0);
}

CompletionRequest completionRequest = new CompletionRequest
{
    Model = "text-davinci-003",
    Temperature = 0.5f,
    MaxTokens = 120,
    TopP = 0.3f,
    FrequencyPenalty = 0.5f,
    PresencePenalty = 0
};

Console.WriteLine("Marv is a chatbot that reluctantly answers questions with sarcastic responses. Please ask a question.");
Console.WriteLine("Type Exit or ^C to terminate");
Console.WriteLine();

using (HttpClient httpClient = new HttpClient())
{
    Console.Write("> ");
    string? userResponse = Console.ReadLine();

    try
    {
        while (!CliUtilities.IsExitInput(userResponse))
        {
            if (string.IsNullOrWhiteSpace(userResponse))
            {
                Console.WriteLine("Use Exit or ^C to terminate");
                Console.WriteLine();
                break;
            }
            completionRequest.Prompt = userResponse;

            Console.WriteLine();

            CompletionResponse? completionResponse = null;


            using (var httpReq = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/completions"))
            {

                httpReq.Headers.Add("Authorization", $"Bearer {apiKey}");

                string requestString = JsonSerializer.Serialize(completionRequest);

                httpReq.Content = new StringContent(requestString, Encoding.UTF8, "application/json");

                using (HttpResponseMessage? httpResponse = await httpClient.SendAsync(httpReq))
                {
                    if (httpResponse is not null)
                    {
                        string responseString = await httpResponse.Content.ReadAsStringAsync();
                        if (httpResponse.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(responseString))
                        {
                            completionResponse = JsonSerializer.Deserialize<CompletionResponse>(responseString);
                        }
                    }
                }
            }


            if (completionResponse is not null)
            {
                string? completionText = completionResponse.Choices?[0]?.Text;

                Console.WriteLine(completionText);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Tokens Used: ");
                Console.ForegroundColor = ConsoleColor.White;
                
                Console.WriteLine(completionResponse.Usage?.TotalTokens);
                
                Console.WriteLine();

                Console.Write("> ");

                userResponse = Console.ReadLine();
            }
        }
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.Message);
 
        Console.WriteLine();
    }
}