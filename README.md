[![CodeQL](https://github.com/johniwasz/whetstone.chatgpt/actions/workflows/codeql.yml/badge.svg)](https://github.com/johniwasz/whetstone.chatgpt/actions/workflows/codeql.yml)

# Whetstone.ChatGPT

A simple light-weight library that wraps the Open AI API with support for dependency injection.

Supported features include:

- GPT 4, GPT 3.5 Turbo and Chat Completions
- Audio Transcription and Translation (Whisper API)
- Chat Completions
- Vision Completions
- Files
- Fine Tunes
- Images
- Embeddings
- Moderations
- Response streaming

For a video walkthrough of a Blazor web app built on this library, please see:

https://user-images.githubusercontent.com/44126651/224557966-cbbb6b67-0f6c-4bf7-9416-b8114cd06de8.mp4

This is deployed to github pages and available at: [Open AI UI](https://johniwasz.github.io/whetstone.chatgpt/). Source for the Blazor web app is at [Whetstone.ChatGPT.Blazor.App](https://github.com/johniwasz/whetstone.chatgpt/tree/main/src/examples/blazor/Whetstone.ChatGPT.Blazor.App/Whetstone.ChatGPT.Blazor.App).

[Examples](https://github.com/johniwasz/whetstone.chatgpt/tree/main/src/examples) include:

- [Command line bot](https://github.com/johniwasz/whetstone.chatgpt/tree/main/src/examples/Whetstone.ChatGPT.SimpleCommandlineBot)

## Dependency Injection Quickstart

```C#
services.Configure<ChatGPTCredentials>(options =>
{    
    options.ApiKey = "YOURAPIKEY";
    options.Organization = "YOURORGANIZATIONID";
});
```

Use: 
```C#
services.AddHttpClient();
```
or:
```C#
services.AddHttpClient<IChatGPTClient, ChatGPTClient>();
```
Configure `IChatGPTClient` service:
```C#
services.AddScoped<IChatGPTClient, ChatGPTClient>();
```

## Chat Completions

Chat completions are a special type of completion that are optimized for chat. They are designed to be used in a conversational context.

### Chat Completion Quickstart

This shows a usage of the GPT-3.5 Turbo model.

``` C#
using Whetstone.ChatGPT;
using Whetstone.ChatGPT.Models;

. . .

var gptRequest = new ChatGPTChatCompletionRequest
{
    Model = ChatGPT35Models.Turbo,
    Messages = new List<ChatGPTChatCompletionMessage>()
        {
            new ChatGPTChatCompletionMessage(ChatGPTMessageRoles.System, "You are a helpful assistant."),
            new ChatGPTChatCompletionMessage(ChatGPTMessageRoles.User, "Who won the world series in 2020?"),
            new ChatGPTChatCompletionMessage(ChatGPTMessageRoles.Assistant, "The Los Angeles Dodgers won the World Series in 2020."),
            new ChatGPTChatCompletionMessage(ChatGPTMessageRoles.User, "Where was it played?")
        },
    Temperature = 0.9f,
    MaxTokens = 100
};

using IChatGPTClient client = new ChatGPTClient("YOURAPIKEY");

var response = await client.CreateChatCompletionAsync(gptRequest);

Console.WriteLine(response?.GetCompletionText());

```

GPT-4 models can also be used provided your account has been granted access to the [limited beta](https://openai.com/waitlist/gpt-4).

## Completion

Completions use [models](https://beta.openai.com/docs/models) to answer a wide variety of tasks, including but not limited to classification, sentiment analysis, answering questions, etc. 

### Completion Quickstart

This shows a direct useage of the __gpt-3.5-turbo-instruct__ model without any prompts.

Please note, _CreateCompletionAsync_ is obsolete. Use _ChatGPTChatCompletionRequest_, _ChatGPTChatCompletionResponse_, and the _CreateChatCompletionAsync_ method instead.

``` C#
using Whetstone.ChatGPT;
using Whetstone.ChatGPT.Models;

. . .

var gptRequest = new ChatGPTCompletionRequest
{
    Model =  ChatGPT35Models.Gpt35TurboInstruct,
    Prompt = "How is the weather?"
};

using IChatGPTClient client = new ChatGPTClient("YOURAPIKEY");

var response = await client.CreateCompletionAsync(gptRequest);

Console.WriteLine(response.GetCompletionText());
```

GPT-3.5 is not deterministic. One of the test runs of the sample above returned:

> The weather can vary greatly depending on location. In general, you can expect temperatures to be moderate and climate to be comfortable, but it is always best to check the forecast for your specific area.

### Completion Code Sample

A C# console application that uses completions is available at:

[Whetstone.ChatGPT.CommandLineBot (chatgpt-marv)](https://github.com/johniwasz/whetstone.chatgpt/tree/main/src/examples/Whetstone.ChatGPT.CommandLineBot)

This sample includes:

- Authentication
- Created a completion request using a prompt
- Processing completion responses

## File Quickstart

How to create a upload a new fine tuning file.

``` C#

List<ChatGPTTurboFineTuneLine> tuningInput = new() 
{
    new ChatGPTTurboFineTuneLine()
        { 
            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
            { 
                 new(ChatGPTMessageRoles.System, "Marv is a factual chatbot that is also sarcastic."),
                 new(ChatGPTMessageRoles.User, "What's the capital of France?"),
                 new(ChatGPTMessageRoles.Assistant, "Paris, as if everyone doesn't know that already.")
            },
        },
    new ChatGPTTurboFineTuneLine()
        {
            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
            {
                 new(ChatGPTMessageRoles.System, "Marv is a factual chatbot that is also sarcastic."),
                 new(ChatGPTMessageRoles.User, "Who wrote 'Romeo and Juliet'?"),
                 new(ChatGPTMessageRoles.Assistant, "Oh, just some guy named William Shakespeare. Ever heard of him?")
            },
        },
    . . .
};

byte[] tuningText = tuningInput.ToJsonLBinary();

string fileName = "marvin.jsonl";

ChatGPTUploadFileRequest? uploadRequest = new ChatGPTUploadFileRequest
{
    File = new ChatGPTFileContent
    {
        FileName = fileName,
        Content = tuningText
    }
};

ChatGPTFileInfo? newTurboTestFile;

using (IChatGPTClient client = new ChatGPTClient("YOURAPIKEY"))
{
    newTurboTestFile = await client.UploadFileAsync(uploadRequest);
}

```

## Fine Tuning Quickstart

Once the file has been created, get the fileId, and reference it when creating a new fine tuning.

```C#
IChatGPTClient client = new ChatGPTClient("YOURAPIKEY");

uploadedFileInfo = await client.UploadFileAsync(uploadRequest);

var fileList = await client.ListFilesAsync();
var foundFile = fileList?.Data?.First(x => x.Filename.Equals("marvin.jsonl"));

ChatGPTCreateFineTuneRequest tuningRequest = new ChatGPTCreateFineTuneRequest
{
    TrainingFileId = foundFile?.Id,
    Model = "gpt-3.5-turbo-1106"
};

ChatGPTFineTuneJob? tuneResponse = await client.CreateFineTuneAsync(tuningRequest);

string? fineTuneId = tuneResponse?.Id;

```

Processing the fine tuning request will take some time. Once it finishes, the __Status__ will report "succeeded" and it's ready to be used in a completion request.

```C#
using IChatGPTClient client = new ChatGPTClient("YOURAPIKEY");

ChatGPTFineTuneJob? tuneResponse = await client.RetrieveFineTuneAsync("FINETUNEID");

if (tuneResponse.Status.Equals("succeeded"))
{
    var gptRequest = new ChatGPTChatCompletionRequest
    {
        Model = "FINETUNEID",
        Messages = new List<ChatGPTChatCompletionMessage>()
        {
            new ChatGPTChatCompletionMessage(ChatGPTMessageRoles.System, "You are a helpful assistant."),
            new ChatGPTChatCompletionMessage(ChatGPTMessageRoles.User, "Who won the world series in 2020?"),
            new ChatGPTChatCompletionMessage(ChatGPTMessageRoles.Assistant, "The Los Angeles Dodgers won the World Series in 2020."),
            new ChatGPTChatCompletionMessage(ChatGPTMessageRoles.User, "Where was it played?")
        },
        Temperature = 0.9f,
        MaxTokens = 100
    };
    
    var response = await client.CreateChatCompletionAsync(gptRequest);

    Console.WriteLine(response?.GetCompletionText());

```

## Image Quickstart

Here's an example that generates a 1024x1024 image.

```C#
ChatGPTCreateImageRequest imageRequest = new()
{
    Prompt = "A sail boat",
    Size = CreatedImageSize.Size1024,
    ResponseFormat = CreatedImageFormat.Base64
};

using IChatGPTClient client = new ChatGPTClient("YOURAPIKEY");

ChatGPTImageResponse? imageResponse = await client.CreateImageAsync(imageRequest);

var imageData = imageResponse?.Data?[0];

if (imageData != null)
{
    byte[]? imageBytes = await client.DownloadImageAsync(imageData);
}
```

## Audio Transcription Quickstart

Her's an example that transcribes an audio file using the Whisper.

```C#

string audioFile = @"audiofiles\transcriptiontest.mp3";

byte[] fileContents = File.ReadAllBytes(audioFile);
ChatGPTFileContent gptFile = new ChatGPTFileContent
{
    FileName = audioFile,
    Content = fileContents
};

ChatGPTAudioTranscriptionRequest? transcriptionRequest = new ChatGPTAudioTranscriptionRequest
{
    File = gptFile
};

using IChatGPTClient client = new ChatGPTClient("YOURAPIKEY");

ChatGPTAudioResponse? audioResponse = await client.CreateTranscriptionAsync(transcriptionRequest, true);

Console.WriteLine(audioResponse?.Text);
```