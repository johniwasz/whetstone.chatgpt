using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models.FineTuning
{
    /// <summary>
    /// The hyperparameters used for the fine-tuning job.
    /// </summary>
    public class Hyperparameters
    {
        /// <summary>
        /// Number of examples in each batch. A larger batch size means that model parameters are updated less frequently, but with lower variance.
        /// </summary>
        [JsonConverter(typeof(AutoIntConverter))]
        [JsonPropertyName("batch_size")]
        public int? BatchSize { get; set; }

        /// <summary>
        /// Scaling factor for the learning rate. A smaller learning rate may be useful to avoid overfitting.
        /// </summary>
        [JsonConverter(typeof(AutoFloatConverter))]
        [JsonPropertyName("learning_rate_multiplier")]
        public float? LearningRateMultiplier { get; set; }

        /// <summary>
        /// The number of epochs to train the model for. An epoch refers to one full cycle through the training dataset.
        /// </summary>
        [JsonConverter(typeof(AutoIntConverter))]
        [JsonPropertyName("n_epochs")]
        public int? NumberOfEpochs { get; set; }

    }
}
