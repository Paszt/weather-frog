using System;

namespace weatherfrog.WeatherApi
{
    class Urls
    {

        public static string Astronomy(string q, DateTime dt) => Configuration.BaseUrl +
            $"astronomy.json?key={Configuration.ApiKey}&q={Uri.EscapeDataString(q)}&dt={dt:yyyy-MM-dd}";

        public static string Current(string q) => Configuration.BaseUrl +
            $"current.json?key={Configuration.ApiKey}&q={Uri.EscapeDataString(q)}";

        public static string Forecast(string q, int days = 10, bool alerts = true) => Configuration.BaseUrl +
            $"forecast.json?key={Configuration.ApiKey}&q={Uri.EscapeDataString(q)}&days={days}&aqi=no" +
            $"&alerts={(alerts == true ? "yes" : "no")}";

        public static string Historical(string q, DateTime dt) => Configuration.BaseUrl +
            $"history.json?key={Configuration.ApiKey}&q={Uri.EscapeDataString(q)}&dt={dt:yyyy-MM-dd}";

        public static string TimeZone(string q) => Configuration.BaseUrl +
            $"timezone.json?key={Configuration.ApiKey}&q={Uri.EscapeDataString(q)}";

        public static string IpLookup(string ipAddress) => Configuration.BaseUrl +
            $"ip.json?key={Configuration.ApiKey}&q={ipAddress}";

        public static string LocationLookup(string q) => Configuration.BaseUrl +
            $"search.json?key={Configuration.ApiKey}&q={Uri.EscapeDataString(q)}";
    }
}
