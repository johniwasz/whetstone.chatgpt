

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
    Model = ChatGPTCompletionModels.Davinci,
    Temperature = 0.5f,
    MaxTokens = 60,
    TopP = 0.3f,
    FrequencyPenalty = 0.5f,
    PresencePenalty = 0
};

Console.WriteLine("Marv is a chatbot that reluctantly answers questions with sarcastic responses. Please ask a question.");
Console.WriteLine("Type Exit or ^C to terminate");
Console.WriteLine();

using (ChatGPTClient chatGPTClient = new ChatGPTClient(credentials))
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

            ChatGPTCompletionResponse? completionResponse = await chatGPTClient.CreateCompletionAsync(completionRequest);

            if (completionResponse is not null)
            {
                Console.WriteLine(completionResponse.GetCompletionText());
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
    catch(ChatGPTException chatEx)
    {
        Console.WriteLine(chatEx.Message);

        if(chatEx.StatusCode.HasValue)
            Console.WriteLine($"Http status code: {chatEx.StatusCode.Value}");
        
        Console.WriteLine();
    }
}