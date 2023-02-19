using Markdig;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Xml;
using Whetstone.ChatGPT.Blazor.App.Components;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Blazor.App.Pages.Prompts
{
    public partial class TestDataGenerator
    {
        private string Response { get; set; } = default!;

        public Task ProcessCompletionResponseAsync(ChatGPTCompletionResponse completionResponse)
        {
            if (completionResponse is not null)
            {
                string? completionText = completionResponse.GetCompletionText();
                Response = string.IsNullOrEmpty(completionText) ? string.Empty : completionText;
            }

            return Task.CompletedTask;
        }

        private MarkupString FormatResponse(string response)
        {
            MarkupString retVal = default;

            try
            {
                string formattedJson = FormatJsonText(response);
                retVal = (MarkupString) $"<pre>{formattedJson}</pre>";
            }
            catch(Exception ex)
            {
                Logger.LogWarning("Response string is invalid JSON: {exception}", ex);
                retVal = (MarkupString)response.Replace(Environment.NewLine, "<br/>");
            }

            return retVal;
        }

        static string FormatJsonText(string jsonString)
        {
            using var doc = JsonDocument.Parse(
                jsonString,
                new JsonDocumentOptions
                {
                    AllowTrailingCommas = true
                }
            );

            using MemoryStream memoryStream = new();

            using (
                Utf8JsonWriter utf8JsonWriter = new(
                    memoryStream,
                    new JsonWriterOptions
                    {
                        Indented = true
                    }
                )
            )
            {
                doc.WriteTo(utf8JsonWriter);
            }
            return new System.Text.UTF8Encoding()
                .GetString(memoryStream.ToArray());
        }
    }
}
