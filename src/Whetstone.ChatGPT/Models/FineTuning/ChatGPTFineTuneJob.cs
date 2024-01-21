﻿// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models.File;

namespace Whetstone.ChatGPT.Models.FineTuning
{

    /// <summary>
    /// This is the summary of the fine-tune job.
    /// </summary>
    /// <remarks>
    /// Use the Id to trace the fine-tune job progress. Once the job is complete, the Model property will be populated and should be used when creating a new <see cref="ChatGPTCompletionRequest"/>.
    /// </remarks>
    [DebuggerDisplay("Id = {Id}, FineTunedModel = {FineTunedModel}, Model = {Model}, Status = {Status}")]
    public class ChatGPTFineTuneJob
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("object")]
        [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This is the name of the property returned by the API.")]
        public string? @Object { get; set; }

        /// <summary>
        /// Name of the model used to generate this fine-tune job.
        /// </summary>
        /// <remarks>
        /// <see cref="ChatGPTCreateFineTuneRequest.Model">Model</see>  
        /// </remarks>
        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonConverter(typeof(UnixEpochTimeJsonConverter))]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Name of the completed model that can be used for completion requests.
        /// </summary>
        /// <remarks>
        /// Use this property when generating new completion requests.
        /// </remarks>
        [JsonPropertyName("fine_tuned_model")]
        public string? FineTunedModel { get; set; }

        [JsonPropertyName("hyperparameters")]
        public Hyperparameters? Hyperparameters { get; set; }

        [JsonPropertyName("organization_id")]
        public string? OrganizationId { get; set; }

        [JsonPropertyName("result_files")]
        public List<string>? ResultFiles { get; set; }

        /// <summary>
        /// Current status of the fine-tune job.
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }


        [JsonPropertyName("validation_file")]
        public string? ValidationFileId { get; set; }

        [JsonPropertyName("training_file")]
        public string? TrainingFileId { get; set; }

        [JsonConverter(typeof(UnixEpochTimeJsonConverter))]
        [JsonPropertyName("finished_at")]
        public DateTime? FinishedAt { get; set; }
    }


}