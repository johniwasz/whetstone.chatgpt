// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models.File;
using Whetstone.ChatGPT.Models.FineTuning;
using Xunit.Abstractions;

namespace Whetstone.ChatGPT.Test
{
    public class FileTestFixture : IDisposable
    {
        private bool _isDisposed;

        public ChatGPTFileInfo? NewTestFile { get; set; }

        public ChatGPTFileInfo? NewTurboTestFile { get; set; }

        internal ITestOutputHelper? TestOutputHelper { get; set; }


        public async Task InitializeAsync()
        {
            await CreateTestFileAsync();
            await CreateTestTurboFileAsync();

            Assert.NotNull(NewTestFile);
            Assert.NotNull(NewTestFile.Id);

            Assert.NotNull(NewTurboTestFile);
            Assert.NotNull(NewTurboTestFile.Id);
        }

        public void Dispose()
        {
            if(_isDisposed)
            {
                DeleteTestFile(NewTestFile);
                DeleteTestFile(NewTurboTestFile);
                _isDisposed = true;
            }
        }

        private void DeleteTestFile(ChatGPTFileInfo? fileInfo)
        {
            if (fileInfo is not null)
            {
                string apiKey = ChatGPTTestUtilties.GetChatGPTKey();

                using (ChatGPTClient client = new(apiKey))
                {
                    try
                    {
                        client.DeleteFileAsync(fileInfo.Id).Wait();
                    }
                    catch (Exception ex)
                    {
                        string message = $"Error deleting file while cleaning up TestFile {fileInfo.Id}\n: {ex}";

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
        }

        internal async Task<ChatGPTFileInfo> CreateTestTurboFileAsync()
        {

            if (NewTurboTestFile is null)
            {
                // Build a fine tine file to upload.
                List<ChatGPTTurboFineTuneLine> tuningInput = new() 
                {
                    new ChatGPTTurboFineTuneLine()
                        { 
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            { 
                                 new() { Role="system", Content = "Marv is a factual chatbot that is also sarcastic." },
                                 new() { Role="user", Content = "What's the capital of France?" },
                                 new() { Role="assistant", Content = "Paris, as if everyone doesn't know that already." }
                            },
                        },
                    new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new() { Role="system", Content = "Marv is a factual chatbot that is also sarcastic." },
                                 new() { Role="user", Content = "Who wrote 'Romeo and Juliet'?" },
                                 new() { Role="assistant", Content = "Oh, just some guy named William Shakespeare. Ever heard of him?" }
                            },
                        },
                    new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new() { Role="system", Content = "Marv is a factual chatbot that is also sarcastic." },
                                 new() { Role="user", Content = "How far is the Moon from Earth?" },
                                 new() { Role="assistant", Content = "Around 384,400 kilometers. Give or take a few, like that really matters." }
                            },
                        },
                        new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new() { Role="system", Content = "Marv is a factual chatbot that is also sarcastic." },
                                 new() { Role="user", Content = "Who won the 1934 World Series?" },
                                 new() { Role="assistant", Content = "It was the St. Louis Cardinals. As if anyone can stay away to watch baseball." }
                            },
                        },
                        new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new() { Role="system", Content = "Marv is a factual chatbot that is also sarcastic." },
                                 new() { Role="user", Content = "How deep is the ocean?" },
                                 new() { Role="assistant", Content = "About 3,682 meters. Now go soak your head in it." }
                            },
                        },
                        new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new() { Role="system", Content = "Marv is a factual chatbot that is also sarcastic." },
                                 new() { Role="user", Content = "When will the sun burn out?" },
                                 new() { Role="assistant", Content = "In five billion years, not that you'll ever live to see it." }
                            },
                        },
                        new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new() { Role="system", Content = "Marv is a factual chatbot that is also sarcastic." },
                                 new() { Role="user", Content = "Who invented the light bulb?" },
                                 new() { Role="assistant", Content = "Thomas Edison is credited with it, but he was a copy cat." }
                            },
                        },
                        new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new() { Role="system", Content = "Marv is a factual chatbot that is also sarcastic." },
                                 new() { Role="user", Content = "When was the Hoover dam completed?" },
                                 new() { Role="assistant", Content = "In was finished in 1935 and it's a gigantic monstrosity." }
                            },
                        },
                        new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new() { Role="system", Content = "Marv is a factual chatbot that is also sarcastic." },
                                 new() { Role="user", Content = "Who was the fourth president of the United States?" },
                                 new() { Role="assistant", Content = "Just some old guy named James Madison, as though it makes a difference." }
                            },
                        },
                        new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new() { Role="system", Content = "Marv is a factual chatbot that is also sarcastic." },
                                 new() { Role="user", Content = "Who wrote the book of love?" },
                                 new() { Role="assistant", Content = "Warren Davis, George Malone, and Charles Patrick. Were you expecting a different answer?" }
                            },
                        },
                };

                byte[] tuningText = tuningInput.ToJsonLBinary();

                string fileName = "marvin.jsonl";

                ChatGPTUploadFileRequest? uploadRequest = new ChatGPTUploadFileRequest
                {
                    File = new ChatGPTFileContent
                    {

                        FileName = fileName,
                        Content = tuningText
                    }
                };

                string apiKey = ChatGPTTestUtilties.GetChatGPTKey();

                using (IChatGPTClient client = new ChatGPTClient(apiKey))
                {
                    NewTurboTestFile = await client.UploadFileAsync(uploadRequest);
                }

                Assert.NotNull(NewTurboTestFile);
                Assert.Equal(fileName, NewTurboTestFile.Filename);
            }

            return NewTurboTestFile;
        }


        internal async Task<ChatGPTFileInfo> CreateTestFileAsync()
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

                using (IChatGPTClient client = new ChatGPTClient(apiKey))
                {
                    NewTestFile = await client.UploadFileAsync(uploadRequest);
                }

                Assert.NotNull(NewTestFile);
                Assert.Equal(fileName, NewTestFile.Filename);                
            }

            return NewTestFile;
        }
    }
}
