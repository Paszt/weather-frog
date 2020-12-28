using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using weatherfrog.WeatherApi.Models;

namespace weatherfrog.Infrastructure
{
    class Utilities
    {
        public static Icon CreateTaskbarIcon(Current current)
        {
            if (My.Settings.TaskbarIconStyle == TaskbarIconStyle.Temperature)
            {
                DrawingVisual dv = new();
                using DrawingContext dc = dv.RenderOpen();
                FormattedText temperatureFormattedText = new(
                     current.Temp + "°",
                     new System.Globalization.CultureInfo("en-us"),
                     FlowDirection.LeftToRight,
                     Fonts.GetTypefaces(new Uri("pack://application:,,,/"), "./resources/").First(),
                     11, 
                     System.Windows.Media.Brushes.White,
                     null,
                     TextFormattingMode.Ideal,
                     4.0);
                dc.DrawText(temperatureFormattedText, new System.Windows.Point(0, 0));

                dc.Close();
                return CreateIcon(16, 16, dv);
            }
            else
            {
                return CreateIcon(16, 16, App.Current.Forecast.CurrentWeather.WeatherIcon);
            }
        }

        /// <summary>
        /// Creates a System.Drawing.Icon from the given visual with dpi X&Y of 96 and a Pixel Format of Pbgra32.
        /// </summary>
        /// <param name="pixelWidth">The width of the resulting Icon.</param>
        /// <param name="pixelHeight">The height of the resulting Icon.</param>
        /// <param name="visual">The <see cref="Visual" /> object to be used as the Icon.</param>
        /// <returns></returns>
        public static Icon CreateIcon(int pixelWidth, int pixelHeight, Visual visual)
        {
            RenderTargetBitmap rbmp = new(pixelWidth, pixelHeight, 96.0, 96.0, PixelFormats.Pbgra32);
            rbmp.Render(visual);
            PngBitmapEncoder encoder = new();
            encoder.Frames.Add(BitmapFrame.Create(rbmp));
            using MemoryStream stream = new();
            encoder.Save(stream);
            using Bitmap bmp = new(stream);
            IntPtr Hicon = bmp.GetHicon();
            return Icon.FromHandle(Hicon);
        }

        /// <summary>
        /// Creates a System.Drawing.Icon from the given ImageSource with dpi X&Y of 96 and a Pixel Format of Pbgra32.
        /// </summary>
        /// <param name="pixelWidth">The width of the resulting Icon.</param>
        /// <param name="pixelHeight">The height of the resulting Icon.</param>
        /// <param name="imageSource">The <see cref="ImageSource" /> to be used as the Icon.</param>
        /// <returns></returns>
        public static Icon CreateIcon(int pixelWidth, int pixelHeight, ImageSource imageSource)
        {
            DrawingVisual dv = new();
            using DrawingContext dc = dv.RenderOpen();
            Rect rect = new(new System.Windows.Point(0, 0), new System.Windows.Size(pixelWidth, pixelHeight));
            dc.DrawImage(imageSource, rect);
            dc.Close();
            return CreateIcon(pixelWidth, pixelHeight, dv);
        }
    }
}
