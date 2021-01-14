//Alternatives to IHttpClientFactory:
// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.1#alternatives-to-ihttpclientfactory

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

//custom converters: https://github.com/dotnet/runtime/issues/29960#issuecomment-535166692

namespace weatherfrog.WeatherApi
{
    public static class API
    {

        private static readonly object syncObject = new object();
        private static HttpClient clientInstance = null;

        private static HttpClient ClientInstance
        {
            get
            {
                lock (syncObject)
                {
                    if (null == clientInstance)
                    {
                        // Retyrhandler retires on transient errors, 
                        // SocketsHttpHandler handles reusing sockets to avoid socket exhaustion and any DNS changes.
                        clientInstance = new(new Utilities.RetryHandler(new SocketsHttpHandler()), false);
                        clientInstance.DefaultRequestHeaders.Add("User-Agent", Configuration.UserAgent);
                    }
                    return clientInstance;
                }
            }
        }

        /// <summary>
        /// Queries the weather API and returns realtime weather information.
        /// </summary>
        /// <param name="q">Location query which can be US Zipcode, UK Postcode, Canada Postalcode, IP address, 
        /// Latitude/Longitude (decimal degree) or city name.</param>
        /// <returns></returns>
        public static async Task<Models.CurrentWeather> GetCurrentWeatherAsync(string q)
        {
            //using HttpClient httpClient = new(new SocketsHttpHandler(), false);
            return await ClientInstance.GetFromJsonAsync<Models.CurrentWeather>(string.Format(Urls.Current(q)));
        }

        /// <summary>
        /// Queries the weather API and returns hourly and daily weather forecast for up to 10 days, 
        /// including current weather and alerts.
        /// </summary>
        /// <param name="q">Location query which can be US Zipcode, UK Postcode, Canada Postalcode, IP address, 
        /// Latitude/Longitude (decimal degree) or city name.</param>
        /// <param name="days">Number of days of weather forecast. Value ranges from 1 to 10 (Free API returns 3 maximum).
        /// If not provided, the maximum number of days allowed by API pricing is returned.</param>
        /// <returns></returns>
        public static async Task<Models.Forecast> GetForecastAsync(string q, int days = 10)
        {
            return await ClientInstance.GetFromJsonAsync<Models.Forecast>(Urls.Forecast(q, days));
        }

        /// <summary>
        /// Queries the weather API and returns historical weather information (for the day and each hour) 
        /// for the specified location on the specified date, includes current weather.
        /// </summary>
        /// <param name="q">Location query which can be US Zipcode, UK Postcode, Canada Postalcode, IP address, 
        /// Latitude/Longitude (decimal degree) or city name.</param>
        /// <param name="dt">Date on or after 1st Jan, 2010 as a string in yyyy-MM-dd format 
        /// (The free API can only access the last 7 days).</param>
        /// <returns></returns>
        public static async Task<Models.Forecast> GetHistoricalWeatherAsync(string q, DateTime dt)
        {
            return await ClientInstance.GetFromJsonAsync<Models.Forecast>(Urls.Historical(q, dt));
        }

        /// <summary>
        /// Queries the API and returns up to date information for sunrise, sunset, moonrise, moonset, 
        /// moon phase and illumination for the specified location.
        /// </summary>
        /// <param name="q">Location query which can be US Zipcode, UK Postcode, Canada Postalcode, IP address, 
        /// Latitude/Longitude (decimal degree) or city name.</param>
        /// <param name="dt">Date as a string in yyyy-MM-dd format</param>
        /// <returns></returns>
        public static async Task<Models.Astronomy> GetAtronomyAsync(string q, DateTime dt)
        {
            return await ClientInstance.GetFromJsonAsync<Models.Astronomy>(Urls.Astronomy(q, dt));
        }

        /// <summary>
        /// Queries the API and returns up to date time zone and local time information for the specified location.
        /// </summary>
        /// <param name="q">Location query which can be US Zipcode, UK Postcode, Canada Postalcode, IP address, 
        /// Latitude/Longitude (decimal degree) or city name.</param>
        /// <returns></returns>
        public static async Task<Models.TimeZone> GetTimeZoneAsync(string q)
        {
            return await ClientInstance.GetFromJsonAsync<Models.TimeZone>(Urls.TimeZone(q));
        }

        /// <summary>
        /// Queries the weather API for location information for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">A string containing the value to looup.</param>
        /// <returns></returns>
        public static async Task<Models.IpLookup> LookupIpAsync(string ipAddress)
        {
            return await ClientInstance.GetFromJsonAsync<Models.IpLookup>(Urls.IpLookup(ipAddress));
        }

        public static async Task<List<Models.SearchResult>> LookupLocationAsync(string q)
        {
            return await ClientInstance.GetFromJsonAsync<List<Models.SearchResult>>(Urls.LocationLookup(q));
        }
    }
}
