using System;
using System.Text.Json.Serialization;

namespace weatherfrog.WeatherApi.Models
{
    public class Current : BaseModel
    {
        private int? lastUpdatedEpoch;
        private string lastUpdated;
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
        private double? feelsLikeC;
        private double? feelsLikeF;
        private double? visibilityKm;
        private double? visibilityMiles;
        private double? uv;
        private double? gustMph;
        private double? gustKph;

        /// <summary>
        /// Local time when the real time data was updated in unix time.
        /// </summary>
        [JsonPropertyName("last_updated_epoch")]
        public int? LastUpdatedEpoch { get => lastUpdatedEpoch; set => SetProperty(ref lastUpdatedEpoch, value); }

        /// <summary>
        /// Local time when the real time data was updated.
        /// </summary>
        [JsonPropertyName("last_updated")]
        public string LastUpdated { get => lastUpdated; set => SetProperty(ref lastUpdated, value); }

        /// <summary>
        /// Temperature in celsius.
        /// </summary>
        [JsonPropertyName("temp_c")]
        public double? TempC { get => tempC; set => SetProperty(ref tempC, value); }

        /// <summary>
        /// Temperature in fahrenheit.
        /// </summary>
        [JsonPropertyName("temp_f")]
        public double? TempF { get => tempF; set => SetProperty(ref tempF, value); }

        //public double Temp(UnitSystem unitSystem) => (unitSystem == UnitSystem.Imperial) ? TempF.Value : TempC.Value;

        public int Temp => (My.Settings.UnitSystem == UnitSystem.Imperial)
            ? (int)Math.Round(TempF.Value, MidpointRounding.AwayFromZero)
            : (int)Math.Round(TempC.Value, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Whether to show day condition icon or night icon.
        /// </summary>
        [JsonPropertyName("is_day"), JsonConverter(typeof(Utilities.ToBooleanConverter))]
        public bool? IsDay { get => isDay; set => SetProperty(ref isDay, value); }

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

        public int WindSpeed => (My.Settings.UnitSystem == UnitSystem.Imperial)
            ? (int)Math.Round(WindMph.Value, MidpointRounding.AwayFromZero)
            : (int)Math.Round(WindKph.Value, MidpointRounding.AwayFromZero);

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

        public int Pressure => (My.Settings.UnitSystem == UnitSystem.Imperial)
            ? (int)Math.Round(PressureIn.Value, MidpointRounding.AwayFromZero)
            : (int)Math.Round(PressureMb.Value, MidpointRounding.AwayFromZero);

        /// <summary>
        /// 	Precipitation amount in millimeters.
        /// </summary>
        [JsonPropertyName("precip_mm")]
        public double? PrecipMm { get => precipMm; set => SetProperty(ref precipMm, value); }

        /// <summary>
        /// Precipitation amount in inches.
        /// </summary>
        [JsonPropertyName("precip_in")]
        public double? PrecipIn { get => precipIn; set => SetProperty(ref precipIn, value); }

        public int Precip => (My.Settings.UnitSystem == UnitSystem.Imperial)
            ? (int)Math.Round(PrecipIn.Value, MidpointRounding.AwayFromZero)
            : (int)Math.Round(PrecipMm.Value, MidpointRounding.AwayFromZero);

        /// <summary>
        /// 	Humidity as percentage.
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
        public double? FeelsLikeC { get => feelsLikeC; set => SetProperty(ref feelsLikeC, value); }

        /// <summary>
        /// Feels like temperature as fahrenheit.
        /// </summary>
        [JsonPropertyName("feelslike_f")]
        public double? FeelsLikeF { get => feelsLikeF; set => SetProperty(ref feelsLikeF, value); }

        //public double FeelsLike(UnitSystem unitSystem) => (unitSystem == UnitSystem.Imperial) ? FeelslikeF.Value : FeelslikeC.Value;

        public int FeelsLike => (My.Settings.UnitSystem == UnitSystem.Imperial)
            ? (int)Math.Round(FeelsLikeF.Value, MidpointRounding.AwayFromZero)
            : (int)Math.Round(FeelsLikeC.Value, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Visibility in kilometer.
        /// </summary>
        [JsonPropertyName("vis_km")]
        public double? VisibilityKm { get => visibilityKm; set => SetProperty(ref visibilityKm, value); }

        /// <summary>
        /// Visibility in miles.
        /// </summary>
        [JsonPropertyName("vis_miles")]
        public double? VisibilityMiles { get => visibilityMiles; set => SetProperty(ref visibilityMiles, value); }

        public double Visibility => (My.Settings.UnitSystem == UnitSystem.Imperial)
            ? VisibilityMiles.Value
            : VisibilityKm.Value;

        /// <summary>
        /// UV Index.
        /// </summary>
        [JsonPropertyName("uv")]
        public double? UV { get => uv; set => SetProperty(ref uv, value); }

        /// <summary>
        /// Wind gust in miles per hour.
        /// </summary>
        [JsonPropertyName("gust_mph")]
        public double? GustMph { get => gustMph; set => SetProperty(ref gustMph, value); }

        /// <summary>
        /// Wind gust in kilometer per hour.
        /// </summary>
        [JsonPropertyName("gust_kph")]
        public double? GustKph { get => gustKph; set => SetProperty(ref gustKph, value); }

        public double Gust => (My.Settings.UnitSystem == UnitSystem.Imperial) ? GustMph.Value : GustKph.Value;

        [JsonPropertyName("condition")]
        public Condition Condition { get => condition; set => SetProperty(ref condition, value); }

        public System.Windows.Media.ImageSource WeatherIcon => Condition.GetWeatherIcon(IsDay.Value);

        public System.Windows.Media.Brush BackgroundBrush => Condition.GetBackgroundBrush(IsDay.Value);
    }
}
