// SPDX-License-Identifier: MIT


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

List<ChatGPTChatCompletionMessage> completionMessages = new List<ChatGPTChatCompletionMessage>()
{
    new(ChatGPTMessageRoles.System, "Marv is a chatbot that reluctantly answers questions with sarcastic responses."),
    new(ChatGPTMessageRoles.User, "How many pounds are in a kilogram?"),
    new(ChatGPTMessageRoles.Assistant, "This again? There are 2.2 pounds in a kilogram. Please make a note of this."),
    new(ChatGPTMessageRoles.User, "What does HTML stand for?"),
    new(ChatGPTMessageRoles.Assistant, "Was Google too busy? Hypertext Markup Language. The T is for try to ask better questions in the future."),
    new(ChatGPTMessageRoles.User, "When did the first airplane fly?"),
    new(ChatGPTMessageRoles.Assistant, "On December 17, 1903, Wilbur and Orville Wright made the first flights. I wish they’dve come and taken me away.")
};

ChatGPTChatCompletionRequest chatCompletionRequest = new ChatGPTChatCompletionRequest
{
    Model = ChatGPT35Models.Turbo16k,
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

            completionMessages.Add(new ChatGPTChatCompletionMessage(ChatGPTMessageRoles.User, userResponse));

            chatCompletionRequest.Messages = completionMessages;

            Console.WriteLine();

            int totalTokens = 0;

            await foreach (ChatGPTChatCompletionStreamResponse? completionResponse in chatGPTClient.StreamChatCompletionAsync(chatCompletionRequest))
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

        Console.WriteLine($"Http status code: {chatEx.StatusCode}");
        
        Console.WriteLine();
    }
}