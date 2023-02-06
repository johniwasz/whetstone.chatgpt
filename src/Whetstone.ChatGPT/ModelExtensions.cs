using System;
using System.Collections.Generic;
using System.Globalization;
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



        /// <summary>
        /// Returns text formatted according to JsonL
        /// </summary>
        /// <param name="tuningLines">List of prompts and completions for fine tuning.</param>
        /// <returns>JsonL string</returns>
        public static string ToJsonL(this IEnumerable<ChatGPTFineTuneLine> tuningLines)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var line in tuningLines)
            {
                builder.AppendLine(System.Text.Json.JsonSerializer.Serialize(line));
            }

            return builder.ToString();
        }

        public static byte[] ToJsonLBinary(this IEnumerable<ChatGPTFineTuneLine> tuningLines)
        {
            return Encoding.UTF8.GetBytes(tuningLines.ToJsonL());
        }

    }
}
