using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace weatherfrog.WeatherApi.Models
{
    public class ForecastDays : BaseModel
    {
        private List<Forecastday> forecastdays;

        [JsonPropertyName("forecastday")]
        public List<Forecastday> Forecastdays { get => forecastdays; set => SetProperty(ref forecastdays, value); }
    }
}
