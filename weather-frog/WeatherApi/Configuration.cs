using System;

namespace weatherfrog.WeatherApi
{
    public partial class Configuration
    {
        /// <summary>
        /// The base Url for API calls
        /// </summary>
        public static string BaseUrl { get; set; } = "https://api.weatherapi.com/v1/";

        /// <summary>
        /// The weatherapi.com API key. Go to https://www.weatherapi.com/signup.aspx to sign up for a key.
        /// </summary>
        public static string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// String value sent in the user-agent header when making requests against the API web service.
        /// </summary>
        public static string UserAgent { get; set; } = "WeatherApiNet/1.0.0";
    }

}
