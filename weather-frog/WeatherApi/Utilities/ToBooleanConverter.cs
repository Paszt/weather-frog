using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace weatherfrog.WeatherApi.Utilities
{
    class ToBooleanConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            reader.TokenType switch
            {
                JsonTokenType.String => bool.Parse(reader.GetString()),
                JsonTokenType.Number => reader.GetInt16() == 1,
                _ => true
            };

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) =>
            writer.WriteBooleanValue(value);
    }
}
