// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models;
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

            ChatGPTCreateImageRequest imageRequest = new ChatGPTCreateImageRequest()
            {
                Prompt = "A sail boat",
                Size = CreatedImageSize.Size256,
                ResponseFormat = CreatedImageFormat.Url
            };


            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
#if NETFRAMEWORK
                ChatGPTImageResponse imageResponse = await client.CreateImageAsync(imageRequest);
#else
                ChatGPTImageResponse? imageResponse = await client.CreateImageAsync(imageRequest);
#endif                
                Assert.NotNull(imageResponse);

                Assert.NotNull(imageResponse.Data);

                Assert.Single(imageResponse.Data);

                Assert.NotNull(imageResponse.Data[0].Url);
                
#if NETFRAMEWORK
                string imageData = imageResponse.Data[0].Url;
#else
                string? imageData = imageResponse.Data[0].Url;
#endif
                Assert.NotNull(imageData);

#if NETFRAMEWORK
                byte[] imageBytes = await client.DownloadImageAsync(imageResponse.Data[0]);
#else
                byte[]? imageBytes = await client.DownloadImageAsync(imageResponse.Data[0]);
#endif
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
#if NETFRAMEWORK
                Image = LoadFile("ImageFiles/sailboat.png"),
#else
                Image = await LoadFileAsync("ImageFiles/sailboat.png"),
#endif
                Size = CreatedImageSize.Size1024,
                ResponseFormat = CreatedImageFormat.Base64
            };


            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
#if NETFRAMEWORK
                ChatGPTImageResponse imageResponse = await client.CreateImageVariationAsync(imageRequest);
#else
                ChatGPTImageResponse? imageResponse = await client.CreateImageVariationAsync(imageRequest);
#endif
                Assert.NotNull(imageResponse);

                Assert.NotNull(imageResponse.Data);

                Assert.Single(imageResponse.Data);

#if NETFRAMEWORK
                string imageData = imageResponse.Data[0].Base64;
#else
                string? imageData = imageResponse.Data[0].Base64;
#endif
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
#if NETFRAMEWORK
                Image = LoadFile("ImageFiles/sailboat.png"),
                Mask = LoadFile("ImageFiles/sailboat-alpha.png"),
#else
                Image = await LoadFileAsync("ImageFiles/sailboat.png"),
                Mask = await LoadFileAsync("ImageFiles/sailboat-alpha.png"),
#endif
                Size = CreatedImageSize.Size1024,
                ResponseFormat = CreatedImageFormat.Base64
            };


            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {

#if NETFRAMEWORK
                ChatGPTImageResponse imageResponse = await client.CreateImageEditAsync(imageRequest);
#else
                ChatGPTImageResponse? imageResponse = await client.CreateImageEditAsync(imageRequest);
#endif

                Assert.NotNull(imageResponse);

                Assert.NotNull(imageResponse.Data);

                Assert.Single(imageResponse.Data);

                Assert.NotNull(imageResponse.Data[0].Base64);

#if NETFRAMEWORK
                string imageData = imageResponse.Data[0].Base64;
#else
                string? imageData = imageResponse.Data[0].Base64;
#endif
                Assert.False(string.IsNullOrEmpty(imageData));

                byte[] imageBytes = Convert.FromBase64String(imageData);
                Assert.True(imageBytes.Length > 0);
                File.WriteAllBytes("sailvariation.png", imageBytes);
            }
        }

#if NETFRAMEWORK
        private ChatGPTFileContent LoadFile(string filePath)
        {

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Test file {filePath} not found");
            }

            byte[] fileBytes = File.ReadAllBytes(filePath);

            ChatGPTFileContent fileContents = new ChatGPTFileContent
            {
                FileName = Path.GetFileName(filePath),
                Content = fileBytes
            };

            return fileContents;
        }

#else
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
#endif
    }
}
