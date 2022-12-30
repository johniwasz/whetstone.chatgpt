using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT
{
    public static class ModelExtensions
    {


        /// <summary>
        /// Returns the text of the first choice returned from an edit request.
        /// </summary>
        /// <param name="response">String or null.</param>
        /// <returns>Text from the first choice returned.</returns>
        public static string? GetCompletionText(this ChatGPTCompletionResponse response)
        {
            return response?.Choices?[0]?.Text;
        }


        /// <summary>
        /// Returns the text of the first choice returned from an edit request.
        /// </summary>
        /// <param name="response">String or null.</param>
        /// <returns>Text from the first choice returned.</returns>
        public static string? GetEditedText(this ChatGPTCreateEditResponse response)
        {
            return response?.Choices?[0]?.Text;
        }

    }
}
