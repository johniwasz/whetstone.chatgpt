[![NuGet version](https://badge.fury.io/nu/Whetstone.ChatGPT.svg)](https://badge.fury.io/nu/Whetstone.ChatGPT) [![CodeQL](https://github.com/johniwasz/whetstone.chatgpt/actions/workflows/codeql.yml/badge.svg)](https://github.com/johniwasz/whetstone.chatgpt/actions/workflows/codeql.yml)

# Whetstone.ChatGPT

A simple light-weight library that wraps ChatGPT API completions. Additions to support images and other beta features are in progress.

## Completion Quickstart

```
string apiKey = GetChatGPTKey();

ChatGPTClient client = new ChatGPTClient(apiKey);

var gptRequest = new ChatGPTCompletionRequest
{
    Model = ChatGPTCompletionModels.Davinci,
    Prompt = "How is the weather?",
    Temperature = 0.9f,
    MaxTokens = 140
};

var response = await client.CreateCompletionAsync(gptRequest);

Console.WriteLine(response.GetCompletionText());
```

This writes:

> The weather can vary greatly depending on location. In general, you can expect temperatures to be moderate and climate to be comfortable, but it is always best to check the forecast for your specific area.


## Editing Quickstart

To use submit an edit completion request:

```
string apiKey = GetChatGPTKey();

ChatGPTClient client = new ChatGPTClient(apiKey);

var gptEditRequest = new ChatGPTCreateEditRequest
{             
    Input = "What day of the wek is it?",
    Instruction = "Fix spelling mistakes"
};

var response = await client.CreateEditAsync(gptEditRequest);

Console.WriteLine(response.GetEditedText());
```

This writes:

> What day of the week is it?
