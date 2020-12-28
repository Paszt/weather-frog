using System.Text.Json.Serialization;

namespace weatherfrog.WeatherApi.Models
{
    public class CurrentWeather : BaseModel
    {
        private Location location;
        private Current data;

        [JsonPropertyName("location")]
        public Location Location { get => location; set => SetProperty(ref location, value); }

        [JsonPropertyName("current")]
        public Current Data { get => data; set => SetProperty(ref data, value); }
    }
}
