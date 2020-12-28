using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace weatherfrog.WeatherApi.Models
{
    public class Forecastday : BaseModel
    {
        private DateTimeOffset? date;
        private long? dateEpoch;
        private DailyWeatherData weatherData;
        private AstronomyData astronomyData;
        private List<Hour> hourlyWeather;

        [JsonPropertyName("date")]
        public DateTimeOffset? Date { get => date; set => SetProperty(ref date, value); }

        public string DisplayDate
        {
            get
            {
                if (Date == DateTime.Today)
                {
                    return "Today";
                }
                else { return Date.Value.ToString("dddd, MMMM d"); }
            }
        }

        [JsonPropertyName("date_epoch")]
        public long? DateEpoch { get => dateEpoch; set => SetProperty(ref dateEpoch, value); }

        [JsonPropertyName("day")]
        public DailyWeatherData WeatherData { get => weatherData; set => SetProperty(ref weatherData, value); }

        [JsonPropertyName("astro")]
        public AstronomyData AstronomyData { get => astronomyData; set => SetProperty(ref astronomyData, value); }

        [JsonPropertyName("hour")]
        public List<Hour> HourlyWeather { get => hourlyWeather; set => SetProperty(ref hourlyWeather, value); }

    }
}
