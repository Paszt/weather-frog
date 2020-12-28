using System;
using System.Text.Json.Serialization;

namespace weatherfrog.WeatherApi.Models
{
    public partial class DailyWeatherData : BaseModel
    {
        private double? maxTempC;
        private double? maxTempF;
        private double? minTempC;
        private double? minTempF;
        private double? avgTempC;
        private double? avgTempF;
        private double? maxWindMph;
        private double? maxWindKph;
        private double? totalPrecipMm;
        private double? totalPrecipIn;
        private double? avgVisibilityKm;
        private double? avgVisibilityMiles;
        private double? avgHumidity;
        private bool? dailyWillItRain;
        private int? dailyChanceOfRain;
        private bool? dailyWillItSnow;
        private int? dailyChanceOfSnow;
        private Condition condition;
        private double? uv;

        /// <summary>
        /// Maximum temperature for the day in celsius.
        /// </summary>
        [JsonPropertyName("maxtemp_c")]
        public double? MaxTempC { get => maxTempC; set => SetProperty(ref maxTempC, value); }

        /// <summary>
        /// Maximum temperature for the day in fahrenheit.
        /// </summary>
        [JsonPropertyName("maxtemp_f")]
        public double? MaxTempF { get => maxTempF; set => SetProperty(ref maxTempF, value); }

        public int MaxTemp => (My.Settings.UnitSystem == UnitSystem.Imperial)
          ? (int)Math.Round(MaxTempF.Value, MidpointRounding.AwayFromZero)
          : (int)Math.Round(MaxTempC.Value, MidpointRounding.AwayFromZero);

        //public double MaxTemp(UnitSystem unitSystem) => (unitSystem == UnitSystem.Imperial) ? MaxTempF.Value : MaxTempC.Value;

        /// <summary>
        /// 	Minimum temperature for the day in celsius.
        /// </summary>
        [JsonPropertyName("mintemp_c")]
        public double? MinTempC { get => minTempC; set => SetProperty(ref minTempC, value); }

        /// <summary>
        /// Minimum temperature for the day in fahrenheit.
        /// </summary>
        [JsonPropertyName("mintemp_f")]
        public double? MinTempF { get => minTempF; set => SetProperty(ref minTempF, value); }

