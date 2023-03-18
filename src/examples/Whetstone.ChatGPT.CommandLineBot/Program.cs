

using Microsoft.Extensions.Configuration;
using System.Text;
using Whetstone.ChatGPT.CommandLineBot;

var builder = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddCommandLine(args);

IConfigurationRoot configuration = builder.Build();


ChatGPTCredentials? credentials = CliUtilities.GetChatGPTCredentials(configuration);


if (credentials is null)
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

ChatGPTCompletionRequest completionRequest = new ChatGPTCompletionRequest
{
    Model = ChatGPT35Models.Davinci003,
    Temperature = 1.0f,
    MaxTokens = 500,
    TopP = 0.3f,
    FrequencyPenalty = 0.5f,
    PresencePenalty = 0
};

Console.WriteLine("Marv is a chatbot that reluctantly answers questions with sarcastic responses. Please ask a question.");
Console.WriteLine("Type Exit or ^C to terminate");
Console.WriteLine();

using (ChatGPTClient chatGPTClient = new(credentials))
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

            string userInput = $"You: {userResponse}\nMarv: ";

            string userPrompt = string.Concat(baselinePrompt, userInput);

            completionRequest.Prompt = userPrompt;

            Console.WriteLine();

            int totalTokens = 0;

            await foreach (ChatGPTCompletionStreamResponse? completionResponse in chatGPTClient.StreamCompletionAsync(completionRequest))
            {
                if (completionResponse is not null)
                {
                    Console.Write(completionResponse.GetCompletionText());
                }
            }
            
            Console.WriteLine();

            // Tokens used is reported as 0 when streaming.
            // Console.ForegroundColor = ConsoleColor.Yellow;
            // Console.Write("Tokens Used: ");
            /// Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine(totalTokens);

            Console.WriteLine();

            Console.Write("> ");

            userResponse = Console.ReadLine();

        }
    }
    catch(ChatGPTException chatEx)
    {
        Console.WriteLine(chatEx.Message);

        if(chatEx.StatusCode.HasValue)
            Console.WriteLine($"Http status code: {chatEx.StatusCode.Value}");
        
        Console.WriteLine();
    }
}