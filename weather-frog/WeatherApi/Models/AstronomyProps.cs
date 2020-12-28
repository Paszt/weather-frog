using System.Text.Json.Serialization;

namespace weatherfrog.WeatherApi.Models
{
    public class AstronomyProps : BaseModel
    {
        private AstronomyData astro;

        [JsonPropertyName("astro")]
        public AstronomyData Astro { get => astro; set => SetProperty(ref astro, value); }
    }
}
