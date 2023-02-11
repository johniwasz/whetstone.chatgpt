using Blazorise;
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
        private const string PROMPTTEMPLATE = "Top {0} {1}. CSV Format.";

        private readonly string DEFAULTCOLUMNS = "Number, \"Category\"";

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
            await LoadTableGeneratorAsync("/whetstone.chatgpt/js/TableGenerator.js");
#else
            await LoadTableGeneratorAsync("../../../js/TableGenerator.js");
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

            StringBuilder promptBuilder = new StringBuilder();

            promptBuilder.AppendLine(string.Format(PROMPTTEMPLATE, tableRequest.MaxItems, tableRequest.Category));
            promptBuilder.Append(DEFAULTCOLUMNS);
            promptBuilder.Append('.');

            ChatGPTCompletionRequest gptPromptRequest = new()
            {
                Prompt = promptBuilder.ToString(),
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
                        List<string> fieldList = new();
                        string[] columnArr = DEFAULTCOLUMNS.Split(',');
                        for (int i = 0; i < columnArr.Length; i++)
                        {
                            fieldList.Add(columnArr[i].Trim().Replace("\"", ""));
                        }
                        
                        this.Fields = fieldList;

                        rawResponse = rawResponse.Trim();

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
                                        fieldList = new();

                                        foreach (string field in fields)
                                        {
                                            if (!string.IsNullOrWhiteSpace(field))
                                            {
                                                fieldList.Add(field);
                                            }
                                        }
                                        dataRows.Add(fieldList);
                                        
                                    }
                                    
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                exception = ex;
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
                string fileName = BuildFileName();
                BlazorDownloadFile(fileName, "text/csv", csvContents);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                exception = ex;
            }
        }

        private string BuildFileName()
        {
            string formattedDate = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            return $"exportlist-{formattedDate}.csv";
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
