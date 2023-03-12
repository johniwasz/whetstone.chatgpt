[![CodeQL](https://github.com/johniwasz/whetstone.chatgpt/actions/workflows/codeql.yml/badge.svg)](https://github.com/johniwasz/whetstone.chatgpt/actions/workflows/codeql.yml)

# Whetstone.ChatGPT

A simple light-weight library that wraps the GPT-3 API with support for dependency injection.

Supported features include:

- Completions
- Edits
- Files
- Fine Tunes
- Images
- Embeddings
- Moderations
- Response streaming

For a video walkthrough of a Blazor web app built on this library, please click the image below:

[![You Tube Walkthrough](https://img.youtube.com/vi/ifs98S9ktJU/0.jpg)](https://www.youtube.com/watch?v=ifs98S9ktJU)

This is deployed to github pages and available at: [GPT-3 UI](https://johniwasz.github.io/whetstone.chatgpt/). Source for the Blazor web app is at [Whetstone.ChatGPT.Blazor.App](https://github.com/johniwasz/whetstone.chatgpt/tree/main/src/examples/blazor/Whetstone.ChatGPT.Blazor.App/Whetstone.ChatGPT.Blazor.App).

[Examples](https://github.com/johniwasz/whetstone.chatgpt/tree/main/src/examples) include:

- [Command line bot](https://github.com/johniwasz/whetstone.chatgpt/tree/main/src/examples/Whetstone.ChatGPT.SimpleCommandlineBot)
- [Azure Function Twitter Webhook](https://github.com/johniwasz/whetstone.chatgpt/tree/main/src/examples/twitter-webhook) that responds to DMs

https://user-images.githubusercontent.com/44126651/224557966-cbbb6b67-0f6c-4bf7-9416-b8114cd06de8.mp4

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

## Completion

GPT-3 Completions use [models](https://beta.openai.com/docs/models) to answer a wide variety of tasks, including but not limited to classification, sentiment analysis, answering questions, etc. 

### Completion Quickstart

This shows a direct useage of the __text-davinci-003__ model without any prompts.

``` C#
using Whetstone.ChatGPT;
using Whetstone.ChatGPT.Models;

. . .

IChatGPTClient client = new ChatGPTClient("YOURAPIKEY");

var gptRequest = new ChatGPTCompletionRequest
{
    Model = ChatGPTCompletionModels.Davinci,
    Prompt = "How is the weather?"
};

var response = await client.CreateCompletionAsync(gptRequest);

Console.WriteLine(response.GetCompletionText());
```

GPT-3 is not deterministic. One of the test runs of the sample above returned:

> The weather can vary greatly depending on location. In general, you can expect temperatures to be moderate and climate to be comfortable, but it is always best to check the forecast for your specific area.

### Completion Code Sample

A C# console application that uses completions is available at:

[Whetstone.ChatGPT.CommandLineBot (chatgpt-marv)](https://github.com/johniwasz/whetstone.chatgpt/tree/main/src/examples/Whetstone.ChatGPT.CommandLineBot)

This sample includes:

- Authentication
- Created a completion request using a prompt
- Processing completion responses

## Editing Quickstart

To use submit an edit completion request:

``` C#
using Whetstone.ChatGPT;
using Whetstone.ChatGPT.Models;

. . .

IChatGPTClient client = new ChatGPTClient("YOURAPIKEY");

var gptEditRequest = new ChatGPTCreateEditRequest
{             
    Input = "What day of the wek is it?",
    Instruction = "Fix spelling mistakes"
};

var response = await client.CreateEditAsync(gptEditRequest);

Console.WriteLine(response.GetEditedText());
```

One of the test runs returned:

> What day of the week is it?

## File Quickstart

How to create a upload a new fine tuning file.

``` C#

List<ChatGPTFineTuneLine> tuningInput = new()
{
    new ChatGPTFineTuneLine("<PROMPT>", "<COMPLETION>"),
    new ChatGPTFineTuneLine("<PROMPT>", "<COMPLETION>"),
    . . .
};

byte[] tuningText = tuningInput.ToJsonLBinary();

string fileName = "finetuningsample.jsonl";

ChatGPTUploadFileRequest? uploadRequest = new ChatGPTUploadFileRequest
{
    File = new ChatGPTFileContent
    {
        FileName = fileName,
        Content = tuningText
    }
};

ChatGPTFileInfo? uploadedFileInfo;
using (IChatGPTClient client = new ChatGPTClient("YOURAPIKEY"))
{
    uploadedFileInfo = await client.UploadFileAsync(uploadRequest);
}
```

## Fine Tuning Quickstart

Once the file has been created, get the fileId, and reference it when creating a new fine tuning.

```C#
using (IChatGPTClient client = new ChatGPTClient("YOURAPIKEY"))
{
  var fileList = await client.ListFilesAsync();
  var foundFile =  fileList.Data.First(x => x.Filename.Equals("finetuningsample.jsonl"));

  ChatGPTCreateFineTuneRequest tuningRequest = new ChatGPTCreateFineTuneRequest
  {
     TrainingFileId = foundFile.Id
  };

  ChatGPTFineTuneJob? tuneResponse = await client.CreateFineTuneAsync(tuningRequest);

  string fineTuneId = tuneResponse.Id;

}

```

Processing the fine tuning request will take some time. Once it finishes, the __Status__ will report "succeeded" and it's ready to be used in a completion request.

```C#
using (IChatGPTClient client = new ChatGPTClient("YOURAPIKEY"))
{
  ChatGPTFineTuneJob? tuneResponse = await client.RetrieveFineTuneAsync("FINETUNEID");

  if(tuneResponse.Status.Equals("succeeded"))
  {
    var gptRequest = new ChatGPTCompletionRequest
    {
      Model = ChatGPTCompletionModels.Davinci,
      Prompt = "How is the weather?"
    };

    var response = await client.CreateCompletionAsync(gptRequest);

    Console.WriteLine(response.GetCompletionText());
  }
}

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

using (IChatGPTClient client = new ChatGPTClient("YOURAPIKEY"))
{
    ChatGPTImageResponse? imageResponse = await client.CreateImageAsync(imageRequest);

    var imageData = imageResponse?.Data[0];

    if (imageData != null)
    {
        byte[] imageBytes = await client.DownloadImageAsync(imageData);
    }
}
```
