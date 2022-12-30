using System;
using System.Diagnostics.CodeAnalysis;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT;

public class ChatGPTException : Exception
{

    public ChatGPTException(ChatGPTError? chatGptError) : base(chatGptError?.Message)
    {   
        this.ChatGPTError = chatGptError;
    }

    public ChatGPTException(string? message) : base(message)
    {
        
    }

    public ChatGPTError? ChatGPTError { get; private set; }
}
