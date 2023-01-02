using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models;
using Xunit.Abstractions;

namespace Whetstone.ChatGPT.Test
{
    public class ChatGPTFineTuneTest : IClassFixture<FileTestFixture>, IClassFixture<FineTuneFixture>
    {

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly FileTestFixture _fileTestFixture;
        private readonly FineTuneFixture _fineTuneFixture;

        public ChatGPTFineTuneTest(ITestOutputHelper testOutputHelper, FileTestFixture fileTestFixture, FineTuneFixture fineTuneFixture)
        {
            _testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
            _fileTestFixture = fileTestFixture ?? throw new ArgumentNullException(nameof(fileTestFixture));
            _fineTuneFixture = fineTuneFixture ?? throw new ArgumentNullException(nameof(fineTuneFixture));
            
            _fileTestFixture.TestOutputHelper = _testOutputHelper;
        }

        [Fact(Skip = "Cancelling a fine tune job leaves an orphaned job. Until there is a way to clean up orphaned jobs, this will be skipped.")]
        public async Task SubmitFineTuningRequestCancelAndDeleteAsync()

        {

            await InitializeTest();

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

                ChatGPTCreateFineTuneRequest tuningRequest = new ChatGPTCreateFineTuneRequest
                {
                    TrainingFileId = _fileTestFixture.ExistingFileId
                };

                ChatGPTFineTuneJob? tuneResponse = await client.CreateFineTuneAsync(tuningRequest);

                Assert.NotNull(tuneResponse);
                Assert.NotNull(tuneResponse.Status);
                Assert.NotNull(tuneResponse.Id);
                
                _testOutputHelper.WriteLine($"Status: {tuneResponse.Status}");

                Thread.Sleep(2000);

                tuneResponse = await client.CancelFineTuneAsync(tuneResponse.Id);

                Assert.NotNull(tuneResponse);
                Assert.NotNull(tuneResponse.Status);
                Assert.NotNull(tuneResponse.Id);

                _testOutputHelper.WriteLine($"Status: {tuneResponse.Status}");

                //Assert.NotNull(tuneResponse.FineTunedModel);
                //ChatGPTDeleteResponse? deleteResponse = await client.DeleteModelAsync(tuneResponse.FineTunedModel);

                //Assert.NotNull(deleteResponse);
                //Assert.NotNull(deleteResponse.Object);

                //_testOutputHelper.WriteLine($"Deleted: {deleteResponse.Deleted}");

            }
        }

        [Fact]
        public async Task SubmitBadFineTuningRequestAsync()
        {

            await InitializeTest();

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

                ChatGPTCreateFineTuneRequest tuningRequest = new ChatGPTCreateFineTuneRequest();
                tuningRequest.TrainingFileId = _fileTestFixture.ExistingFileId;
                tuningRequest.Model = Guid.NewGuid().ToString();

                ChatGPTException badFileException = await Assert.ThrowsAsync<ChatGPTException>(async () => await client.CreateFineTuneAsync(tuningRequest));

                Assert.NotNull(badFileException.ChatGPTError);

                Assert.NotNull(badFileException.StatusCode);

                Assert.Equal(HttpStatusCode.BadRequest, badFileException.StatusCode);

            }
        }


        [Fact]
        public async Task RetrieveFineTuningAsync()
        {

            await InitializeTest();

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                ChatGPTFineTuneJob? tuneResponse = await client.RetrieveFineTuneAsync(_fineTuneFixture.ExistingFineTuneId);

                Assert.NotNull(tuneResponse);
                Assert.NotNull(tuneResponse.Status);

                _testOutputHelper.WriteLine($"Status: {tuneResponse.Status}");

                if (tuneResponse.ResultFiles?.Count > 0)
                {
                    _testOutputHelper.WriteLine($"Result Files: {tuneResponse.ResultFiles.Count}");

                    foreach (var resultFile in tuneResponse.ResultFiles)
                    {
                        _testOutputHelper.WriteLine($"Result File: {resultFile}");
                    }

                    ChatGPTFileInfo fileInfo = tuneResponse.ResultFiles.First();

                    ChatGPTFileContent? fileContent = await client.RetrieveFileContentAsync(fileInfo.Id);

                    Assert.NotNull(fileContent);

                    Assert.NotNull(fileContent.Content);

                    string fileContentString = Encoding.UTF8.GetString(fileContent.Content);
                    _testOutputHelper.WriteLine(fileContentString);

                }
            }
        }

        [Fact]
        public async Task RetrieveFineTuningEventsAsync()
        {

            await InitializeTest();

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                ChatGPTListResponse<ChatGPTEvent>? eventResponse = await client.ListFineTuneEventsAsync(_fineTuneFixture.ExistingFineTuneId);

                Assert.NotNull(eventResponse);
                Assert.NotNull(eventResponse.Data);
                Assert.NotEmpty(eventResponse.Data);

                foreach (var fineTuneJobEvent in eventResponse.Data)
                {
                    _testOutputHelper.WriteLine($"Event: {fineTuneJobEvent.Message}, {fineTuneJobEvent.CreatedAt}");
                }
            }
        }


        [Fact]
        public async Task CancelCompletedFineTuneJobAsync()
        {

            await InitializeTest();

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                ChatGPTException completedJobException = await Assert.ThrowsAsync<ChatGPTException>(async () => await client.CancelFineTuneAsync(_fineTuneFixture.ExistingFineTuneId));

                Assert.Contains("Cannot cancel a job", completedJobException.Message, StringComparison.OrdinalIgnoreCase);

                Assert.Equal(HttpStatusCode.BadRequest, completedJobException.StatusCode);
            }
        }

        
        [Fact(Skip = "Testing model deletes is expensive, so it's a manual test.")]
        public async Task DeleteModelAsync()
        {

            await InitializeTest();

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                ChatGPTDeleteResponse? deleteResponse = await client.DeleteModelAsync(_fineTuneFixture.ExistingFineTunedModel);

                Assert.NotNull(deleteResponse);
                Assert.NotNull(deleteResponse.Object);

                _testOutputHelper.WriteLine($"Deleted: {deleteResponse.Deleted}");
            }
        }


        [Fact]
        public async Task GPTFineTuneCompletion()
        {
            await InitializeTest();
            
            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                var gptRequest = new ChatGPTCompletionRequest
                {
                    Model = _fineTuneFixture.ExistingFineTunedModel,
                    Prompt = "How is the weather?",
                    Temperature = 0.9f,
                    MaxTokens = 10
                };

                var response = await client.CreateCompletionAsync(gptRequest);

                Assert.NotNull(response);

                Assert.True(!string.IsNullOrWhiteSpace(response.GetCompletionText()));
            }
        }


        private async Task InitializeTest()
        {
            await _fileTestFixture.InitializeFirstFromListAsync();
            Assert.NotNull(_fileTestFixture.ExistingFileId);
            
            await _fineTuneFixture.InitializeAsync();
            Assert.NotNull(_fineTuneFixture.ExistingFineTuneId);
            Assert.NotNull(_fineTuneFixture.ExistingFineTunedModel);

        }
    }
}
