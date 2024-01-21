// SPDX-License-Identifier: MIT
using System.ComponentModel;
using System.Text.Json.Serialization;
using Whetstone.ChatGPT.Models.File;

namespace Whetstone.ChatGPT.Models.FineTuning
{

    /// <summary>
    /// Defines a fine-tuning request. <see href="https://platform.openai.com/docs/api-reference/fine-tuning/create">Fine-tunes</see>.
    /// </summary>
    /// <remarks>
    /// <para>A <see cref="TrainingFileId"">TrainingFileID</see> is required.</para>
    /// </remarks>
    public class ChatGPTCreateFineTuneRequest
    {

        /// <summary>
        /// The name of the model to fine-tune. You can select one of the <see href="https://platform.openai.com/docs/guides/fine-tuning/what-models-can-be-fine-tuned">supported models</see>.
        /// </summary>
        /// <remarks>
        [JsonPropertyOrder(0)]
        [JsonPropertyName("model")]
        public string? Model { get; set; }

        /// <summary>
        /// The ID of an uploaded file that contains training data.
        /// </summary>
        /// <remarks>
        /// <para>See <see cref="ChatGPTClient.UploadFileAsync(ChatGPTUploadFileRequest?, CancellationToken?)"UploadFileAsync</see> for how to upload a file.</para>
        /// <para>Your dataset must be formatted as a JSONL file, where each training example is a JSON object with the keys "prompt" and "completion". Additionally, you must upload your file with the purpose <c>fine-tune</c>.</para>
        /// <para>See the <see href="https://beta.openai.com/docs/guides/fine-tuning/creating-training-data"/>fine-tuning guide</para> for more details.</para>
        /// </remarks>
        [JsonPropertyOrder(1)]
        [JsonPropertyName("training_file")]
        public string? TrainingFileId { get; set; }

        /// <summary>
        /// The hyperparameters used for the fine-tuning job.
        /// </summary>
        [JsonPropertyOrder(2)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("hyperparamters")]
        public ChatGPTFineTuneHyperparamters? Hyperparameters { get; set; }

        /// <summary>
        /// <para>A string of up to 18 characters that will be added to your fine-tuned model name.</para>
        /// <para>For example, a suffix of "custom-model-name" would produce a model name like <c>ft:gpt-3.5-turbo:openai:custom-model-name:7p4lURel</c>.</para>
        /// </summary>
        [JsonPropertyOrder(3)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("suffix")]
        public string? Suffix { get; set; }

        /// <summary>
        /// The ID of an uploaded file that contains validation data.
        /// </summary>
        /// <remarks>
        /// <para>If you provide this file, the data is used to generate validation metrics periodically during fine-tuning. These metrics can be viewed in the <see href="https://beta.openai.com/docs/guides/fine-tuning/analyzing-your-fine-tuned-model"/>fine-tuning results file<see>. Your train and validation data should be mutually exclusive.</para>
        /// <para>Your dataset must be formatted as a JSONL file, where each validation example is a JSON object with the keys "prompt" and "completion". Additionally, you must upload your file with the purpose <c>fine-tune</c>.</para>
        /// <para>See the <see href="https://beta.openai.com/docs/guides/fine-tuning/creating-training-data"/>fine-tuning guide</para> for more details.</para>
        /// </remarks>
        [JsonPropertyOrder(4)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("validation_file")]
        public string? ValidationFileId { get; set; }
    }

    public class ChatGPTFineTuneHyperparamters
    {

        /// <summary>
        /// The batch size to use for training. The batch size is the number of training examples used to train a single forward and backward pass.
        /// </summary>
        /// <remarks>
        /// By default, the batch size will be dynamically configured to be ~0.2% of the number of examples in the training set, capped at 256 - in general, we've found that larger batch sizes tend to work better for larger datasets.
        /// </remarks>  
        [JsonPropertyOrder(0)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("batch_size")]
        public int? BatchSize { get; set; }

        /// <summary>
        /// The learning rate multiplier to use for training. The fine-tuning learning rate is the original learning rate used for pretraining multiplied by this value.
        /// </summary>
        /// <remarks>
        /// By default, the learning rate multiplier is the 0.05, 0.1, or 0.2 depending on final <see cref="ChatGPTCreateFineTuneRequest.BatchSize">>BatchSize</see> (larger learning rates tend to perform better with larger batch sizes). We recommend experimenting with values in the range 0.02 to 0.2 to see what produces the best results.
        /// </remarks>
        [JsonPropertyOrder(1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("learning_rate_multiplier")]
        public float? LearningRateMultiplier { get; set; }

        /// <summary>
        /// The number of epochs to train the model for. An epoch refers to one full cycle through the training dataset.
        /// </summary>
        [JsonPropertyOrder(2)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("n_epochs")]
        public int? NumberOfEpochs { get; set; } = null;
    }
}