using System.Text.Json.Serialization;

namespace weatherfrog.Illustrations
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TimeOfDay
    {
        Any = 0,
        Day = 1,
        Night = 2
    }
}
