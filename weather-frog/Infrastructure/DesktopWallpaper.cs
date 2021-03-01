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
        private readonly CultureInfo cultureInfo = new("en-us");
        private readonly Typeface typeface;

        public double Width { get; set; }
        public double Height { get; set; }
        public Rect WorkArea { get; set; }
        public bool DrawTaskbar { get; set; } = false;

        public DesktopWallpaper(double width, double height, Rect workArea)
        {
            // Design time can't reference the font...
            try
            {
                typeface = Fonts.GetTypefaces(new Uri("pack://application:,,,/"), "./resources/").First();
            }
            catch (Exception)
            {
                typeface = Fonts.SystemTypefaces.First();
            }
            Width = width;
            Height = height;
            WorkArea = workArea;
        }

        public static DesktopWallpaper FromSystemParameters() =>
            new(SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight, SystemParameters.WorkArea);

        private Visual CreateVisual(WeatherApi.Models.Forecast forecast) => new ContainerVisual()
        {
            Children = {DrawBackground(forecast),
                        DrawClouds(forecast),
                        DrawFrogIllustration(forecast),
                        DrawText(forecast),
                        DrawWeatherIcon(forecast)}
        };

        public RenderTargetBitmap CreateBitmap(WeatherApi.Models.Forecast forecast) =>
            CreateBitmap(CreateVisual(forecast));

        private RenderTargetBitmap CreateBitmap(Visual visual)
        {
            RenderTargetBitmap rtbmp = new((int)Width, (int)Height, 96.0, 96.0, PixelFormats.Pbgra32);
            rtbmp.Render(visual);
            return rtbmp;
        }

        internal void Update(WeatherApi.Models.Forecast forecast, bool UpdateDimensionsFromSystemParameters = false)
        {
            if (UpdateDimensionsFromSystemParameters)
                UpdateDimensions(SystemParameters.PrimaryScreenWidth,
                                 SystemParameters.PrimaryScreenHeight,
                                 SystemParameters.WorkArea);
            SetDesktopWallpaper(CreateVisual(forecast));
        }

        internal void Update(WeatherApi.Models.Forecast forecast, double width, double height, Rect workArea)
        {
            UpdateDimensions(width, height, workArea);
            Update(forecast);
        }

        internal void UpdateDimensions(double width, double height, Rect workArea)
        {
            Width = width;
            Height = height;
            WorkArea = workArea;
        }

        private void SetDesktopWallpaper(Visual visual) =>
            _ = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, SaveBitmap(visual),
                                     SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

        private DrawingVisual DrawBackground(WeatherApi.Models.Forecast forecast)
        {
            DrawingVisual dv = new();
            using DrawingContext dc = dv.RenderOpen();
            dc.DrawRectangle(forecast?.CurrentWeather?.BackgroundBrush, null, new(0, 0, Width, Height));
            if (DrawTaskbar)
            {
                Rect taskbarRect = new();
                // TODO: Draw Taskbar
                if (Width == WorkArea.Width)
                {
                    taskbarRect.Width = Width;
                    taskbarRect.Height = Height - WorkArea.Height;
                    taskbarRect.X = 0;
                    taskbarRect.Y = 0;
                    if (WorkArea.Y == 0)
                    {
                        taskbarRect.Y = WorkArea.Height;
                    }
                }
                else
                {
                    taskbarRect.Width = Width - WorkArea.Width;
                    taskbarRect.Height = Height;
                    taskbarRect.Y = 0;
                    taskbarRect.X = 0;
                    if (WorkArea.X == 0)
                    {
                        taskbarRect.X = WorkArea.Width;
                    }
                }
                dc.DrawRectangle(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#99000000")), null, taskbarRect);
            }
            dc.Close();
            return dv;
        }

        private DrawingVisual DrawClouds(WeatherApi.Models.Forecast forecast)
        {
            if (forecast.CurrentWeather.Cloud == 0) return null;
            string cloudFilename = "clouds" +
                ((int)(Math.Round(forecast.CurrentWeather.Cloud.Value / 10.0d, MidpointRounding.ToPositiveInfinity) * 10)).ToString("d3") +
                ".png";
            BitmapFrame bitmapFrame;
            try
            {
                bitmapFrame = BitmapFrame.Create(
                    new Uri("pack://application:,,,/weather-frog;component/Resources/Clouds/" + cloudFilename));
            }
            catch (Exception) { return null; }

            DrawingVisual dv = new();
            using DrawingContext dc = dv.RenderOpen();
            dc.DrawImage(bitmapFrame, GetIllustrationRectangle(bitmapFrame, AlignmentX.Center));
            dc.Close();
            return dv;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Work Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0022:Use expression body for methods", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        private DrawingVisual DrawFrogIllustration(WeatherApi.Models.Forecast forecast)
        {
            //DrawingVisual dv = new();
            //using DrawingContext dc = dv.RenderOpen();
            ////TODO: Draw Frog Illustration on Wallpaper.
            //Debug.WriteLine(forecast);
            //dc.Close();
            //return dv;
            return null;
        }

        private DrawingVisual DrawText(WeatherApi.Models.Forecast forecast)
        {
            // TODO: DesktopWallpaper.DrawText(): Instead of hardcoding values (text size, locations, etc), calculate them based on Width, Height & WorkArea properties.
            DrawingVisual dv = new();
            using DrawingContext dc = dv.RenderOpen();

            //double screenWidth = SystemParameters.WorkArea.Width;
            //double screenHeight = SystemParameters.WorkArea.Height;
            double leftTextLeft = (WorkArea.Width * 0.09) + WorkArea.Left;

            // Location
            string location = forecast.Location.DisplayName;
            FormattedText locationText = new(location, cultureInfo, FlowDirection.LeftToRight, typeface, 30, Brushes.Black, 1.0d);
            dc.DrawText(locationText, new Point(leftTextLeft + 12, 40.0d + WorkArea.Top));

            // Temperature
            string temperature = forecast.CurrentWeather.Temp.ToString();
            FormattedText temperatureText = new(temperature, cultureInfo, FlowDirection.LeftToRight, typeface, 200,
                Brushes.White, 1.0d);
            dc.DrawText(temperatureText, new Point(leftTextLeft, 70 + WorkArea.Top));
            //Temp Units
            string temperatureUnitsString = "°" + WeatherApi.Models.Forecast.TempUnitAbbreviated;
            FormattedText temperatureUnitsText = new(temperatureUnitsString, cultureInfo, FlowDirection.LeftToRight,
                typeface, 80, Brushes.White, 1.0d);
            dc.DrawText(temperatureUnitsText, new Point(leftTextLeft + temperatureText.Width + 10, 96.0d + WorkArea.Top));

            // Feels Like
            string feelsLikeTemp = forecast.CurrentWeather.FeelsLike.ToString();
            FormattedText apparentTempText = new("Feels like " + feelsLikeTemp + "°", cultureInfo,
                FlowDirection.LeftToRight, typeface, 40, Brushes.White, 1.0d);
            dc.DrawText(apparentTempText, new Point(leftTextLeft + 10, 300.0d + WorkArea.Top));

            //Right Side Text
            double rightTextCenter = (WorkArea.Width * 0.77d) + WorkArea.Left + (weatherIconWidth / 2) - 150.0d;

            // Chance of precipitation (rain or snow)
            string precipInfo = forecast.Days.Forecastdays[0].WeatherData.PrecipitationInfo;
            if (!string.IsNullOrEmpty(precipInfo))
            {
                FormattedText precipInfoText = new(precipInfo, cultureInfo, FlowDirection.LeftToRight,
                    typeface, 30, Brushes.Cyan, 1.0d)
                { TextAlignment = TextAlignment.Center, MaxTextWidth = 300.0d };
                dc.DrawText(precipInfoText, new Point(rightTextCenter + 25, 40.0d + WorkArea.Top));
                //Umbrella
                dc.DrawImage((ImageSource)Application.Current.FindResource("Umbrella"),
                    new Rect(rightTextCenter + (precipInfoText.Width / 2) - 10, 44.0d + WorkArea.Top, 24.0d, 24.0d));
            }

            // Weather Description
            FormattedText descriptionText = new(forecast.CurrentWeather.Condition.Text, cultureInfo, FlowDirection.LeftToRight,
                typeface, 40, Brushes.White, 1.0d)
            { TextAlignment = TextAlignment.Center, MaxTextWidth = 300.0d };
            dc.DrawText(descriptionText, new Point(rightTextCenter, 300.0d + WorkArea.Top));

            dc.Close();
            return dv;
        }

        private DrawingVisual DrawWeatherIcon(WeatherApi.Models.Forecast forecast)
        {
            DrawingVisual dv = new();
            using DrawingContext dc = dv.RenderOpen();
            Rect WeatherIconRect = new(new Point((WorkArea.Width * 0.77) + WorkArea.Left, 97.0 + WorkArea.Top),
                                       new Size(weatherIconWidth, weatherIconWidth));
            dc.DrawImage(forecast.CurrentWeather.WeatherIcon, WeatherIconRect);
            dc.Close();
            return dv;
        }

        private string SaveBitmap(Visual visual)
        {
            RenderTargetBitmap rtbmp = CreateBitmap(visual);
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

        internal void NetworkError()
        {
            BitmapFrame bitmapFrame = BitmapFrame.Create(
                   new Uri("pack://application:,,,/weather-frog;component/Resources/FrogIllustrations/NetworkUnavailable.png"));
            DrawTextAndImage("Network currently unavailable", bitmapFrame);
        }

        internal void Offline(string message = "Weather Frog Is Offline")
        {
            BitmapFrame bitmapFrame = BitmapFrame.Create(
                   new Uri("pack://application:,,,/weather-frog;component/Resources/FrogIllustrations/SomethingWrong.png"));
            DrawTextAndImage(message, bitmapFrame);
        }

        private void DrawTextAndImage(string textToDraw, ImageSource imageSource)
        {
            const double maxTextWidth = 600.0d;
            DrawingVisual dv = new();
            using DrawingContext dc = dv.RenderOpen();
            dc.DrawRectangle(App.DefaultBackgroundBrush, null, new(0, 0, Width, Height));
            FormattedText msgText = new(textToDraw, cultureInfo, FlowDirection.LeftToRight,
                typeface, 40, Brushes.White, 1.0d)
            { TextAlignment = TextAlignment.Center, MaxTextWidth = maxTextWidth };
            dc.DrawText(msgText, new Point((WorkArea.Width / 2) - (maxTextWidth / 2), 120.0d));
            dc.DrawImage(imageSource, GetIllustrationRectangle(imageSource, AlignmentX.Center));
            dc.Close();
            SetDesktopWallpaper(dv);
        }

        private Rect GetIllustrationRectangle(ImageSource imageSource, AlignmentX alignmentX = AlignmentX.Left)
        {
            double y = WorkArea.Height / 3.0d;
            double height = WorkArea.Height * (2.0d / 3.0d);
            double scale = height / imageSource.Height;
            double width = imageSource.Width * scale;
            double x = 0.0d;
            switch (alignmentX)
            {
                case AlignmentX.Center:
                    x = (WorkArea.Width - width) / 2.0d;
                    break;
                case AlignmentX.Right:
                    x = WorkArea.Width - width;
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