        public int MinTemp => (My.Settings.UnitSystem == UnitSystem.Imperial)
            ? (int)Math.Round(MinTempF.Value, MidpointRounding.AwayFromZero)
            : (int)Math.Round(MinTempC.Value, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Average temperature for the day in celsius.
        /// </summary>
        [JsonPropertyName("avgtemp_c")]
        public double? AvgTempC { get => avgTempC; set => SetProperty(ref avgTempC, value); }

        /// <summary>
        /// Average temperature for the day in fahrenheit.
        /// </summary>
        [JsonPropertyName("avgtemp_f")]
        public double? AvgTempF { get => avgTempF; set => SetProperty(ref avgTempF, value); }

        public int AvgTemp => (My.Settings.UnitSystem == UnitSystem.Imperial)
            ? (int)Math.Round(AvgTempF.Value, MidpointRounding.AwayFromZero)
            : (int)Math.Round(AvgTempC.Value, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Maximum wind speed for the day in miles per hour.
        /// </summary>
        [JsonPropertyName("maxwind_mph")]
        public double? MaxWindMph { get => maxWindMph; set => SetProperty(ref maxWindMph, value); }

        /// <summary>
        /// Maximum wind speed for the day in kilometers per hour.
        /// </summary>
        [JsonPropertyName("maxwind_kph")]
        public double? MaxWindKph { get => maxWindKph; set => SetProperty(ref maxWindKph, value); }

        public int MaxWind => (My.Settings.UnitSystem == UnitSystem.Imperial)
            ? (int)Math.Round(MaxWindMph.Value, MidpointRounding.AwayFromZero)
            : (int)Math.Round(MaxWindKph.Value, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Total precipitation for the day in milimeters.
        /// </summary>
        [JsonPropertyName("totalprecip_mm")]
        public double? TotalPrecipMm { get => totalPrecipMm; set => SetProperty(ref totalPrecipMm, value); }

        /// <summary>
        /// Total precipitation for the day in inches.
        /// </summary>
        [JsonPropertyName("totalprecip_in")]
        public double? TotalPrecipIn { get => totalPrecipIn; set => SetProperty(ref totalPrecipIn, value); }

        public int TotalPrecip => (My.Settings.UnitSystem == UnitSystem.Imperial)
            ? (int)Math.Round(TotalPrecipIn.Value, MidpointRounding.AwayFromZero)
            : (int)Math.Round(TotalPrecipMm.Value, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Average visibility for the day in kilometers.
        /// </summary>
        [JsonPropertyName("avgvis_km")]
        public double? AvgVisibilityKm { get => avgVisibilityKm; set => SetProperty(ref avgVisibilityKm, value); }

        /// <summary>
        /// Average visibility for the day in miles.
        /// </summary>
        [JsonPropertyName("avgvis_miles")]
        public double? AvgVisibilityMiles { get => avgVisibilityMiles; set => SetProperty(ref avgVisibilityMiles, value); }

        public int AvgVisibility => (My.Settings.UnitSystem == UnitSystem.Imperial)
            ? (int)Math.Round(AvgVisibilityMiles.Value, MidpointRounding.AwayFromZero)
            : (int)Math.Round(AvgVisibilityKm.Value, MidpointRounding.AwayFromZero);

        /// <summary>
        /// Average humidity for the day as percentage.
        /// </summary>
        [JsonPropertyName("avghumidity")]
        public double? AvgHumidity { get => avgHumidity; set => SetProperty(ref avgHumidity, value); }

        /// <summary>
        /// Will it will rain or not. 1 = Yes 0 = No.
        /// </summary>
        [JsonPropertyName("daily_will_it_rain"), JsonConverter(typeof(Utilities.ToBooleanConverter))]
        public bool? DailyWillItRain { get => dailyWillItRain; set => SetProperty(ref dailyWillItRain, value); }

        /// <summary>
        /// Chance of rain as percentage.
        /// </summary>
        [JsonPropertyName("daily_chance_of_rain"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public int? DailyChanceOfRain { get => dailyChanceOfRain; set => SetProperty(ref dailyChanceOfRain, value); }

        /// <summary>
        /// Will it snow or not. 1 = Yes 0 = No.
        /// </summary>
        [JsonPropertyName("daily_will_it_snow"), JsonConverter(typeof(Utilities.ToBooleanConverter))]
        public bool? DailyWillItSnow { get => dailyWillItSnow; set => SetProperty(ref dailyWillItSnow, value); }

        /// <summary>
        /// Chance of snow as percentage.
        /// </summary>
        [JsonPropertyName("daily_chance_of_snow"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public int? DailyChanceOfSnow { get => dailyChanceOfSnow; set => SetProperty(ref dailyChanceOfSnow, value); }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("condition")]
        public Condition Condition { get => condition; set => SetProperty(ref condition, value); }

        /// <summary>
        /// UV Index
        /// </summary>
        [JsonPropertyName("uv")]
        public double? UV { get => uv; set => SetProperty(ref uv, value); }

        public System.Windows.Media.ImageSource WeatherIcon => Condition?.GetWeatherIcon(true);

        public System.Windows.Media.Brush BackgroundBrush => Condition.GetBackgroundBrush(true);

        public string PrecipitationInfo
        {
            get
            {
                if (DailyWillItSnow == true)
                {
                    return DailyChanceOfSnow.ToString() + "% Snow";
                }
                else if (DailyWillItRain == true)
                {
                    return DailyChanceOfRain.ToString() + "% Precip";
                }
                //TODO: DailyWeatherDara.PrecipitationInfo: This temporary text for is for testing, need to removed.
                return "No Precip";
                //return string.Empty;
            }
        }

    }
}
