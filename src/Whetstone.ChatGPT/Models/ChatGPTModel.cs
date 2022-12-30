using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Whetstone.ChatGPT.Models
{

    [DebuggerDisplay("Id = {Id}, OwnedBy = {OwnedBy}, CreatedAt = {CreatedAt}")]
    public class ChatGPTModel
    {

        /// <summary>
        /// ID of the model. You can use the <see href="https://beta.openai.com/docs/api-reference/models/list">List models</see> API to see all of your available models, or see the GPT-3 <see href="https://beta.openai.com/docs/models/overview">Model overview</see> for descriptions of them.
        /// </summary>
        /// <remarks>
        /// <para>
        /// See <see cref="ChatGPTCompletionModels">ChatGPTCompletionModels</see> for recommended completion models.
        /// </para>
        /// <para>
        /// See <see cref="ChatGPTEditModels">ChatGPTEditModels</see> for recommended edit models.
        /// </para>
        /// </remarks>
        [JsonPropertyName("id")]
        public string? Id { get; set; }


        [JsonPropertyName("object")]
        [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This is the name of the property returned by the API.")]
        public string? @Object { get; set; }


        [JsonPropertyName("created")]
        public int Created { get; set; }

        [JsonPropertyName("owned_by")]
        public string? OwnedBy { get; set; }

        [JsonPropertyName("permission")]
        public List<ChatGPTModelPermissions>? Permission { get; set; }

        [JsonPropertyName("root")]
        public string? Root { get; set; }

        [JsonPropertyName("parent")]
        public string? Parent { get; set; }
    }
}
