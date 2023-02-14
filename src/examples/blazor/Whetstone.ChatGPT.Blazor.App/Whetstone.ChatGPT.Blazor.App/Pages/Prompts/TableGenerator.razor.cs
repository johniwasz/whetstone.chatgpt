using Blazorise;
using Blazorise.Bootstrap5;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.JSInterop;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Linq;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using Whetstone.ChatGPT.Blazor.App.Models;
using Whetstone.ChatGPT.Blazor.App.State;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Blazor.App.Pages.Prompts
{

    [SupportedOSPlatform("browser")]
    public partial class TableGenerator : IDisposable
    {
        private const string PROMPTTEMPLATE = "{0} {1}. CSV Format. Quotes around all fields.";

        [CascadingParameter]
        public ApplicationState AppState { get; set; } = default!;

        private TableRequest tableRequest = new();

        public Exception? exception { get; set; } = default!;

        private bool isLoading = false;

        private IEnumerable<string>? Fields = default;

        private IEnumerable<IEnumerable<string>>? DataRows = default!;

        private ChatGPTCompletionRequest gptCompletionRequest = new();

        private ChatGPTUsage completionUsage = new();

        private Visibility completionDetailsVisibility = Visibility.Invisible;

        private bool isDisposed = false;

        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

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
            catch (Exception ex)
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

            List<string> fieldList = tableRequest.Attributes.Where(x => !string.IsNullOrWhiteSpace(x.Name)).Select(x => x.IsNumeric ? x.Name.Trim() : $"\"{x.Name.Trim()}\"").ToList();

            promptBuilder.Append(string.Join(",", fieldList));
            promptBuilder.Append('.');

            gptCompletionRequest = new()
            {
                Prompt = promptBuilder.ToString(),
                Model = ChatGPTCompletionModels.Davinci,
                MaxTokens = 1000,
                Temperature = 0.0f,
            };

            // Remove any quotes from the lables that will be used in the table display
            fieldList = fieldList.Select(x => x.Trim('"')).ToList();

            try
            {
                isLoading = true;

                if (cancelTokenSource.TryReset())
                {

                    ChatGPTCompletionResponse? completionResponse = await ChatClient.CreateCompletionAsync(gptCompletionRequest, cancelTokenSource.Token);

                    if (completionResponse is not null)
                    {
                        string? rawResponse = completionResponse.GetCompletionText();

                        if (rawResponse is not null)
                        {
                            this.Fields = fieldList;

                            rawResponse = rawResponse.Trim();

                            using (StringReader reader = new(rawResponse))
                            {
                                using (TextFieldParser parser = new(reader))
                                {
                                    parser.TextFieldType = FieldType.Delimited;
                                    parser.SetDelimiters(",");

                                    List<List<string>> dataRows = new();

                                    while (!parser.EndOfData)
                                    {
                                        string[]? fields = parser.ReadFields();
                                        if (fields is not null)
                                        {
                                            fieldList = new();

                                            for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
                                            {
                                                string field = fields[fieldIndex];

                                                if (!string.IsNullOrWhiteSpace(field))
                                                {
                                                    string formattedField = field.Trim();

                                                    if (fieldIndex < tableRequest.Attributes.Count && tableRequest.Attributes[fieldIndex].IsNumeric)
                                                    {
                                                        field = field.Trim('"').Replace(",", string.Empty);
                                                    }

                                                    fieldList.Add(field.Trim());
                                                }
                                            }
                                            dataRows.Add(fieldList);
                                        }
                                    }
                                    DataRows = dataRows;
                                }
                            }
                        }

                        if (completionResponse.Usage is not null)
                        {
                            AppState.UpdateTokenUsage(completionResponse.Usage);
                            completionUsage = completionResponse.Usage;
                        }

                        completionDetailsVisibility = Visibility.Visible;
                    }
                }
            }
            catch (ChatGPTException chatEx)
            {
                Logger.LogError($"ChatGPT error getting completion: {chatEx}");
                exception = chatEx;
            }
            catch (TaskCanceledException)
            {
                // do nothing
                Logger.LogInformation("Task cancelled while getting completion");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error getting completion: {ex}");
                exception = ex;
            }
            finally
            {
                isLoading = false;
            }
        }

        private void AddAttribute()
        {
            tableRequest.Attributes.Add(new AttribueItem());
        }

        private void CancelRequest()
        {
            cancelTokenSource.Cancel();
        }

        private void RemoveAttribute(AttribueItem attribContext)
        {
            tableRequest.Attributes.Remove(attribContext);
        }

        private Visibility IsVisible(bool isFixed)
        {
            return isFixed ? Visibility.Invisible : Visibility.Visible;
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
                foreach(IEnumerable<string> row in DataRows)
                {
                    List<string> fieldValues = new();
                    for (int fieldIndex = 0; fieldIndex < row.Count(); fieldIndex++)
                    {
                        string fieldValue = row.ElementAt(fieldIndex);

                        if (fieldIndex < tableRequest.Attributes.Count && !tableRequest.Attributes[fieldIndex].IsNumeric)
                        {
                            fieldValue = $"\"{fieldValue}\"";
                        }

                        fieldValues.Add(fieldValue);
                    }
                    stringBuilder.AppendLine(string.Join(",", fieldValues));
                }
            }
            return stringBuilder.ToString();
        }

        #region Clean Up
        ~TableGenerator()
        {
            Dispose(true);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                cancelTokenSource.Cancel();
                cancelTokenSource.Dispose();
                isDisposed = true;
            }
        }
        #endregion
    }
}
