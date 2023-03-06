using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualBasic.FileIO;
using System.Runtime.Versioning;
using System.Text;
using Whetstone.ChatGPT.Blazor.App.Components;
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

        private ChatGPTCompletionResponse gptCompletionResponse = new();

        private ChatGPTUsage completionUsage = new();

        private Visibility completionDetailsVisibility = Visibility.Invisible;

        private bool isDisposed = false;

        private readonly CancellationTokenSource cancelTokenSource = new();

        private ChatOptionsSelector? optionsSelector = default!;

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
                Model = optionsSelector!.SelectedModel,
                MaxTokens = optionsSelector!.MaxTokens,
                Temperature = optionsSelector!.Temperature,
            };

            // Remove any quotes from the lables that will be used in the table display
            fieldList = fieldList.Select(x => x.Trim('"')).ToList();

            string? rawResponse = default;
            isLoading = true;

            try
            {
                if (cancelTokenSource.TryReset())
                {
                    ChatGPTCompletionResponse? completionResponse = await ChatClient.CreateCompletionAsync(gptCompletionRequest, cancelTokenSource.Token);
                    
                    if (completionResponse is not null)
                    {
                        gptCompletionResponse = completionResponse;
                        
                        rawResponse = gptCompletionResponse.GetCompletionText();
                        rawResponse = rawResponse!.Trim();
                        
                        if (gptCompletionResponse.Usage is not null)
                        {
                            AppState.UpdateTokenUsage(gptCompletionResponse.Usage);
                            completionUsage = gptCompletionResponse.Usage;
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
                // do nothing except log.
                Logger.LogInformation("Task cancelled while getting completion");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error getting completion: {ex}");
                exception = ex;
            }

            if (rawResponse is not null)
            {
                this.Fields = fieldList;
                try
                {
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
                catch (Exception ex)
                {
                    Logger.LogError($"Error processing completion: {ex}");
                    exception = ex;
                }
                finally
                {
                    isLoading = false;
                }
            }
            else
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

        private string GetCSV()
        {
            StringBuilder stringBuilder = new();

            if (Fields is not null)
                stringBuilder.AppendLine(string.Join(",", Fields));

            if (DataRows is not null)
            {
                foreach (IEnumerable<string> row in DataRows)
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

        private async Task ProcessExceptionAsync(Exception ex)
        {
            exception = ex;
            await Task.CompletedTask;
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
