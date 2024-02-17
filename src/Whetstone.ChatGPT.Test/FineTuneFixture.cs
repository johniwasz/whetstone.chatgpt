// SPDX-License-Identifier: MIT
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models;
using Whetstone.ChatGPT.Models.FineTuning;
using Xunit;

namespace Whetstone.ChatGPT.Test
{
    public class FineTuneFixture
    {
        private bool _isInitialized;

        public FineTuneFixture()
        {

        }

#if NETFRAMEWORK
        internal string ExistingFineTuneId
        {
            get;
            private set;
        }

        internal string ExistingFineTunedModel
        {
            get;
            private set;
        }
#else
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
#endif
        
        internal async Task InitializeAsync()
        {
            if (!_isInitialized)
            {

                string apiKey = ChatGPTTestUtilties.GetChatGPTKey();
                using (ChatGPTClient client = new ChatGPTClient(apiKey))
                {

                    var fineTuneList = await client.ListFineTuneJobsAsync();

                    Assert.NotNull(fineTuneList);

                    Assert.NotNull(fineTuneList.Data);

                    // Enable these next two lines!
                    // Assert.NotEmpty(fineTuneList.Data);

                    Assert.Contains(fineTuneList.Data, (x) => { return !string.IsNullOrWhiteSpace(x.Id); });
                    
                    ChatGPTFineTuneJob fineTuneJob = fineTuneList.Data.Last(x => !string.IsNullOrEmpty(x.Id) && !string.IsNullOrEmpty(x.Status) && x.Status.Equals("succeeded"));
                    Assert.NotNull(fineTuneJob);

                    Assert.NotNull(fineTuneJob.Id);

                    Assert.NotNull(fineTuneJob.FineTunedModel);
                    ExistingFineTuneId = fineTuneJob.Id;
                    ExistingFineTunedModel = fineTuneJob.FineTunedModel;
                    
                    _isInitialized = true;
                }
            }
        }
    }
}
