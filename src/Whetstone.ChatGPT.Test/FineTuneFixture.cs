// SPDX-License-Identifier: MIT
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Test
{
    public class FineTuneFixture
    {
        private bool _isInitialized;

        public FineTuneFixture()
        {

        }
        
        internal string? ExistingFineTuneId
        {
            get;
            private set;
        }

        internal string? ExistingFineTunedModel
        {
            get;
            private set;
        }

        internal async Task InitializeAsync()
        {
            if (!_isInitialized)
            {

                string apiKey = ChatGPTTestUtilties.GetChatGPTKey();
                using (ChatGPTClient client = new(apiKey))
                {

                    var fineTuneList = await client.ListFineTunesAsync();

                    Assert.NotNull(fineTuneList);

                    Assert.NotNull(fineTuneList.Data);

                    Assert.NotEmpty(fineTuneList.Data);

                    Assert.Contains(fineTuneList.Data, (x) => { return !string.IsNullOrWhiteSpace(x.Id); });

                    // var filteredJobs = fineTuneList.Data.Where(x => !string.IsNullOrEmpty(x.Id) && !string.IsNullOrEmpty(x.Status) && x.Status.Equals("succeeded"));

                   // filteredJobs.Last();

                    ChatGPTFineTuneJob fineTuneJob = fineTuneList.Data.Last(x => !string.IsNullOrEmpty(x.Id) && !string.IsNullOrEmpty(x.Status) && x.Status.Equals("succeeded"));

                    ExistingFineTuneId = fineTuneJob.Id;
                    ExistingFineTunedModel = fineTuneJob.FineTunedModel;
                    _isInitialized = true;
                }
            }
        }
    }
}
