using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models;
using Xunit.Abstractions;

namespace Whetstone.ChatGPT.Test
{
    public class FileTestFixture : IDisposable
    {
        private bool _isDisposed;

        public ChatGPTFileInfo? NewTestFile { get; set; }

        public string? ExistingFileId { get; set; }

        internal ITestOutputHelper? TestOutputHelper { get; set; }

        public void Dispose()
        {
            if(_isDisposed)
            {
                if (NewTestFile is not null)
                {
                    string apiKey = ChatGPTTestUtilties.GetChatGPTKey();

                    using (ChatGPTClient client = new(apiKey))
                    {
                        try
                        {
                            client.DeleteFileAsync(NewTestFile.Id).Wait();
                        }
                        catch(Exception ex)
                        {
                            string message = $"Error deleting file while cleaning up TestFile {NewTestFile.Id}\n: {ex}";

                            if (TestOutputHelper is not null)
                            {
                                TestOutputHelper.WriteLine(message);
                            }
                            else
                            {
                                Console.WriteLine(message);
                            }
                        }
                    }
                }
                _isDisposed = true;
            }
        }


        internal async Task InitializeFirstFromListAsync()
        {
            if (string.IsNullOrWhiteSpace(ExistingFileId))
            {

                string apiKey = ChatGPTTestUtilties.GetChatGPTKey();
                using (ChatGPTClient client = new(apiKey))
                {

                    var fileList = await client.ListFilesAsync();

                    Assert.NotNull(fileList);

                    Assert.NotNull(fileList.Data);

                    Assert.NotEmpty(fileList.Data);

                    ExistingFileId = fileList.Data.First().Id;
                }
            }
        }

        internal async Task CreateTestFileAsync()
        {

            if (NewTestFile == null)
            {


                // Build a fine tine file to upload.
                List<ChatGPTFineTuneLine> tuningInput = new()
                {
                    new ChatGPTFineTuneLine("I don't even know where to start!\n", "Can't go wrong with rabbits, doc.\n"),

                    new ChatGPTFineTuneLine("Requesting clearance for landing.", "Roger, rabbit!\n"),

                    new ChatGPTFineTuneLine("You've got the wrong bunny.", "He just doesn't know star potential when he sees it.\n"),

                    new ChatGPTFineTuneLine("I'm not a rabbit!", "You're a rabbit, Doc!\n"),

                    new ChatGPTFineTuneLine("Now I got you. You, you, wabbit.", "Say doc, are you trying to get yourself in trouble with the law? This ain't wabbit huntin' season.\n"),

                    new ChatGPTFineTuneLine("Duck season.", "Wabbit season.\n"),

                    new ChatGPTFineTuneLine("Wabbit season.", "Duck season.\n"),

                    new ChatGPTFineTuneLine("Awwight. Come on out or I'll bwast you out!", "For shame, doc! Hunting rabbits with an elephant gun.\n"),

                    new ChatGPTFineTuneLine("Hey! What's the big idea? Why don't you wook where you're going.", "Oh, how simply dreadful. You poor little man.\n"),

                    new ChatGPTFineTuneLine("if my luck doesn't change, I'm coming back to get ya.", "nter, O seeker of knowledge.\n"),

                    new ChatGPTFineTuneLine("What kind of flower is that?", "t's a carnation doc! Why?\n")
                };


                byte[] tuningText = tuningInput.ToJsonLBinary();

                string fileName = "bugstuning.jsonl";

                ChatGPTUploadFileRequest? uploadRequest = new ChatGPTUploadFileRequest
                {
                    File = new ChatGPTFileContent
                    {

                        FileName = fileName,
                        Content = tuningText
                    }
                };

                string apiKey = ChatGPTTestUtilties.GetChatGPTKey();

                ChatGPTFileInfo? uploadedFileInfo;
                using (IChatGPTClient client = new ChatGPTClient(apiKey))
                {
                    uploadedFileInfo = await client.UploadFileAsync(uploadRequest);
                }


                using (IChatGPTClient client = new ChatGPTClient(apiKey))
                {
                    NewTestFile = await client.UploadFileAsync(uploadRequest);
                }

                Assert.NotNull(NewTestFile);

                Assert.Equal(fileName, NewTestFile.Filename);
            }
            
        }
    }
}
