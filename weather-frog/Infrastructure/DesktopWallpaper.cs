using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace weatherfrog.Infrastructure
{
    sealed class DesktopWallpaper
    {
        private const int weatherIconWidth = 176;

        internal static void Update(WeatherApi.Models.Forecast fc)
        {
            ContainerVisual container = new()
            {
                Children = {
                    DrawBackground(fc),
                    DrawFrogIllustration(fc),
                    DrawText(fc),
                    DrawWeatherIcon(fc)}
            };
            _ = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, SaveBitmap(container), SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }

        private static DrawingVisual DrawBackground(WeatherApi.Models.Forecast forecast)
        {
            DrawingVisual dv = new();
            using DrawingContext dc = dv.RenderOpen();
            dc.DrawRectangle(forecast.CurrentWeather.BackgroundBrush, null, SystemParameters.WorkArea);
            dc.Close();
            return dv;
        }

        private static DrawingVisual DrawFrogIllustration(WeatherApi.Models.Forecast forecast)
        {
            DrawingVisual dv = new();
            using DrawingContext dc = dv.RenderOpen();

            dc.Close();
            return dv;
        }

        private static DrawingVisual DrawText(WeatherApi.Models.Forecast forecast)
        {
            DrawingVisual dv = new();
            using DrawingContext dc = dv.RenderOpen();
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double leftTextLeft = screenWidth * 0.09;

            Typeface tf = Fonts.GetTypefaces(new Uri("pack://application:,,,/"), "./resources/").First();
            CultureInfo ci = new("en-us");

            // Temperature
            string temperature = forecast.CurrentWeather.Temp.ToString();
            FormattedText temperatureText = new(temperature, ci, FlowDirection.LeftToRight, tf, 120, Brushes.White, 1.0d);
            dc.DrawText(temperatureText, new Point(leftTextLeft, 120));
            string temperatureUnitsString = "°" + ((My.Settings.UnitSystem == WeatherApi.Models.UnitSystem.Imperial) ? "F" : "C");
            FormattedText temperatureUnitsText = new(temperatureUnitsString, ci, FlowDirection.LeftToRight,
                tf, 80, Brushes.White, 1.0d);
            dc.DrawText(temperatureUnitsText, new Point(leftTextLeft + temperatureText.Width + 5, 130));

            // Feels Like
            string feelsLikeTemp = forecast.CurrentWeather.FeelsLike.ToString();
            FormattedText apparentTempText = new("Feels like " + feelsLikeTemp + "°", ci, FlowDirection.LeftToRight,
                tf, 40, Brushes.White, 1.0d);
            dc.DrawText(apparentTempText, new Point(leftTextLeft + 10, 300));

            // Weather Description
            double descriptionCenter = screenWidth * 0.77d + weatherIconWidth / 2;
            FormattedText descriptionText = new(forecast.CurrentWeather.Condition.Text, ci, FlowDirection.LeftToRight,
                tf, 40, Brushes.White, 1.0d)
            { TextAlignment = TextAlignment.Center };
            dc.DrawText(descriptionText, new Point(descriptionCenter, 300));
            dc.Close();
            return dv;

            // Chance of precipitation (rain or snow)


            // Location


        }

        private static DrawingVisual DrawWeatherIcon(WeatherApi.Models.Forecast forecast)
        {
            DrawingVisual dv = new();
            using DrawingContext dc = dv.RenderOpen();
            Rect WeatherIconRect = new(new Point(SystemParameters.PrimaryScreenWidth * 0.77, 97.0),
                                       new Size(weatherIconWidth, weatherIconWidth));
            dc.DrawImage(forecast.CurrentWeather.WeatherIcon, WeatherIconRect);
            dc.Close();
            return dv;
        }

        private static string SaveBitmap(ContainerVisual container)
        {
            RenderTargetBitmap rtbmp = new((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight,
                96.0, 96.0, PixelFormats.Pbgra32);
            rtbmp.Render(container);
            BmpBitmapEncoder enc = new();
            enc.Frames.Add(BitmapFrame.Create(rtbmp));
            string returnValue = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wallpaper.bmp");
            using FileStream stm = File.Create(returnValue);
            enc.Save(stm);
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            key.SetValue("WallpaperStyle", "1");
            key.SetValue("TileWallpaper", "0");

            return returnValue;
        }

        #region Win32

        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        #endregion 

    }
}
