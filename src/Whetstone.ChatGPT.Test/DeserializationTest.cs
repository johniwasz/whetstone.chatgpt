using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Whetstone.ChatGPT.Models;
using Whetstone.ChatGPT.Models.FineTuning;

namespace Whetstone.ChatGPT.Test
{
    public class DeserializationTest
    {
        [Fact]
        public void DeserializeFineTuneJobResponse()
        {
            string responseMessage = @"Responses\tuningjobs.json";

            string fileContents = File.ReadAllText(responseMessage);

            var fineTuneJobResponse = JsonSerializer.Deserialize<ChatGPTListResponse<ChatGPTFineTuneJob>>(fileContents);

        }
    }
}
