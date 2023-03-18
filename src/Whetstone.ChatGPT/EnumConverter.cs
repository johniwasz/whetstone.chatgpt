using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Whetstone.ChatGPT
{
    public class EnumConverter<TEnum> : JsonConverter<TEnum>
        where TEnum : struct, Enum
    {
        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? stringValue = reader.GetString();

            if (stringValue != null)
            {
                return GetEnumValue(stringValue);
            }

            return default;
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.GetDescriptionFromEnumValue<TEnum>());
        }

        private static IEnumerable<TEnum> GetEnumValues()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }

        private static TEnum GetEnumValue(string enumMemberText)
        {
            TEnum retVal = default;

            if (Enum.TryParse(enumMemberText, true, out TEnum parsedVal))
                return parsedVal;

            var enumVals = GetEnumValues();

            Dictionary<string, TEnum> enumMemberNameMappings = new();

            foreach (TEnum enumVal in enumVals)
            {
                string? enumMember = enumVal.GetDescriptionFromEnumValue<TEnum>();

                if (enumMember is not null)
                    enumMemberNameMappings.Add(enumMember, enumVal);
            }

            if (enumMemberNameMappings.TryGetValue(enumMemberText, out TEnum foundVal))
            {
                retVal = foundVal;
            }
            else
                throw new JsonException($"Could not resolve value {enumMemberText} in enum {typeof(TEnum).FullName}");

            return retVal;
        }
    }

    internal static class EnumExtensions
    {
        internal const BindingFlags EnumBindings = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
        
        internal static string GetDescriptionFromEnumValue<TEnum>(this TEnum value)
            where TEnum : struct, Enum
        {
            string enumStringValue = value.ToString();
            
            Type enumType = value.GetType();
            FieldInfo field = enumType.GetField(enumStringValue, EnumBindings)!;

            if (field is not null)
            {
                var customAttributes = field.GetCustomAttributes(typeof(EnumMemberAttribute), false);
                if (customAttributes.Length > 0)
                {
                    var enumMemberAttribute = (EnumMemberAttribute)customAttributes[0];
                    enumStringValue = enumMemberAttribute.Value is null ? enumStringValue : enumMemberAttribute.Value;
                }
            }

            return enumStringValue;
        }
    }
}