using System;
using System.Text.Json.Serialization;

namespace weatherfrog.WeatherApi.Models
{
    public class Hour : BaseModel
    {
        private int? timeEpoch;
        private DateTime time;
        private double? tempC;
        private double? tempF;
        private bool? isDay;
        private Condition condition;
        private double? windMph;
        private double? windKph;
        private int? windDegree;
        private string windDir;
        private double? pressureMb;
        private double? pressureIn;
        private double? precipMm;
        private double? precipIn;
        private int? humidity;
        private int? cloud;
        private double? feelslikeC;
        private double? feelslikeF;
        private double? windchillC;
        private double? windchillF;
        private double? heatindexC;
        private double? heatindexF;
        private double? dewpointC;
        private double? dewpointF;
        private bool? willItRain;
        private int? chanceOfRain;
        private bool? willItSnow;
        private int? chanceOfSnow;
        private double? visKm;
        private double? visMiles;
        private double? gustMph;
        private double? gustKph;

        /// <summary>
        /// Time as epoch.
        /// </summary>
        [JsonPropertyName("time_epoch")]
        public int? TimeEpoch
        { get => timeEpoch; set => SetProperty(ref timeEpoch, value); }

        /// <summary>
        /// Date and time.
        /// </summary>
        [JsonPropertyName("time"), JsonConverter(typeof(Utilities.LocalDateTimeConverter))]
        public DateTime Time
        { get => time; set => SetProperty(ref time, value); }

        /// <summary>
        /// Temperature in celsius.
        /// </summary>
        [JsonPropertyName("temp_c")]
        public double? TempC
        { get => tempC; set => SetProperty(ref tempC, value); }

        /// <summary>
        /// Temperature in fahrenheit.
        /// </summary>
        [JsonPropertyName("temp_f")]
        public double? TempF { get => tempF; set => SetProperty(ref tempF, value); }

