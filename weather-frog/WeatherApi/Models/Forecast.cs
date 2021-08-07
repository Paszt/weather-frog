using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace weatherfrog.WeatherApi.Models
{
    public partial class Forecast : BaseModel
    {
        private Location location;
        private Current currentWeather;
        private ForecastDays days;
        private Alerts alerts;

        [JsonPropertyName("location")]
        public Location Location { get => location; set => SetProperty(ref location, value); }

        [JsonPropertyName("current")]
        public Current CurrentWeather { get => currentWeather; set => SetProperty(ref currentWeather, value); }

        [JsonPropertyName("forecast")]
        public ForecastDays Days { get => days; set => SetProperty(ref days, value); }

        [JsonPropertyName("alerts")]
        public Alerts Alerts { get => alerts; set => SetProperty(ref alerts, value); }

        [JsonIgnore]
        public static string TempUnitAbbreviated => (My.Settings.UnitSystem == UnitSystem.Imperial) ? "F" : "C";

        [JsonIgnore]
        public static string WindUnitAbbreviated => (My.Settings.UnitSystem == UnitSystem.Imperial) ? "mph" : "kph";
    }
}
