using System;

namespace weatherfrog.WeatherApi
{
    class Urls
    {

        public static string Astronomy(string q, DateTime dt)
        {
            return string.Format(Configuration.BaseUrl +
                "astronomy.json?key={0}&q={1}&dt={2}", Configuration.ApiKey, Uri.EscapeDataString(q), dt.ToString("yyyy-MM-dd"));
        }

        public static string Current(string q)
        {
            return string.Format(Configuration.BaseUrl + 
                "current.json?key={0}&q={1}", Configuration.ApiKey, Uri.EscapeDataString(q));
        }

        public static string Forecast(string q, int days = 10)
        {
            return string.Format(Configuration.BaseUrl + 
                "forecast.json?key={0}&q={1}&days={2}", Configuration.ApiKey, Uri.EscapeDataString(q), days);
        }

        public static string Historical(string q, DateTime dt)
        {
            return string.Format(Configuration.BaseUrl +
                "history.json?key={0}&q={1}&dt={2}", Configuration.ApiKey, Uri.EscapeDataString(q), dt.ToString("yyyy-MM-dd"));
        }

        public static string TimeZone(string q)
        {
            return string.Format(Configuration.BaseUrl +
                "timezone.json?key={0}&q={1}", Configuration.ApiKey, Uri.EscapeDataString(q));
        }

        public static string IpLookup(string ipAddress)
        {
             return string.Format(Configuration.BaseUrl +
                "ip.json?key={0}&q={1}", Configuration.ApiKey, Uri.EscapeDataString(ipAddress));
        }

        public static string LocationLookup(string q)
        {
            return string.Format(Configuration.BaseUrl +
                "search.json?key={0}&q={1}", Configuration.ApiKey, Uri.EscapeDataString(q));
        }
    }
}
