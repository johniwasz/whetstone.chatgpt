// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models.Embeddings;

namespace Whetstone.ChatGPT.Test
{
    public class EmbeddingsTest
    {


        [Fact]
        public async Task ValidateEmbeddingAsync()
        {
            ChatGPTCreateEmbeddingsRequest embeddingsRequest = new ChatGPTCreateEmbeddingsRequest
            {
                Model = ChatGPTEmbeddingModels.Ada,

                Inputs = new List<string>
                {
                    "The food was delicious and the waiter..."
                }
            };

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                ChatGPTCreateEmbeddingsResponse? embeddingsResponse = await client.CreateEmbeddingsAsync(embeddingsRequest);

                Assert.NotNull(embeddingsResponse);

                Assert.NotNull(embeddingsResponse.Data);

                Assert.Equal("list", embeddingsResponse.Object);

                Assert.NotNull(embeddingsResponse.Data[0]);

                Assert.NotNull(embeddingsResponse.Data[0].Embedding);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Assert.True(embeddingsResponse.Data[0].Embedding.Count>0);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
        }

    }
}
