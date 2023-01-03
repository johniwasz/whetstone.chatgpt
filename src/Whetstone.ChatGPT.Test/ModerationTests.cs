using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Test
{
    public class ModerationTests
    {
        [Fact]
        public void SerializeModerationRequest()
        {
            ChatGPTCreateModerationRequest moderationRequest = new ChatGPTCreateModerationRequest
            {
                Inputs = new List<string>
            {
                "I want to kill them all."
            },

                Model = ModerationModels.Stable
            };

            string json = JsonSerializer.Serialize(moderationRequest);
            
            Assert.Contains("text-moderation-stable", json);

            ChatGPTCreateModerationRequest? modRequestDeser = JsonSerializer.Deserialize<ChatGPTCreateModerationRequest>(json);

            Assert.NotNull(modRequestDeser);

            Assert.Equal(ModerationModels.Stable, modRequestDeser.Model);
        }

        [Fact]
        public void SerializeModerationRequestNullModel()
        {
            ChatGPTCreateModerationRequest moderationRequest = new ChatGPTCreateModerationRequest
            {
                Inputs = new List<string>
                {
                    "I want to kill them all."
                }
            };

            string json = JsonSerializer.Serialize(moderationRequest);

            ChatGPTCreateModerationRequest? modRequestDeser = JsonSerializer.Deserialize<ChatGPTCreateModerationRequest>(json);

            Assert.NotNull(modRequestDeser);
            
        }

        [Fact]
        public async Task ValidateModerationAsync()
        {
            ChatGPTCreateModerationRequest moderationRequest = new ChatGPTCreateModerationRequest
            {
                Inputs = new List<string>
            {
                "I want to kill them all.",
                "I want to kill me."
            },

                Model = ModerationModels.Latest
            };

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                ChatGPTCreateModerationResponse? moderationResponse = await client.CreateModerationAsync(moderationRequest);

                Assert.NotNull(moderationResponse);

                Assert.NotNull(moderationResponse.Results);

                Assert.Equal(2, moderationResponse.Results.Count);

                ModerationResult firstModerationResult = moderationResponse.Results[0];

                Assert.NotNull(firstModerationResult.Categories);

                Assert.True(firstModerationResult.Categories.Violence);

                ModerationResult secondModerationResult = moderationResponse.Results[1];

                Assert.NotNull(secondModerationResult.Categories);

                Assert.True(secondModerationResult.Categories.SelfHarm);
            }
        }
    }
}