        public int Temp => (My.Settings.UnitSystem == UnitSystem.Imperial)
            ? (int)Math.Round(TempF.Value, MidpointRounding.AwayFromZero)
            : (int)Math.Round(TempC.Value, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Whether to show day condition icon or night icon. 1 = Yes 0 = No.
        /// </summary>
        [JsonPropertyName("is_day"), JsonConverter(typeof(Utilities.ToBooleanConverter))]
        public bool? IsDay { get => isDay; set => SetProperty(ref isDay, value); }

        [JsonPropertyName("condition")]
        public Condition Condition { get => condition; set => SetProperty(ref condition, value); }

        /// <summary>
        /// Wind speed in miles per hour.
        /// </summary>
        [JsonPropertyName("wind_mph")]
        public double? WindMph { get => windMph; set => SetProperty(ref windMph, value); }

        /// <summary>
        /// Wind speed in kilometer per hour.
        /// </summary>
        [JsonPropertyName("wind_kph")]
        public double? WindKph { get => windKph; set => SetProperty(ref windKph, value); }

        /// <summary>
        /// Wind direction in degrees.
        /// </summary>
        [JsonPropertyName("wind_degree")]
        public int? WindDegree { get => windDegree; set => SetProperty(ref windDegree, value); }

        /// <summary>
        /// Wind direction as 16 point compass. e.g.: NSW.
        /// </summary>
        [JsonPropertyName("wind_dir")]
        public string WindDir { get => windDir; set => SetProperty(ref windDir, value); }

        /// <summary>
        /// Pressure in millibars.
        /// </summary>
        [JsonPropertyName("pressure_mb")]
        public double? PressureMb { get => pressureMb; set => SetProperty(ref pressureMb, value); }

        /// <summary>
        /// Pressure in inches.
        /// </summary>
        [JsonPropertyName("pressure_in")]
        public double? PressureIn { get => pressureIn; set => SetProperty(ref pressureIn, value); }

        /// <summary>
        /// Precipitation amount in millimeters.
        /// </summary>
        [JsonPropertyName("precip_mm")]
        public double? PrecipMm { get => precipMm; set => SetProperty(ref precipMm, value); }

        /// <summary>
        /// Precipitation amount in inches.
        /// </summary>
        [JsonPropertyName("precip_in")]
        public double? PrecipIn { get => precipIn; set => SetProperty(ref precipIn, value); }

        /// <summary>
        /// Humidity as percentage.
        /// </summary>
        [JsonPropertyName("humidity")]
        public int? Humidity { get => humidity; set => SetProperty(ref humidity, value); }

        /// <summary>
        /// Cloud cover as percentage.
        /// </summary>
        [JsonPropertyName("cloud")]
        public int? Cloud { get => cloud; set => SetProperty(ref cloud, value); }

        /// <summary>
        /// Feels like temperature as celcius.
        /// </summary>
        [JsonPropertyName("feelslike_c")]
        public double? FeelslikeC { get => feelslikeC; set => SetProperty(ref feelslikeC, value); }

        /// <summary>
        /// Feels like temperature as fahrenheit.
        /// </summary>
        [JsonPropertyName("feelslike_f")]
        public double? FeelslikeF { get => feelslikeF; set => SetProperty(ref feelslikeF, value); }

        /// <summary>
        /// Windchill temperature in celcius.
        /// </summary>
        [JsonPropertyName("windchill_c")]
        public double? WindchillC { get => windchillC; set => SetProperty(ref windchillC, value); }

        /// <summary>
        /// Windchill temperature in fahrenheit.
        /// </summary>
        [JsonPropertyName("windchill_f")]
        public double? WindchillF { get => windchillF; set => SetProperty(ref windchillF, value); }

        /// <summary>
        /// Heat index in celcius.
        /// </summary>
        [JsonPropertyName("heatindex_c")]
        public double? HeatindexC { get => heatindexC; set => SetProperty(ref heatindexC, value); }

        /// <summary>
        /// Heat index in fahrenheit.
        /// </summary>
        [JsonPropertyName("heatindex_f")]
        public double? HeatindexF { get => heatindexF; set => SetProperty(ref heatindexF, value); }

        /// <summary>
        /// Dew point in celcius.
        /// </summary>
        [JsonPropertyName("dewpoint_c")]
        public double? DewpointC { get => dewpointC; set => SetProperty(ref dewpointC, value); }

        /// <summary>
        /// Dew point in fahrenheit.
        /// </summary>
        [JsonPropertyName("dewpoint_f")]
        public double? DewpointF { get => dewpointF; set => SetProperty(ref dewpointF, value); }

        /// <summary>
        /// Will it will rain or not. 1 = Yes 0 = No.
        /// </summary>
        [JsonPropertyName("will_it_rain"), JsonConverter(typeof(Utilities.ToBooleanConverter))]
        public bool? WillItRain { get => willItRain; set => SetProperty(ref willItRain, value); }

        /// <summary>
        /// Chance of rain as percentage.
        /// </summary>
        [JsonPropertyName("chance_of_rain")]
        public int? ChanceOfRain { get => chanceOfRain; set => SetProperty(ref chanceOfRain, value); }

        /// <summary>
        /// Will it snow or not. 1 = Yes 0 = No.
        /// </summary>
        [JsonPropertyName("will_it_snow"), JsonConverter(typeof(Utilities.ToBooleanConverter))]
        public bool? WillItSnow { get => willItSnow; set => SetProperty(ref willItSnow, value); }

        /// <summary>
        /// Chance of snow as percentage.
        /// </summary>
        [JsonPropertyName("chance_of_snow")]
        public int? ChanceOfSnow { get => chanceOfSnow; set => SetProperty(ref chanceOfSnow, value); }

        /// <summary>
        /// Visibility in kilometers.
        /// </summary>
        [JsonPropertyName("vis_km")]
        public double? VisibilityKm { get => visKm; set => SetProperty(ref visKm, value); }

        /// <summary>
        /// Visibility in miles.
        /// </summary>
        [JsonPropertyName("vis_miles")]
        public double? VisibilityMiles { get => visMiles; set => SetProperty(ref visMiles, value); }

        /// <summary>
        /// Wind gust in miles per hour.
        /// </summary>
        [JsonPropertyName("gust_mph")]
        public double? GustMph { get => gustMph; set => SetProperty(ref gustMph, value); }

        /// <summary>
        /// Wind gust in kilometers per hour.
        /// </summary>
        [JsonPropertyName("gust_kph")]
        public double? GustKph { get => gustKph; set => SetProperty(ref gustKph, value); }

        public System.Windows.Media.ImageSource WeatherIcon => Condition.GetWeatherIcon(IsDay.Value);

        public System.Windows.Media.Brush BackgroundBrush => Condition.GetBackgroundBrush(IsDay.Value);
    }
}
