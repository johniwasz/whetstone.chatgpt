// SPDX-License-Identifier: MIT
using System.ComponentModel.DataAnnotations;
using Whetstone.ChatGPT.Models.Image;

namespace Whetstone.ChatGPT.Blazor.App.Models
{
    public class ImageRequest
    {
        [Required]
        public string? ImageDescription { get; set; }

        [Required]
        public CreatedImageSize ImageSize { get; set; } = CreatedImageSize.Size1024;
        
    }
}
