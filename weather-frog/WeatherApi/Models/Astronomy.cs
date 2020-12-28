using System.Text.Json.Serialization;

namespace weatherfrog.WeatherApi.Models
{
    public class Astronomy : BaseModel
    {
        private Location location;
        private AstronomyProps data;

        [JsonPropertyName("location")]
        public Location Location { get => location; set => SetProperty(ref location, value); }

        [JsonPropertyName("astronomy")]
        public AstronomyProps Data { get => data; set => SetProperty(ref data, value); }
    }
}
