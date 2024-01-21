using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Whetstone.ChatGPT.Models.FineTuning
{
    public class AutoFloatConverter : JsonConverter<float?>
    {
        public override float? Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        {
            if (reader.TokenType == System.Text.Json.JsonTokenType.Number)
            {
                return reader.GetSingle();
            }
            else if (reader.TokenType == System.Text.Json.JsonTokenType.String)
            {
                string? autoText = reader.GetString();

                if (string.IsNullOrEmpty(autoText))
                {
                    throw new System.Text.Json.JsonException("Invalid value for AutoFloatConverter");
                }

                if (autoText.Equals("auto", StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
            }

            throw new System.Text.Json.JsonException("Invalid value for AutoFloatConverter");

        }

        public override void Write(System.Text.Json.Utf8JsonWriter writer, float? value, System.Text.Json.JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteStringValue("auto");
            }
            else
            {
                writer.WriteNumberValue(value.Value);
            }
        }
    }
}
