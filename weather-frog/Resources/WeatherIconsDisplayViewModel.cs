using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace weatherfrog.Resources
{
    internal class WeatherIconsDisplayViewModel : WeatherApi.Models.BaseModel
    {
        private List<WeatherCondition> weatherConditions;
        public List<WeatherCondition> WeatherConditions { get => weatherConditions; set => SetProperty(ref weatherConditions, value); }

        public WeatherIconsDisplayViewModel()
        {
            //ResourceDictionary myResourceDictionary = new()
            //{
            //    Source = new Uri("/weather-frog;component/Resources/WeatherIcons.xaml",
            //            UriKind.RelativeOrAbsolute)
            //};

            //System.Collections.IDictionaryEnumerator enumerator = myResourceDictionary.GetEnumerator();
            //while (enumerator.MoveNext())
            //{
            //    var current = enumerator.Current;

            //}

            //foreach (System.Collections.DictionaryEntry resource in myResourceDictionary)
            //{

            //}

            //var res = Application.GetResourceStream(new Uri("Resources/weather_conditions.json", UriKind.Relative));
            //var jsonString = new StreamReader(res.Stream).ReadToEnd();
            ////var jsonString = File.ReadAllText( Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/weather_conditions.json"));
            //WeatherConditions = JsonSerializer.Deserialize<List<WeatherCondition>>(jsonString);

            //using HttpClient clientInstance = new(new WeatherApi.Utilities.RetryHandler(new SocketsHttpHandler()), false);
            //using HttpClient clientInstance = new();
            //weatherConditions = clientInstance.GetFromJsonAsync<List<WeatherCondition>>
            //    ("https://www.weatherapi.com/docs/weather_conditions.json").Result;

            Thread downloadThread = new Thread(async () =>
            {
                using HttpClient clientInstance = new();
                WeatherConditions = await clientInstance.GetFromJsonAsync<List<WeatherCondition>>
                    ("https://www.weatherapi.com/docs/weather_conditions.json");
            });
            downloadThread.Start();
        }
    }

    public class WeatherCondition
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("day")]
        public string Day { get; set; }

        [JsonPropertyName("night")]
        public string Night { get; set; }

        [JsonPropertyName("icon")]
        public int Icon { get; set; }

        public System.Windows.Media.ImageSource DayIcon => new WeatherApi.Models.Condition() { Code = Code }.GetWeatherIcon(true);

        public System.Windows.Media.ImageSource NightIcon => new WeatherApi.Models.Condition() { Code = Code }.GetWeatherIcon(false);

        public System.Windows.Media.Brush DayBrush => new WeatherApi.Models.Condition() { Code = Code }.GetBackgroundBrush(true);

        public System.Windows.Media.Brush NightBrush => new WeatherApi.Models.Condition() { Code = Code }.GetBackgroundBrush(false);
    }
}
