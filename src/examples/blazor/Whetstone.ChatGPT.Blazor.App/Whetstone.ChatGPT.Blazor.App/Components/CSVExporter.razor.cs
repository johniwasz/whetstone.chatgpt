// SPDX-License-Identifier: MIT
using Microsoft.AspNetCore.Components;
using System;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

namespace Whetstone.ChatGPT.Blazor.App.Components
{
    [SupportedOSPlatform("browser")]
    public partial class CSVExporter
    {
        [Parameter]
        public string CSVContent { get; set; } = string.Empty;

        [Parameter]
        public EventCallback<Exception> OnException { get; set; }

        [Parameter]
        public Func<string> CSVRetriever { get; set; } = default!;

        [JSImport("BlazorDownloadFile", "ExportLib")]
        internal static partial string BlazorDownloadFile(string filename, string contentType, string content);

        protected override async Task OnInitializedAsync()
        {
#if GHPAGES
            await LoadTableGeneratorAsync("/whetstone.chatgpt/js/TableGenerator.js");
#else
            await LoadTableGeneratorAsync("../../../js/TableGenerator.js");
#endif
        }

        public async Task ExportCSVAsync()
        {
            try
            {
                string fileName = BuildFileName();

                string csvContent = CSVRetriever();

                if(string.IsNullOrWhiteSpace(csvContent))
                {
                    throw new Exception("No CSV content to export");
                }

                BlazorDownloadFile(fileName, "text/csv", csvContent);
            }
            catch (Exception ex)
            {
                await OnException.InvokeAsync(ex);
            }
        }

        private async Task<bool> LoadTableGeneratorAsync(string generatorScriptPath)
        {
            bool isScriptLoaded = false;
            try
            {
                _ = await JSHost.ImportAsync("ExportLib", generatorScriptPath);
                isScriptLoaded = true;
            }
            catch (Exception ex)
            {
                await OnException.InvokeAsync(new Exception($"{generatorScriptPath} not loaded", ex));
            }

            return isScriptLoaded;
        }
        private string BuildFileName()
        {
            string formattedDate = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            return $"exportlist-{formattedDate}.csv";
        }
    }
}