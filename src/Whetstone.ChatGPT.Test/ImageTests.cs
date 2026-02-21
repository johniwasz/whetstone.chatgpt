// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models.File;
using Whetstone.ChatGPT.Models.Image;
using Xunit;

namespace Whetstone.ChatGPT.Test
{
    public class ImageTests
    {
        [Fact(Skip = "Skipping due to cost.")]                
        public async Task GenerateImageAsync()
        {

            ChatGPTCreateImageRequest imageRequest = new()
            {
                Prompt = "A sail boat caught in a storm on a lake. There is a sea monster. Photorealistic.",
                Size = CreatedImageSize.Size1024x1024,
                ResponseFormat = CreatedImageFormat.Base64,
                Model = ChatGPTImageModels.Dall_E_3
            };


            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

                ChatGPTImageResponse? imageResponse = await client.CreateImageAsync(imageRequest);

                Assert.NotNull(imageResponse);

                Assert.NotNull(imageResponse.Data);

                Assert.Single(imageResponse.Data);

                string? imageString = imageResponse.Data[0].Base64;

                // Convert the base64 string to bytes
                byte[]? imageBytes = Convert.FromBase64String(imageString!);

                Assert.NotNull(imageBytes);

                Assert.True(imageBytes.Length > 0);
                File.WriteAllBytes("sailboat.png", imageBytes);
            }
        }

        [Fact(Skip = "Skipping due to cost.")]
        public async Task GenerateImageVariation()
        {
            ChatGPTCreateImageVariationRequest imageRequest = new ChatGPTCreateImageVariationRequest()
            {

                Image = await LoadFileAsync("ImageFiles/sailboat.png"),

                Size = CreatedImageSize.Size1024x1024,
                ResponseFormat = CreatedImageFormat.Base64
            };


            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

                ChatGPTImageResponse? imageResponse = await client.CreateImageVariationAsync(imageRequest);

                Assert.NotNull(imageResponse);

                Assert.NotNull(imageResponse.Data);

                Assert.Single(imageResponse.Data);

                string? imageData = imageResponse.Data[0].Base64;

                Assert.False(string.IsNullOrEmpty(imageData));

                byte[] imageBytes = Convert.FromBase64String(imageData);
                Assert.True(imageBytes.Length > 0);
                File.WriteAllBytes("sailvariation.png", imageBytes);
            }
        }

        [Fact(Skip = "Skipping due to cost.")]
        public async Task CreateImageEdit()
        {

            ChatGPTCreateImageEditRequest imageRequest = new ChatGPTCreateImageEditRequest()
            {
                NumberOfImagesToGenerate = 1,
                Prompt = "sail boat with clouds and a sunset",

                Image = await LoadFileAsync("ImageFiles/sailboat.png"),
                Mask = await LoadFileAsync("ImageFiles/sailboat-alpha.png"),

                Size = CreatedImageSize.Size1024x1024,
                ResponseFormat = CreatedImageFormat.Base64
            };


            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

                ChatGPTImageResponse? imageResponse = await client.CreateImageEditAsync(imageRequest);


                Assert.NotNull(imageResponse);

                Assert.NotNull(imageResponse.Data);

                Assert.Single(imageResponse.Data);

                Assert.NotNull(imageResponse.Data[0].Base64);

                string? imageData = imageResponse.Data[0].Base64;

                Assert.False(string.IsNullOrEmpty(imageData));

                byte[] imageBytes = Convert.FromBase64String(imageData);
                Assert.True(imageBytes.Length > 0);
                File.WriteAllBytes("sailvariation.png", imageBytes);
            }
        }

        private async Task<ChatGPTFileContent> LoadFileAsync(string filePath)
        {

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Test file {filePath} not found");
            }

            byte[] fileBytes = await File.ReadAllBytesAsync(filePath);

            ChatGPTFileContent fileContents = new ChatGPTFileContent
            {
                FileName = Path.GetFileName(filePath),
                Content = fileBytes
            };

            return fileContents;
        }
    }
}
