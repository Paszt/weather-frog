using System.Text.Json.Serialization;

namespace weatherfrog.WeatherApi.Models
{
    public class AstronomyData : BaseModel
    {
        private string sunrise;
        private string sunset;
        private string moonrise;
        private string moonset;
        private string moonPhase;
        private double? moonIllumination;

        /// <summary>
        /// Sunrise time.
        /// </summary>
        [JsonPropertyName("sunrise")]
        public string Sunrise { get => sunrise; set => SetProperty(ref sunrise, value); }

        /// <summary>
        /// Sunset time.
        /// </summary>
        [JsonPropertyName("sunset")]
        public string Sunset { get => sunset; set => SetProperty(ref sunset, value); }

        /// <summary>
        /// Moonrise time.
        /// </summary>
        [JsonPropertyName("moonrise")]
        public string Moonrise { get => moonrise; set => SetProperty(ref moonrise, value); }

        /// <summary>
        /// Moonset time.
        /// </summary>
        [JsonPropertyName("moonset")]
        public string Moonset { get => moonset; set => SetProperty(ref moonset, value); }

        /// <summary>
        /// Phase of the moon.
        /// </summary>
        [JsonPropertyName("moon_phase")]
        public string MoonPhase { get => moonPhase; set => SetProperty(ref moonPhase, value); }

        /// <summary>
        /// Moon illumination as percent.
        /// </summary>
        [JsonPropertyName("moon_illumination")]
        public double? MoonIllumination { get => moonIllumination; set => SetProperty(ref moonIllumination, value); }
    }
}
