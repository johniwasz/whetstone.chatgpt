using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Net;
using Whetstone.ChatGPT.Models;
using Xunit.Abstractions;
using Xunit.Sdk;
using System.Diagnostics.CodeAnalysis;

namespace Whetstone.ChatGPT.Test
{
    public class ChatGPTFileTest : IClassFixture<FileTestFixture>
    {

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly FileTestFixture _fileTestFixture;

        public ChatGPTFileTest(ITestOutputHelper testOutputHelper, FileTestFixture fileTestFixture)
        {
            _testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
            _fileTestFixture = fileTestFixture ?? throw new ArgumentNullException(nameof(fileTestFixture));

            _fileTestFixture.TestOutputHelper = _testOutputHelper;
        }



        [Fact]
        public async Task ListFilesAsync()
        {

            await InitializeExistingFileIdAsync();

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                var fileList = await client.ListFilesAsync();

                Assert.NotNull(fileList);

                Assert.NotNull(fileList.Object);

                Assert.Equal("list", fileList.Object);

                Assert.NotNull(fileList.Data);

                Assert.NotEmpty(fileList.Data);

                Assert.Contains(fileList.Data, x => x.Id == _fileTestFixture.ExistingFileId);

            }

        }


        [Fact]
        public async Task RetrieveFileAsync()
        {
            await InitializeExistingFileIdAsync();

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

                ChatGPTFileInfo? retrieveResponse = await client.RetrieveFileAsync(_fileTestFixture.ExistingFileId);

                Assert.NotNull(retrieveResponse);

                Assert.NotNull(retrieveResponse.Object);

                Assert.Equal("file", retrieveResponse.Object);

                Assert.Equal(_fileTestFixture.ExistingFileId, retrieveResponse.Id);
            }
        }

        [Fact]
        public async Task RetrieveExistingFileContents()
        {

            await InitializeExistingFileIdAsync();

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                ChatGPTFileContent? retrieveResponse = await client.RetrieveFileContentAsync(_fileTestFixture.ExistingFileId);

                Assert.NotNull(retrieveResponse);
                Assert.NotNull(retrieveResponse.Content);

                string originalContents = System.Text.Encoding.UTF8.GetString(retrieveResponse.Content, 0, retrieveResponse.Content.Length);

                Assert.NotNull(originalContents);

                _testOutputHelper.WriteLine("File Contents:");
                _testOutputHelper.WriteLine(originalContents);

            }
        }

        
        [Fact(Skip = "Cannot delete a file right after creation.")]
        public async Task DeleteFileAsync()

        {

            await InitializeTestFileAsync();

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

                var deleteResponse = await client.DeleteFileAsync(_fileTestFixture.ExistingFileId);

                Assert.NotNull(deleteResponse);

                Assert.NotNull(deleteResponse.Object);

                Assert.Equal("file", deleteResponse.Object);

                Assert.True(deleteResponse.Deleted);

                Assert.Equal(_fileTestFixture.NewTestFile?.Id, deleteResponse.Id);
            }

        }


        [Fact]
        public async Task FileUploadBadFile()
        {
            // Build a fine tine file to upload.


            string fileName = "badfile.jsonl";

            ChatGPTUploadFileRequest? uploadRequest = new ChatGPTUploadFileRequest
            {
                File = new ChatGPTFileContent
                {

                    FileName = fileName,
                    Content = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())
                }
            };

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                ChatGPTException badFileException = await Assert.ThrowsAsync<ChatGPTException>(async () => await client.UploadFileAsync(uploadRequest));

                Assert.NotNull(badFileException.ChatGPTError);

                Assert.NotNull(badFileException.StatusCode);

                Assert.Equal(HttpStatusCode.BadRequest, badFileException.StatusCode);

            }

        }

        private async Task InitializeTestFileAsync()
        {

            await _fileTestFixture.CreateTestFileAsync();

            Assert.NotNull(_fileTestFixture.NewTestFile);
            Assert.NotNull(_fileTestFixture.NewTestFile.Id);
        }

        private async Task InitializeExistingFileIdAsync()
        {
            await _fileTestFixture.InitializeFirstFromListAsync();
            Assert.NotNull(_fileTestFixture.ExistingFileId);
        }

    }
}
