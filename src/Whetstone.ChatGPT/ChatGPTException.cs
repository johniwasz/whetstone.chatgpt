﻿// SPDX-License-Identifier: MIT
using System.Diagnostics;
using System.Net;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT;


/// <summary>
/// Represents an error that occurs while using the GPT-3 API Chat GPT
/// </summary>
[DebuggerDisplay("Message = {Message}, StatusCode = {StatusCode}")]
public sealed class ChatGPTException : Exception
{

    internal ChatGPTException(ChatGPTError? chatGptError, HttpStatusCode statusCode) : base(chatGptError?.Message)
    {   
        this.ChatGPTError = chatGptError;
        this.StatusCode = statusCode;
    }

    internal ChatGPTException(ChatGPTError? chatGptError, HttpStatusCode statusCode, Exception innerEx) : base(chatGptError?.Message, innerEx)
    {
        this.ChatGPTError = chatGptError;
        this.StatusCode = statusCode;
    }

    internal ChatGPTException(string message, HttpStatusCode statusCode) : base(message)
    {
        this.StatusCode = statusCode;
    }

    internal ChatGPTException(string message, HttpStatusCode statusCode, Exception innerEx) : base(message, innerEx)
    {
        this.StatusCode = statusCode;
    }

    /// <summary>
    /// The error returned by the GPT-3 API. Null if exception was not generated from the API.
    /// </summary>
    public ChatGPTError? ChatGPTError { get; private set; }

    /// <summary>
    /// HTTP status code returned by the GPT-3 API. Null if exception was not generated from the API.
    /// </summary>
    public HttpStatusCode StatusCode { get; private set; }
}
