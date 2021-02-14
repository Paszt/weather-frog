using System.Text.Json.Serialization;

namespace weatherfrog.Illustrations
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TimeOfDay
    {
        Any,
        Day,
        Night
    }
}
