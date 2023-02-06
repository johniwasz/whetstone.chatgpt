﻿using Blazorise;
using Blazorise.Bootstrap5;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Text;
using Whetstone.ChatGPT.Blazor.App.Models;
using Whetstone.ChatGPT.Blazor.App.State;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Blazor.App.Pages.Prompts
{

    [SupportedOSPlatform("browser")]
    public partial class TableGenerator
    {
        private const string PROMPTTEMPLATE = "Top {0} {1}. CSV Format. Include Columns. Columns: {2}";

        private readonly string DEFAULTCOLUMNS = "Number, Name";

        [CascadingParameter]
        public ApplicationState AppState { get; set; } = default!;

        private TableRequest tableRequest = new();

        public Exception? exception { get; set; } = default!;

        private bool isLoading = false;

        private IEnumerable<string>? Fields = default;

        private IEnumerable<IEnumerable<string>>? DataRows = default!;

        protected override async Task OnInitializedAsync()
        {
#if GHPAGES
            string path = "../../../js/TableGenerator.js";
            bool isScriptLoaded = await LoadTableGeneratorAsync(path);

            if (!isScriptLoaded)
            {
                path = "../../../../js/TableGenerator.js";
                isScriptLoaded = await LoadTableGeneratorAsync("../../../../js/TableGenerator.js");
                if(isScriptLoaded)
                {
                    Console.WriteLine($"{path} loaded");
                }
            }
            else
            {
                Console.WriteLine($"{path} loaded");
            }
#else
            await LoadTableGeneratorAsync("../../../js/TableGenerator.js");
            Console.WriteLine("GHPAGES not compiled");
#endif
            
        }

        private async Task<bool> LoadTableGeneratorAsync(string generatorScriptPath)
        {
            bool isScriptLoaded = false;
            try
            {
                await JSHost.ImportAsync("ExportLib", generatorScriptPath);
                isScriptLoaded = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{generatorScriptPath} not loaded");
                Console.WriteLine(ex);
            }

            return isScriptLoaded;
        }

        private async Task HandleSubmitAsync()
        {
            exception = null;

            ChatGPTCompletionRequest gptPromptRequest = new()
            {
                Prompt = string.Format(PROMPTTEMPLATE, tableRequest.MaxItems, tableRequest.Category, DEFAULTCOLUMNS),
                Model = ChatGPTCompletionModels.Davinci,
                MaxTokens = 1000,
                Temperature = 0.0f,
            };

            try
            {
                isLoading = true;

                ChatGPTCompletionResponse? completionResponse = await ChatClient.CreateCompletionAsync(gptPromptRequest);

                if (completionResponse is not null)
                {
                    string? rawResponse = completionResponse.GetCompletionText();

                    if (rawResponse is not null)
                    {
                        rawResponse = string.Concat(DEFAULTCOLUMNS, rawResponse.Trim());

                        int lineIndex = 0;
                        using (StringReader reader = new StringReader(rawResponse))
                        {
                            using (TextFieldParser parser = new TextFieldParser(reader))
                            {
                                parser.TextFieldType = FieldType.Delimited;
                                parser.SetDelimiters(",");

                                List<List<string>> dataRows = new List<List<string>>();
                                
                                while (!parser.EndOfData)
                                {
                                    string[]? fields = parser.ReadFields();
                                    if (fields is not null)
                                    {
                                        List<string> fieldList = new();

                                        foreach (string field in fields)
                                        {
                                            if (!string.IsNullOrWhiteSpace(field))
                                            {
                                                fieldList.Add(field);
                                            }
                                        }
                                        
                                        if (lineIndex == 0)
                                        {
                                            this.Fields = fieldList;
                                        }
                                        else
                                        {
                                            dataRows.Add(fieldList);
                                        }
                                    }
                                    lineIndex++;
                                }

                                DataRows = dataRows;
                            }
                        }
                    }
                    
                    ChatGPTUsage? completionUsage = completionResponse.Usage;

                    if (completionUsage is not null)
                    {
                        AppState.UpdateTokenUsage(completionUsage);
                    }
                }
            }
            catch (ChatGPTException chatEx)
            {
                Console.WriteLine(chatEx);
                exception = chatEx;
            }
            finally
            {
                isLoading = false;
            }
        }

        [JSImport("BlazorDownloadFile", "ExportLib")]
        internal static partial string BlazorDownloadFile(string filename, string contentType, string content);

        public void ExportCSV()
        {
            try
            {
                string csvContents = GetCSV();
                byte[] file = System.Text.Encoding.UTF8.GetBytes(csvContents);

                //JSRuntime.InvokeVoidAsync("BlazorDownloadFile", "exportfile.csv", "text/csv", csvContents);
                BlazorDownloadFile("exportfile.csv", "text/csv", csvContents);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                exception = ex;
            }
        }

        private string GetCSV()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (Fields is not null)
                stringBuilder.AppendLine(string.Join(",", Fields));

            if (DataRows is not null)
            {
                foreach (var row in DataRows)
                {
                    stringBuilder.AppendLine(string.Join(",", row));
                }
            }
            return stringBuilder.ToString();
        }
    }
}