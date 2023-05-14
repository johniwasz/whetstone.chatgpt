// SPDX-License-Identifier: MIT
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Whetstone.ChatGPT.Models
{

    /// <summary>
    /// Defines a fine-tuning request. <see href="https://beta.openai.com/docs/api-reference/fine-tunes/create">Fine-tunes</see>.
    /// </summary>
    /// <remarks>
    /// <para>A <see cref="TrainingFileId"">TrainingFileID</see> is required.</para>
    /// <para>The <see cref="Model">Model</see> defaults to <see cref="ChatGPTFineTuneModels.Ada">Ada</see>.</para>
    /// </remarks>
    public class ChatGPTCreateFineTuneRequest
    {
        /// <summary>
        /// The ID of an uploaded file that contains training data.
        /// </summary>
        /// <remarks>
        /// <para>See <see cref="ChatGPTClient.UploadFileAsync(ChatGPTUploadFileRequest?, CancellationToken?)"UploadFileAsync</see> for how to upload a file.</para>
        /// <para>Your dataset must be formatted as a JSONL file, where each training example is a JSON object with the keys "prompt" and "completion". Additionally, you must upload your file with the purpose <c>fine-tune</c>.</para>
        /// <para>See the <see href="https://beta.openai.com/docs/guides/fine-tuning/creating-training-data"/>fine-tuning guide</para> for more details.</para>
        /// </remarks>
        [JsonPropertyOrder(0)]
        [JsonPropertyName("training_file")]
        public string? TrainingFileId { get; set; }

        /// <summary>
        /// The ID of an uploaded file that contains validation data.
        /// </summary>
        /// <remarks>
        /// <para>If you provide this file, the data is used to generate validation metrics periodically during fine-tuning. These metrics can be viewed in the <see href="https://beta.openai.com/docs/guides/fine-tuning/analyzing-your-fine-tuned-model"/>fine-tuning results file<see>. Your train and validation data should be mutually exclusive.</para>
        /// <para>Your dataset must be formatted as a JSONL file, where each validation example is a JSON object with the keys "prompt" and "completion". Additionally, you must upload your file with the purpose <c>fine-tune</c>.</para>
        /// <para>See the <see href="https://beta.openai.com/docs/guides/fine-tuning/creating-training-data"/>fine-tuning guide</para> for more details.</para>
        /// </remarks>
        [JsonPropertyOrder(1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("validation_file")]
        public string? ValidationFileId { get; set; }

        /// <summary>
        /// The name of the base model to fine-tune. You can select one of "ada", "babbage", "curie", "davinci", or a fine-tuned model created after 2022-04-21. To learn more about these models, see the <see href="https://beta.openai.com/docs/models">Models</see> documentation.
        /// </summary>
        /// <remarks>
        /// <para>Use this property along with <see cref="ChatGPTFineTuneModels">ChatGPTFineTuneModels</see>.</para>
        /// <code>
        /// ChatGPTCreateFineTuneRequest fineTuneRequest = new ChatGPTCreateFineTuneRequest();
        /// fineTuneRequest.Model = ChatGPTFineTuneModels.Ada;
        /// </code>
        /// </remarks>
        [JsonPropertyOrder(2)]
        [JsonPropertyName("model")]
        public string? Model { get; set; }

        /// <summary>
        /// The number of epochs to train the model for. An epoch refers to one full cycle through the training dataset.
        /// </summary>
        [DefaultValue(4)]
        [JsonPropertyOrder(3)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("n_epochs")]
        public int NumberOfEpochs { get; set; } = 4;

        /// <summary>
        /// The batch size to use for training. The batch size is the number of training examples used to train a single forward and backward pass.
        /// </summary>
        /// <remarks>
        /// By default, the batch size will be dynamically configured to be ~0.2% of the number of examples in the training set, capped at 256 - in general, we've found that larger batch sizes tend to work better for larger datasets.
        /// </remarks>  
        [JsonPropertyOrder(4)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("batch_size")]
        public int? BatchSize { get; set; }

        /// <summary>
        /// The learning rate multiplier to use for training. The fine-tuning learning rate is the original learning rate used for pretraining multiplied by this value.
        /// </summary>
        /// <remarks>
        /// By default, the learning rate multiplier is the 0.05, 0.1, or 0.2 depending on final <see cref="ChatGPTCreateFineTuneRequest.BatchSize">>BatchSize</see> (larger learning rates tend to perform better with larger batch sizes). We recommend experimenting with values in the range 0.02 to 0.2 to see what produces the best results.
        /// </remarks>
        [JsonPropertyOrder(5)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("learning_rate_multiplier")]
        public float? LearningRateMultiplier { get; set; }

        /// <summary>
        /// If set, we calculate classification-specific metrics such as accuracy and F-1 score using the validation set at the end of every epoch. These metrics can be viewed in the <see href="https://beta.openai.com/docs/guides/fine-tuning/analyzing-your-fine-tuned-model">results file</see>.
        /// </summary>
        /// <remarks>
        /// In order to compute classification metrics, you must provide a <see cref="ChatGPTCreateFineTuneRequest.ValidationFileId">validation file</see>. Additionally, you must specify <see cref="ChatGPTCreateFineTuneRequest.NumberOfClasse">NumberOfClasse</see> for multiclass classification or <see cref="ChatGPTCreateFineTuneRequest.PositiveClas">PositiveClas</see> for binary classification.
        /// </remarks>
        [JsonPropertyOrder(6)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("compute_classification_metrics")]
        public bool ComputeClassificationMetrics { get; set; }

        /// <summary>
        /// <para>The number of classes in a classification task.</para>
        /// <para>This parameter is required for multiclass classification.</para>
        /// </summary>
        [JsonPropertyOrder(7)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("classification_n_classes")]
        public int? NumberOfClasses { get; set; }

        /// <summary>
        /// <para>The positive class in binary classification.</para>
        /// <para>This parameter is needed to generate precision, recall, and F1 metrics when doing binary classification.</para>
        /// </summary>
        [JsonPropertyOrder(8)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("classification_positive_class")]
        public string? PositiveClass { get; set; }

        /// <summary>
        /// <para>If this is provided, we calculate F-beta scores at the specified beta values. The F-beta score is a generalization of F-1 score. This is only used for binary classification.</para>        
        /// </summary>
        /// <remarks>
        /// With a beta of 1 (i.e.the F-1 score), precision and recall are given the same weight.A larger beta score puts more weight on recall and less on precision. A smaller beta score puts more weight on precision and less on recall.
        /// </remarks>
        [JsonPropertyOrder(9)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("classification_betas")]
        public List<float>? ClassificationBetas { get; set; }

        /// <summary>
        /// <para>A string of up to 40 characters that will be added to your fine-tuned model name.</para>
        /// <para>For example, a suffix of "custom-model-name" would produce a model name like <c>ada:ft-your-org:custom-model-name-2022-02-15-04-21-04.</c></para>
        /// </summary>
        [JsonPropertyOrder(10)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("suffix")]
        public string? Suffix { get; set; }
    }
}