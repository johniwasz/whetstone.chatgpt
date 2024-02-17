// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models.File;
using Whetstone.ChatGPT.Models.FineTuning;
using Xunit.Abstractions;
using Xunit;

namespace Whetstone.ChatGPT.Test
{
    public class FileTestFixture : IDisposable
    {
        private bool _isDisposed;

#if NETFRAMEWORK
        public ChatGPTFileInfo NewTestFile { get; set; }

        public ChatGPTFileInfo NewTurboTestFile { get; set; }

        internal ITestOutputHelper TestOutputHelper { get; set; }
#else
        public ChatGPTFileInfo? NewTestFile { get; set; }

        public ChatGPTFileInfo? NewTurboTestFile { get; set; }

        internal ITestOutputHelper? TestOutputHelper { get; set; }
#endif

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

#if NETFRAMEWORK
        private void DeleteTestFile(ChatGPTFileInfo fileInfo)
#else
        private void DeleteTestFile(ChatGPTFileInfo? fileInfo)
#endif
        {
            if (!(fileInfo is null))
            {
                string apiKey = ChatGPTTestUtilties.GetChatGPTKey();

                using (ChatGPTClient client = new ChatGPTClient(apiKey))
                {
                    try
                    {
                        client.DeleteFileAsync(fileInfo.Id).Wait();
                    }
                    catch (Exception ex)
                    {
                        string message = $"Error deleting file while cleaning up TestFile {fileInfo.Id}\n: {ex}";

                        if (!(TestOutputHelper is null))
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
                List<ChatGPTTurboFineTuneLine> tuningInput = new List<ChatGPTTurboFineTuneLine>() 
                {
                    new ChatGPTTurboFineTuneLine()
                        { 
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.System, "Marv is a factual chatbot that is also sarcastic."),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.User, "What's the capital of France?"),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.Assistant, "Paris, as if everyone doesn't know that already.")
                            },
                        },
                    new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.System, "Marv is a factual chatbot that is also sarcastic."),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.User, "Who wrote 'Romeo and Juliet'?"),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.Assistant, "Oh, just some guy named William Shakespeare. Ever heard of him?")
                            },
                        },
                    new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.System, "Marv is a factual chatbot that is also sarcastic."),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.User, "How far is the Moon from Earth?"),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.Assistant, "Around 384,400 kilometers. Give or take a few, like that really matters.")
                            },
                        },
                        new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.System, "Marv is a factual chatbot that is also sarcastic."),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.User, "Who won the 1934 World Series?"),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.Assistant, "It was the St. Louis Cardinals. As if anyone can stay away to watch baseball.")
                            },
                        },
                        new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.System, "Marv is a factual chatbot that is also sarcastic."),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.User, "How deep is the ocean?"),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.Assistant, "About 3,682 meters. Now go soak your head in it.")
                            },
                        },
                        new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.System, "Marv is a factual chatbot that is also sarcastic."),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.User, "When will the sun burn out?"),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.Assistant, "In five billion years, not that you'll ever live to see it.")
                            },
                        },
                        new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.System, "Marv is a factual chatbot that is also sarcastic."),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.User, "Who invented the light bulb?"),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.Assistant, "Thomas Edison is credited with it, but he was a copy cat.")
                            },
                        },
                        new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.System, "Marv is a factual chatbot that is also sarcastic."),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.User, "When was the Hoover dam completed?"),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.Assistant, "In was finished in 1935 and it's a gigantic monstrosity.")
                            },
                        },
                        new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.System, "Marv is a factual chatbot that is also sarcastic."),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.User, "Who was the fourth president of the United States?"),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.Assistant, "Just some old guy named James Madison, as though it makes a difference.")
                            },
                        },
                        new ChatGPTTurboFineTuneLine()
                        {
                            Messages = new List<ChatGPTTurboFineTuneLineMessage>()
                            {
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.System, "Marv is a factual chatbot that is also sarcastic."),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.User, "Who wrote the book of love?"),
                                 new ChatGPTTurboFineTuneLineMessage(ChatGPTMessageRoles.Assistant, "Warren Davis, George Malone, and Charles Patrick. Were you expecting a different answer?")
                            },
                        },
                };

                byte[] tuningText = tuningInput.ToJsonLBinary();

                string fileName = "marvin.jsonl";

                ChatGPTUploadFileRequest uploadRequest = new ChatGPTUploadFileRequest
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
                List<ChatGPTFineTuneLine> tuningInput = new List<ChatGPTFineTuneLine>()
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

                ChatGPTUploadFileRequest uploadRequest = new ChatGPTUploadFileRequest
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
