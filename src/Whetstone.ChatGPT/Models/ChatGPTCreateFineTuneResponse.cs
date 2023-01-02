using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models
{
    public class ChatGPTCreateFineTuneResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("object")]
        [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This is the name of the property returned by the API.")]
        public string? @Object { get; set; }

        /// <summary>
        /// Model used to generate the original request. 
        /// </summary>
        /// <remarks>
        /// See <see cref="Models.ChatGPTCreateFineTuneRequest.Model">Fine Tune Model</see> for recommended completion models.
        /// </remarks>
        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonConverter(typeof(UnixEpochTimeJsonConverter))]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }


        [JsonPropertyName("events")]
        public List<ChatGPTEvent>? Events { get; set; }

        
        [JsonPropertyName("fine_tuned_model")]
        public object? FineTunedModel { get; set; }

        [JsonPropertyName("hyperparams")]
        public HyperParams? HyperParams { get; set; }

        [JsonPropertyName("organization_id")]
        public string? OrganizationId { get; set; }

        [JsonPropertyName("result_files")]
        public List<ChatGPTFileInfo>? ResultFiles { get; set; }
        

        [JsonPropertyName("status")]
        public string? Status { get; set; }


        [JsonPropertyName("validation_files")]
        public List<ChatGPTFileInfo>? ValidationFiles { get; set; }

        [JsonPropertyName("training_files")]
        public List<ChatGPTFileInfo>? TrainingFiles { get; set; }

        [JsonConverter(typeof(UnixEpochTimeJsonConverter))]
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

    }

    public class HyperParams
    {
        [JsonPropertyName("batch_size")]
        public int? BatchSize { get; set; }
        
        [JsonPropertyName("learning_rate_multiplier")]
        public float? LearningRateMultiplier { get; set; }
        
        [JsonPropertyName("n_epochs")]
        public int NumberOfEpochs { get; set; }

        [JsonPropertyName("prompt_loss_weight")]
        public float PromptLossWeight { get; set; }
    }

}
