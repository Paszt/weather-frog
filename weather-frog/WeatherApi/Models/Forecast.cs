using System.Text.Json.Serialization;

namespace weatherfrog.WeatherApi.Models
{
    public partial class Forecast : BaseModel
    {
        private Location location;
        private Current current;
        private ForecastDays days;
        private Alert alert;

        [JsonPropertyName("location")]
        public Location Location { get => location; set => SetProperty(ref location, value); }

        [JsonPropertyName("current")]
        public Current CurrentWeather { get => current; set => SetProperty(ref current, value); }

        [JsonPropertyName("forecast")]
        public ForecastDays Days { get => days; set => SetProperty(ref days, value); }

        [JsonPropertyName("alert")]
        public Alert Alert { get => alert; set => SetProperty(ref alert, value); }

        [JsonIgnore]
        public static string TempUnitAbbreviated => (My.Settings.UnitSystem == UnitSystem.Imperial) ? "F" : "C";

        [JsonIgnore]
        public static string WindUnitAbbreviated => (My.Settings.UnitSystem == UnitSystem.Imperial) ? "mph" : "kph";
    }
}
