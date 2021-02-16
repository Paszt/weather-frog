using System.Threading.Tasks;
using System.Windows;
using weatherfrog.WeatherApi.Models;
using weatherfrog.WeatherApi;

namespace weatherfrog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Icon = (System.Windows.Media.ImageSource)FindResource("FrogHeadDrawingImage");
            Configuration.ApiKey = "9265a223caff4f85bcd01600200312";
            Configuration.UserAgent = typeof(App).Assembly.GetName().Name + "/" +
                typeof(App).Assembly.GetName().Version.ToString() + " (https://github.com/Paszt/weather-frog)";
        }

        private static void DoTask()
        {
            //74.196.61.186
            IpLookup lookup = Task.Run(async () => await Api.LookupIpAsync("213.31.76.60")).Result;
            string IpAddress = lookup.Ip;

            CurrentWeather current = Task.Run(async () => await Api.GetCurrentWeatherAsync("Chocowinity, NC")).Result;
            double tempc = current.Data.TempC.Value;

            Forecast foreCast = Task.Run(async () => await Api.GetForecastAsync("Chocowinity, NC")).Result;
            ForecastDays fsa = foreCast.Days;

            Forecast historical = Task.Run(async () => await Api.GetHistoricalWeatherAsync("Christchurch", System.DateTime.Now.AddDays(-2))).Result;
            ForecastDays hds = historical.Days;

            Astronomy astro = Task.Run(async () => await Api.GetAtronomyAsync("Christchurch", System.DateTime.Now.AddDays(-2))).Result;
            AstronomyProps data = astro.Data;

            TimeZone timezone = Task.Run(async () => await Api.GetTimeZoneAsync("London")).Result;
            string localTime = timezone.Data.Localtime;

        }

        private void LookupButton_Click(object sender, RoutedEventArgs e) => DoTask();
    }
}
