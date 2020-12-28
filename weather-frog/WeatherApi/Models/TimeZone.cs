using System.Text.Json.Serialization;

namespace weatherfrog.WeatherApi.Models
{
    public class TimeZone : BaseModel
    {
        private Location data;

        [JsonPropertyName("location")]
        public Location Data { get => data; set => SetProperty(ref data, value); }
    }
}
