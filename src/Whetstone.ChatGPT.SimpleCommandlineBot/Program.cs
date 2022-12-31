

using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;


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

StringBuilder promptBuilder = new();
promptBuilder.AppendLine("Marv is a chatbot that reluctantly answers questions with sarcastic responses:");
promptBuilder.AppendLine();
promptBuilder.AppendLine("You: How many pounds are in a kilogram?");
promptBuilder.AppendLine("Marv: This again? There are 2.2 pounds in a kilogram. Please make a note of this.");
promptBuilder.AppendLine("You: What does HTML stand for?");
promptBuilder.AppendLine("Marv: Was Google too busy? Hypertext Markup Language. The T is for try to ask better questions in the future.");
promptBuilder.AppendLine("You: When did the first airplane fly?");
promptBuilder.AppendLine("Marv: On December 17, 1903, Wilbur and Orville Wright made the first flights. I wish they’dve come and take me away.");

string baselinePrompt = promptBuilder.ToString();

CompletionRequest completionRequest = new CompletionRequest
{
    Model = "text-davinci-003",
    //Temperature = 0.5f,
    MaxTokens = 120,
    //TopP = 0.3f,
    //FrequencyPenalty = 0.5f,
    //PresencePenalty = 0
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

            // string userInput = $"You: {userResponse}\nMarv: ";

            //string userPrompt = string.Concat(baselinePrompt, userInput);

            // completionRequest.Prompt = userPrompt;

            completionRequest.Prompt = userResponse;

            Console.WriteLine();

            // CompletionResponse? completionResponse = await httpClient.CreateCompletionAsync(completionRequest);

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
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            string responseString = await httpResponse.Content.ReadAsStringAsync();
                            {
                                if (!string.IsNullOrWhiteSpace(responseString))
                                {
                                    completionResponse = JsonSerializer.Deserialize<CompletionResponse>(responseString);

                                }
                            }
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