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

        internal static void Update(WeatherApi.Models.Forecast forecast)
        {
            ContainerVisual container = new()
            {
                Children = {
                    DrawBackground(forecast),
                    DrawFrogIllustration(forecast),
                    DrawText(forecast),
                    DrawWeatherIcon(forecast)}
            };
            SetDesktopWallpaper(container);
        }

        private static void SetDesktopWallpaper(Visual visual)
        {
            _ = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, SaveBitmap(visual), SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
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
            double screenWidth = SystemParameters.WorkArea.Width;
            double screenHeight = SystemParameters.WorkArea.Height;
            double leftTextLeft = screenWidth * 0.09;

            Typeface tf = Fonts.GetTypefaces(new Uri("pack://application:,,,/"), "./resources/").First();
            CultureInfo ci = new("en-us");

            // Temperature
            string temperature = forecast.CurrentWeather.Temp.ToString();
            FormattedText temperatureText = new(temperature, ci, FlowDirection.LeftToRight, tf, 120, Brushes.White, 1.0d);
            dc.DrawText(temperatureText, new Point(leftTextLeft, 120));
            string temperatureUnitsString = "°" + WeatherApi.Models.Forecast.TempUnitAbbreviated;
            FormattedText temperatureUnitsText = new(temperatureUnitsString, ci, FlowDirection.LeftToRight,
                tf, 80, Brushes.White, 1.0d);
            dc.DrawText(temperatureUnitsText, new Point(leftTextLeft + temperatureText.Width + 5, 130));

            // Feels Like
            string feelsLikeTemp = forecast.CurrentWeather.FeelsLike.ToString();
            FormattedText apparentTempText = new("Feels like " + feelsLikeTemp + "°", ci, FlowDirection.LeftToRight,
                tf, 40, Brushes.White, 1.0d);
            dc.DrawText(apparentTempText, new Point(leftTextLeft + 10, 300));

            // Weather Description
            double descriptionCenter = screenWidth * 0.77d + (weatherIconWidth / 2) - 150.0d;
            FormattedText descriptionText = new(forecast.CurrentWeather.Condition.Text, ci, FlowDirection.LeftToRight,
                tf, 40, Brushes.White, 1.0d)
            { TextAlignment = TextAlignment.Center, MaxTextWidth = 300.0d };
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
            Rect WeatherIconRect = new(new Point(SystemParameters.WorkArea.Width * 0.77, 97.0),
                                       new Size(weatherIconWidth, weatherIconWidth));
            dc.DrawImage(forecast.CurrentWeather.WeatherIcon, WeatherIconRect);
            dc.Close();
            return dv;
        }

        private static string SaveBitmap(Visual visual)
        {
            RenderTargetBitmap rtbmp = new((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight,
                96.0, 96.0, PixelFormats.Pbgra32);
            rtbmp.Render(visual);
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

        internal static void Offline(string message = "Weather Frog Is Offline")
        {
            const double maxTextWidth = 600.0d;
            DrawingVisual dv = new();
            using DrawingContext dc = dv.RenderOpen();
            dc.DrawRectangle(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9EABA2")), null, SystemParameters.WorkArea);
            FormattedText msgText = new(message, new CultureInfo("en-us"), FlowDirection.LeftToRight,
                Fonts.GetTypefaces(new Uri("pack://application:,,,/"), "./resources/").First(), 40, Brushes.White, 1.0d)
            { TextAlignment = TextAlignment.Center, MaxTextWidth = maxTextWidth };
            dc.DrawText(msgText, new Point((SystemParameters.WorkArea.Width / 2) - (maxTextWidth / 2), 120.0d));
            BitmapFrame bitmapFrame = BitmapFrame.Create(
                new Uri("pack://application:,,,/weather-frog;component/Resources/FrogIllustrations/SomethingWrong.png"));
            dc.DrawImage(bitmapFrame, GetFrogRectangle(bitmapFrame));
            dc.Close();
            SetDesktopWallpaper(dv);
        }

        private static Rect GetFrogRectangle(ImageSource imageSource)
        {
            double y = SystemParameters.WorkArea.Height / 3.0d;
            double height = SystemParameters.WorkArea.Height * (2.0d / 3.0d);
            double scale = height / imageSource.Height;
            double width = imageSource.Width * scale;
            double x = (SystemParameters.WorkArea.Width - width) / 2.0d;
            return new Rect(x, y, width, height);
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
