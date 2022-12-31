[![NuGet version](https://badge.fury.io/nu/Whetstone.ChatGPT.svg)](https://badge.fury.io/nu/Whetstone.ChatGPT) [![CodeQL](https://github.com/johniwasz/whetstone.chatgpt/actions/workflows/codeql.yml/badge.svg)](https://github.com/johniwasz/whetstone.chatgpt/actions/workflows/codeql.yml)

# Whetstone.ChatGPT

A simple light-weight library that wraps ChatGPT API completions. Additions to support images and other beta features are in progress.

## Completion

ChatGPT Completions use [models](https://beta.openai.com/docs/models) answer a wide variety of tasks, including but not limited to classification, sentiment analysis, answering questions, etc. 

### Completion Quickstart

This shows a direct useage of the text-davinci-003 model without any prompts.

``` C#
using Whetstone.ChatGPT;
using Whetstone.ChatGPT.Models;

. . .

ChatGPTClient client = new ChatGPTClient("YOURAPIKEY");

var gptRequest = new ChatGPTCompletionRequest
{
    Model = ChatGPTCompletionModels.Davinci,
    Prompt = "How is the weather?"
};

var response = await client.CreateCompletionAsync(gptRequest);

Console.WriteLine(response.GetCompletionText());
```

ChatGPT is not deterministic. One of the test runs of the sample above returned:

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

ChatGPTClient client = new ChatGPTClient("YOURAPIKEY");

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

