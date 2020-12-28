using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;

// List of possible codes: https://www.weatherapi.com/docs/weather_conditions.json

namespace weatherfrog.WeatherApi.Models
{
    public class Condition : BaseModel
    {
        private string text;
        private string icon;
        private int? code;

        [JsonPropertyName("text")]
        public string Text { get => text; set => SetProperty(ref text, value); }

        [JsonPropertyName("icon")]
        public string Icon { get => icon; set => SetProperty(ref icon, value); }

        [JsonPropertyName("code")]
        public int? Code { get => code; set => SetProperty(ref code, value); }

        /// <summary>
        /// Gets the Background color for this weather condition.
        /// </summary>
        /// <param name="isDay">Boolean value indicating the time of day the requested color should represent, True for daytime.</param>
        /// <returns></returns>
        public string GetBackgroundColor(bool isDay)
        {
            if (conditions.TryGetValue(Code.Value, out Props props))
            {
                return isDay ? props.BackgroundColorDay : props.BackgroundColorNight;
            };
            return "#657492";
        }

        /// <summary>
        /// Gets a <see cref="SolidColorBrush"/> for this weather condition. 
        /// </summary>
        /// <param name="isDay">Boolean value indicating the time of day the requested color should represent, True for daytime.</param>
        /// <returns></returns>
        public SolidColorBrush GetBackgroundBrush(bool isDay)
        {
            string background = GetBackgroundColor(isDay);
            Color backgroundColor;
            try
            {
                backgroundColor = (Color)ColorConverter.ConvertFromString(background);
            }
            catch (Exception)
            {
                backgroundColor = (Color)ColorConverter.ConvertFromString("#657492");
            }
            return new SolidColorBrush(backgroundColor);
        }

        /// <summary>
        /// Gets a string representing the Icon name for this weather condition.
        /// </summary>
        /// <returns></returns>
        internal string GetIconName()
        {
            if (conditions.TryGetValue(Code.Value, out Props props)) { return props.IconName; };
            return "Unknown";
        }

        /// <summary>
        /// Get the cooresponding <see cref="ImageSource"/> for this weather condition.
        /// </summary>
        /// <param name="isDay">Boolean value indicating the time of day the requested color should represent, True for daytime.</param>
        /// <returns></returns>
        internal ImageSource GetWeatherIcon(bool isDay)
        {
            string iconName = GetIconName();
            ImageSource imgSource;
            // try without Day/Night suffix for icons that are used for both day & night
            imgSource = (ImageSource)Application.Current.TryFindResource(iconName);
            if (imgSource == null)
            {
                iconName += (isDay ? "Day" : "Night");
                imgSource = (ImageSource)Application.Current.TryFindResource(iconName);
                if (imgSource == null) { imgSource = (ImageSource)Application.Current.FindResource("Unknown"); }
            }
            return imgSource;
        }

        static readonly Dictionary<int, Props> conditions = new()
        {
            { 1000, new Props { BackgroundColorDay = "#19A3DF", BackgroundColorNight = "#824FFB", IconName = "Clear" } },
            { 1003, new Props { BackgroundColorDay = "#68AEDB", BackgroundColorNight = "#918EE5", IconName = "PartlyCloudy" } },
            { 1006, new Props { BackgroundColorDay = "#99B3D7", BackgroundColorNight = "#99B3D7", IconName = "Cloudy" } },
            { 1009, new Props { BackgroundColorDay = "#78ACD8", BackgroundColorNight = "#98A0E0", IconName = "MostlyCloudy" } },
            { 1030, new Props { BackgroundColorDay = "#BEB1AE", BackgroundColorNight = "#BEB1AE", IconName = "Mist" } },
            { 1063, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1066, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1069, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1072, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1087, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#92598E", IconName = "ScatteredThunderStorms" } },
            { 1114, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1117, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1135, new Props { BackgroundColorDay = "#BEB1AE", BackgroundColorNight = "#BEB1AE", IconName = "Haze" } },
            { 1147, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1150, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1153, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1168, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1171, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1180, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1183, new Props { BackgroundColorDay = "#2D88E7", BackgroundColorNight = "#86B3E2", IconName = "LightRain" } },
            { 1186, new Props { BackgroundColorDay = "#2D88E7", BackgroundColorNight = "#2D88E7", IconName = "Rain" } },
            { 1189, new Props { BackgroundColorDay = "#2D88E7", BackgroundColorNight = "#2D88E7", IconName = "Rain" } },
            { 1192, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1195, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1198, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1201, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1204, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1207, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1210, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1213, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1216, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1219, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1222, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1225, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1237, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1240, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1243, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1246, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1249, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1252, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1255, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1258, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1261, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1264, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1273, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#92598E", IconName = "ScatteredThunderStorms" } },
            { 1276, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1279, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } },
            { 1282, new Props { BackgroundColorDay = "#", BackgroundColorNight = "#", IconName = "" } }
        };

        class Props
        {
            public string BackgroundColorDay { get; set; }
            public string BackgroundColorNight { get; set; }
            public string IconName { get; set; }
        }

    }
}
