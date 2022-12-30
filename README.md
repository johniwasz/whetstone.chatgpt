# Whetstone.ChatGPT

A simple light-weight library that wraps ChatGPT API completions. Additions to support images and other beta features are in progress.

```
string apiKey = GetChatGPTKey();

ChatGPTClient client = new ChatGPTClient(apiKey);

var gptRequest = new ChatGPTCompletionRequest
{
    Model = ChatGPTCompletionModels.Ada,
    Prompt = "How is the weather?",
    Temperature = 0.9f,
    MaxTokens = 140
};

var response = await client.GetResponseAsync(gptRequest);

Console.WriteLine(response.Choices?[0]?.Text);
```