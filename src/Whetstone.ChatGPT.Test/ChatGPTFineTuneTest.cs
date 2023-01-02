using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models;
using Xunit.Abstractions;

namespace Whetstone.ChatGPT.Test
{
    public class ChatGPTFineTuneTest : IClassFixture<FileTestFixture>
    {

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly FileTestFixture _fileTestFixture;

        public ChatGPTFineTuneTest(ITestOutputHelper testOutputHelper, FileTestFixture fileTestFixture)
        {
            _testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
            _fileTestFixture = fileTestFixture ?? throw new ArgumentNullException(nameof(fileTestFixture));

            _fileTestFixture.TestOutputHelper = _testOutputHelper;
        }

        [Fact]
        public async Task SubmitFineTuningRequest()
        {

            await InitializeExistingFileIdAsync();

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

                ChatGPTCreateFineTuneRequest tuningRequest = new ChatGPTCreateFineTuneRequest();
                tuningRequest.TrainingFileId = _fileTestFixture.ExistingFileId;


                ChatGPTCreateFineTuneResponse? tuneResponse = await client.CreateFineTuneAsync(tuningRequest);

                Assert.NotNull(tuneResponse);
                Assert.NotNull(tuneResponse.Status);

                _testOutputHelper.WriteLine($"Status: {tuneResponse.Status}");
            }
        }

        private async Task InitializeExistingFileIdAsync()
        {
            await _fileTestFixture.InitializeFirstFromListAsync();
            Assert.NotNull(_fileTestFixture.ExistingFileId);
        }
    }
}
