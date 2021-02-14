using System.Text.Json.Serialization;

namespace weatherfrog.Illustrations
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum WeatherCondition
    {
        Any,
        Fog,
        Rain,
        Sleet,
        Snow,
        Sunny,
        Vog
    }
}
