using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Whetstone.ChatGPT.Test
{
    public class ChatGPTFileTest
    {

        private ITestOutputHelper _testOutputHelper;

        public ChatGPTFileTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestGPTFileUpload()
        {
            // Build a fine tine file to upload.
            List<ChatGPTFineTuneLine> tuningInput = new()
            {
                new ChatGPTFineTuneLine("I don't even know where to start!", "Can't go wrong with rabbits, doc."),

                new ChatGPTFineTuneLine("Requesting clearance for landing.", "Roger, rabbit!"),

                new ChatGPTFineTuneLine("You've got the wrong bunny.", "He just doesn't know star potential when he sees it."),

                new ChatGPTFineTuneLine("I'm not a rabbit!", "You're a rabbit, Doc!"),

                new ChatGPTFineTuneLine("Now I got you. You, you, wabbit.", "Say doc, are you trying to get yourself in trouble with the law? This ain't wabbit huntin' season."),

                new ChatGPTFineTuneLine("Duck season.", "Wabbit season."),

                new ChatGPTFineTuneLine("Wabbit season.", "Duck season."),

                new ChatGPTFineTuneLine("Awwight. Come on out or I'll bwast you out!", "For shame, doc! Hunting rabbits with an elephant gun."),

                new ChatGPTFineTuneLine("Hey! What's the big idea? Why don't you wook where you're going.", "Oh, how simply dreadful. You poor little man."),

                new ChatGPTFineTuneLine("if my luck doesn't change, I'm coming back to get ya.", "nter, O seeker of knowledge."),

                new ChatGPTFineTuneLine("What kind of flower is that?", "t's a carnation doc! Why?")
            };

            StringBuilder builder = new StringBuilder();
            foreach (var line in tuningInput)
            {
                builder.AppendLine(System.Text.Json.JsonSerializer.Serialize(line));
            }

            string curDir = System.IO.Directory.GetCurrentDirectory();

            _testOutputHelper.WriteLine(curDir);

            File.WriteAllText("bugstuning.jsonl", builder.ToString());

        }

        
    }
}
