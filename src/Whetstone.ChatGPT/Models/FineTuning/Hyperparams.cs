using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models.FineTuning
{
    public class Hyperparams
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
