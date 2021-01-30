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

            // Location
            string location = forecast.Location.DisplayName;
            FormattedText locationText = new(location, ci, FlowDirection.LeftToRight, tf, 30, Brushes.Black, 1.0d);
            dc.DrawText(locationText, new Point(leftTextLeft + 12, 40));

            // Temperature
            string temperature = forecast.CurrentWeather.Temp.ToString();
            FormattedText temperatureText = new(temperature, ci, FlowDirection.LeftToRight, tf, 200,
                Brushes.White, 1.0d);
            dc.DrawText(temperatureText, new Point(leftTextLeft, 70));
            //Temp Units
            string temperatureUnitsString = "°" + WeatherApi.Models.Forecast.TempUnitAbbreviated;
            FormattedText temperatureUnitsText = new(temperatureUnitsString, ci, FlowDirection.LeftToRight,
                tf, 80, Brushes.White, 1.0d);
            dc.DrawText(temperatureUnitsText, new Point(leftTextLeft + temperatureText.Width + 10, 96));

            // Feels Like
            string feelsLikeTemp = forecast.CurrentWeather.FeelsLike.ToString();
            FormattedText apparentTempText = new("Feels like " + feelsLikeTemp + "°", ci, FlowDirection.LeftToRight,
                tf, 40, Brushes.White, 1.0d);
            dc.DrawText(apparentTempText, new Point(leftTextLeft + 10, 300));

            //Right Side Text
            double rightTextCenter = screenWidth * 0.77d + (weatherIconWidth / 2) - 150.0d;

            // Chance of precipitation (rain or snow)
            string precipInfo = forecast.Days.Forecastdays[0].WeatherData.PrecipitationInfo;
            if (!string.IsNullOrEmpty(precipInfo))
            {
                FormattedText precipInfoText = new(precipInfo, ci, FlowDirection.LeftToRight,
                    tf, 30, Brushes.Cyan, 1.0d)
                { TextAlignment = TextAlignment.Center, MaxTextWidth = 300.0d };
                dc.DrawText(precipInfoText, new Point(rightTextCenter + 25, 40));
                //Umbrella
                dc.DrawImage((ImageSource)Application.Current.FindResource("Umbrella"),
                    new Rect(rightTextCenter + (precipInfoText.Width / 2) - 10, 44, 24, 24));
            }

            // Weather Description
            FormattedText descriptionText = new(forecast.CurrentWeather.Condition.Text, ci, FlowDirection.LeftToRight,
                tf, 40, Brushes.White, 1.0d)
            { TextAlignment = TextAlignment.Center, MaxTextWidth = 300.0d };
            dc.DrawText(descriptionText, new Point(rightTextCenter, 300));


            dc.Close();
            return dv;
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
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wallpaper.bmp");
            using FileStream stm = File.Create(filePath);
            enc.Save(stm);
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            key.SetValue("WallpaperStyle", "1");
            key.SetValue("TileWallpaper", "0");

            return filePath;
        }

        internal static void NetworkError()
        {
            BitmapFrame bitmapFrame = BitmapFrame.Create(
                   new Uri("pack://application:,,,/weather-frog;component/Resources/FrogIllustrations/NetworkUnavailable.png"));
            DrawTextAndImage("Network currently unavailable", bitmapFrame);
        }

        internal static void Offline(string message = "Weather Frog Is Offline")
        {
            BitmapFrame bitmapFrame = BitmapFrame.Create(
                   new Uri("pack://application:,,,/weather-frog;component/Resources/FrogIllustrations/SomethingWrong.png"));
            DrawTextAndImage(message, bitmapFrame);
        }

        private static void DrawTextAndImage(string textToDraw, ImageSource imageSource)
        {
            const double maxTextWidth = 600.0d;
            DrawingVisual dv = new();
            using DrawingContext dc = dv.RenderOpen();
            dc.DrawRectangle(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9EABA2")), null, SystemParameters.WorkArea);
            FormattedText msgText = new(textToDraw, new CultureInfo("en-us"), FlowDirection.LeftToRight,
                Fonts.GetTypefaces(new Uri("pack://application:,,,/"), "./resources/").First(), 40, Brushes.White, 1.0d)
            { TextAlignment = TextAlignment.Center, MaxTextWidth = maxTextWidth };
            dc.DrawText(msgText, new Point((SystemParameters.WorkArea.Width / 2) - (maxTextWidth / 2), 120.0d));
            dc.DrawImage(imageSource, GetIllustrationRectangle(imageSource, AlignmentX.Center));
            dc.Close();
            SetDesktopWallpaper(dv);
        }

        private static Rect GetIllustrationRectangle(ImageSource imageSource, AlignmentX alignmentX = AlignmentX.Left)
        {
            double y = SystemParameters.WorkArea.Height / 3.0d;
            double height = SystemParameters.WorkArea.Height * (2.0d / 3.0d);
            double scale = height / imageSource.Height;
            double width = imageSource.Width * scale;
            double x = 0.0d;
            switch (alignmentX)
            {
                case AlignmentX.Center:
                    x = (SystemParameters.WorkArea.Width - width) / 2.0d;
                    break;
                case AlignmentX.Right:
                    x = SystemParameters.WorkArea.Width - width;
                    break;
            }
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
