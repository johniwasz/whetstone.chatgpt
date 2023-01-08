using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Test
{
    public class ImageTests
    {
        [Fact]
        public async Task GenerateImageAsync()
        {

            ChatGPTCreateImageRequest imageRequest = new()
            {
                Prompt = "A sail boat",
                Size = CreatedImageSize.Size256,
                ResponseFormat = CreatedImageFormat.Url
            };


            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                ChatGPTImageResponse? imageResponse = await client.CreateImageAsync(imageRequest);

                Assert.NotNull(imageResponse);

                Assert.NotNull(imageResponse.Data);

                Assert.Single(imageResponse.Data);

                Assert.NotNull(imageResponse.Data[0].Url);

                string? imageData = imageResponse.Data[0].Url;

                Assert.NotNull(imageData);

                byte[]? imageBytes = await client.DownloadImageAsync(imageResponse.Data[0]);

                Assert.NotNull(imageBytes);

                Assert.True(imageBytes.Length > 0);
                File.WriteAllBytes("sailboat.png", imageBytes);
            }
        }


        [Fact]
        public async Task GenerateImageVariation()
        {
            ChatGPTCreateImageVariationRequest imageRequest = new()
            {
                Image = await LoadFileAsync("ImageFiles/sailboat.png"),
                Size = CreatedImageSize.Size1024,
                ResponseFormat = CreatedImageFormat.Base64
            };


            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                ChatGPTImageResponse? imageResponse = await client.CreateImageVariationAsync(imageRequest);

                Assert.NotNull(imageResponse);

                Assert.NotNull(imageResponse.Data);

                Assert.Single(imageResponse.Data);

                Assert.NotNull(imageResponse.Data[0].Base64);

                string? imageData = imageResponse.Data[0].Base64;

                if (imageData != null)
                {
                    byte[] imageBytes = Convert.FromBase64String(imageData);
                    Assert.True(imageBytes.Length > 0);
                    File.WriteAllBytes("sailvariation.png", imageBytes);
                }
            }
        }

        [Fact]
        public async Task CreateImageEdit()
        {

            ChatGPTCreateImageEditRequest imageRequest = new()
            {
                NumberOfImagesToGenerate = 1,
                Prompt = "sail boat with clouds and a sunset",
                Image = await LoadFileAsync("ImageFiles/sailboat.png"),
                Mask = await LoadFileAsync("ImageFiles/sailboat-alpha.png"),
                Size = CreatedImageSize.Size1024,
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

                if (imageData != null)
                {
                    byte[] imageBytes = Convert.FromBase64String(imageData);
                    Assert.True(imageBytes.Length > 0);
                    File.WriteAllBytes("sailvariation.png", imageBytes);
                }
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
