// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models.Moderation;
using Xunit;

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

                Model = ModerationModels.OmniLatest
            };

            string json = JsonSerializer.Serialize(moderationRequest);
            
            Assert.Contains("omni-moderation-latest", json);

            ChatGPTCreateModerationRequest? modRequestDeser = JsonSerializer.Deserialize<ChatGPTCreateModerationRequest>(json);
            Assert.NotNull(modRequestDeser);

            Assert.Equal(ModerationModels.OmniLatest, modRequestDeser.Model);
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

                Model = ModerationModels.OmniLatest
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
