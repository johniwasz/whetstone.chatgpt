// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Net;
using Xunit.Abstractions;
using Xunit.Sdk;
using System.Diagnostics.CodeAnalysis;
using Whetstone.ChatGPT.Models.File;

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

            _fileTestFixture.InitializeAsync().Wait();
        }



        [Fact]
        public async Task ListFilesAsync()
        {

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                var fileList = await client.ListFilesAsync();

                Assert.NotNull(fileList);

                Assert.NotNull(fileList.Object);

                Assert.Equal("list", fileList.Object);

                Assert.NotNull(fileList.Data);

                Assert.NotEmpty(fileList.Data);

                Assert.Contains(fileList.Data, x => x.Id == _fileTestFixture.NewTurboTestFile?.Id);

            }

        }


        [Fact]
        public async Task RetrieveFileAsync()
        {
            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

                ChatGPTFileInfo? retrieveResponse = await client.RetrieveFileAsync(_fileTestFixture.NewTurboTestFile?.Id);

                Assert.NotNull(retrieveResponse);

                Assert.NotNull(retrieveResponse.Object);

                Assert.Equal("file", retrieveResponse.Object);

                Assert.Equal(_fileTestFixture.NewTurboTestFile?.Id, retrieveResponse.Id);
            }
        }

        [Fact]
        public async Task RetrieveExistingFileContents()
        {

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                ChatGPTFileContent? retrieveResponse = await client.RetrieveFileContentAsync(_fileTestFixture.NewTurboTestFile?.Id);

                Assert.NotNull(retrieveResponse);
                Assert.NotNull(retrieveResponse.Content);

                string originalContents = System.Text.Encoding.UTF8.GetString(retrieveResponse.Content, 0, retrieveResponse.Content.Length);

                Assert.NotNull(originalContents);

                _testOutputHelper.WriteLine("File Contents:");
                _testOutputHelper.WriteLine(originalContents);

            }
        }


        [Fact]
        public async Task DeleteFileAsync()
        {

            var createdFile = await _fileTestFixture.CreateTestFileAsync();

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

                var deleteResponse = await client.DeleteFileAsync(createdFile.Id);

                Assert.NotNull(deleteResponse);

                Assert.NotNull(deleteResponse.Object);

                Assert.Equal("file", deleteResponse.Object);

                Assert.True(deleteResponse.Deleted);

                Assert.Equal(createdFile?.Id, deleteResponse.Id);
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

            using IChatGPTClient client = ChatGPTTestUtilties.GetClient();

            ChatGPTException badFileException = await Assert.ThrowsAsync<ChatGPTException>(async () => await client.UploadFileAsync(uploadRequest));

            Assert.NotNull(badFileException.ChatGPTError);

            Assert.Equal(HttpStatusCode.BadRequest, badFileException.StatusCode);
        }



    }
}
