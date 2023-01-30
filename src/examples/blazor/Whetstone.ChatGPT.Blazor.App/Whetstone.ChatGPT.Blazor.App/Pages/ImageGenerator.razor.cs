
using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Whetstone.ChatGPT.Blazor.App.Models;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT.Blazor.App.Pages
{
    
    public partial class ImageGenerator
    {
        private ImageRequest imageRequest = new();

        private Uri imageUrl { get; set; } = default!;

        public LoadingIndicator? loadingIndicator { get; set; } = default!;
        
        private Exception? exception { get; set; } = default!;

        private async Task HandleSubmitAsync()
        {
            exception = null;

            ChatGPTCreateImageRequest gptRequest = new()
            {
                Prompt = imageRequest.ImageDescription,
                Size = imageRequest.ImageSize,
                ResponseFormat = CreatedImageFormat.Url
            };

            try
            {
                imageUrl = null;

                if (loadingIndicator is not null)
                    await loadingIndicator.Show();

                Logger.LogInformation($"Calling OpenAI API with prompt: {imageRequest.ImageDescription} and size {imageRequest.ImageSize}");

                ChatGPTImageResponse? imageResponse = await ChatClient.CreateImageAsync(gptRequest);

                if (imageResponse?.Data is not null && imageResponse.Data.Count > 0 && imageResponse.Data[0].Url is not null)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    imageUrl = new Uri(imageResponse.Data[0].Url);
#pragma warning restore CS8604 // Possible null reference argument.
                }
            }
            catch (ChatGPTException chatEx)
            {
                exception = chatEx;
            }
            finally
            {
                if (loadingIndicator is not null)
                    await loadingIndicator.Hide();
            }
        }
    }
}
