using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using weatherfrog.WeatherApi.Models;

namespace weatherfrog.Resources
{
    /// <summary>
    /// Interaction logic for HourlyGraph.xaml
    /// </summary>
    public partial class HourlyGraph : UserControl
    {
        private const int HourWidth = 42;
        private readonly CultureInfo cultureInfo = new("en-us");
        private readonly Typeface typeFace;
        private int maxTemp;
        private int minTemp;
        private readonly TranslateTransform rectTranslateTransform;
        private Point dragOrigin;
        private Point dragStart;
        //readonly private Cursor scrollWEDarkCursor;
        //readonly private Cursor scrollWELightCursor;

        public HourlyGraph()
        {
            InitializeComponent();
            rectTranslateTransform = new();
            Border.RenderTransform = rectTranslateTransform;
            //scrollWEDarkCursor = DrawScrollWECursor(Brushes.Black);
            //scrollWELightCursor = DrawScrollWECursor(Brushes.White);

            // Design time can't reference the font...
            try
            {
                typeFace = Fonts.GetTypefaces(new Uri("pack://application:,,,/"), "./resources/").First();
            }
            catch (Exception)
            {
                typeFace = Fonts.SystemTypefaces.First();
            }
            Draw();
        }

        #region Dependency Properties

        public Forecast Forecast
        {
            get => (Forecast)GetValue(ForecastProperty);
            set => SetValue(ForecastProperty, value);
        }

        public static readonly DependencyProperty ForecastProperty =
              DependencyProperty.Register("Forecast", typeof(Forecast),
                typeof(HourlyGraph), new PropertyMetadata(null, OnForecastChanged));

        private static void OnForecastChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HourlyGraph hourlyGraph = d as HourlyGraph;
            hourlyGraph.OnForecastChanged();
        }

        private void OnForecastChanged()
        {
            Draw();
        }

        #endregion

        #region Drawing Graphics

        private void Draw()
        {
            if (!(null == Forecast?.Days))
            {
                const int lastHourToShow = 10;
                //Rectangle.Fill = Brushes.Transparent;
                List<Hour> upcomingHours = null;
                // Get hourly data from the first day, today, starting with and including the current hour.
                int localTimeHour = DateTime.Parse(Forecast?.Location?.Localtime).Hour;
                upcomingHours = Forecast.Days?.Forecastdays[0]?.HourlyWeather?.Where(h => h.Time.Hour > localTimeHour - 1).ToList();
                // Get the hourly data from the next day, tomorrow, up to 10 AM.
                upcomingHours.AddRange(Forecast.Days?.Forecastdays[1]?.HourlyWeather?.Where(h => h.Time.Hour <= lastHourToShow).ToList());

                DrawingVisual dv = new();
                using DrawingContext dc = dv.RenderOpen();

                //Calcualte High/Low Temperatures
                maxTemp = upcomingHours.Max(h => h.Temp);
                minTemp = upcomingHours.Min(h => h.Temp);

                double leftPoint = 0.0;

                // Create the Graph
                Graph.Points.Clear();
                Graph.Points.Add(new Point(0, 150));
                Graph.Points.Add(new Point(0, CalculateYValue(upcomingHours[0].Temp)));

                //leftPoint = 0.0;
                if (!(null == upcomingHours))
                {
                    foreach (Hour hour in upcomingHours)
                    {
                        // Temperature
                        DrawTextCentered(dc, hour.Temp.ToString() + "°", Brushes.White, leftPoint, CalculateYValue(hour.Temp) - 24);
                        // Chance of Precip
                        if (hour.ChanceOfSnow > 0)
                        { DrawTextCentered(dc, hour.ChanceOfSnow.ToString() + '%', Brushes.Cyan, leftPoint, 62); }
                        else
                        {
                            if (hour.ChanceOfRain > 0)
                            { DrawTextCentered(dc, hour.ChanceOfRain.ToString() + '%', Brushes.Cyan, leftPoint, 62); }
                        }
                        //Weather Icon
                        dc.DrawImage(hour.WeatherIcon, new Rect(leftPoint + 4, 80, HourWidth - 8, HourWidth - 8));
                        // Time
                        DrawTextCentered(dc, hour.Time.ToString("h tt"), Brushes.Black, leftPoint, 80 + HourWidth + 6);
                        // Draw Graph
                        Graph.Points.Add(new Point(leftPoint + (HourWidth / 2), CalculateYValue(hour.Temp)));

                        leftPoint += HourWidth;
                    }
                }
                dc.Close();
                // Close graph
                // final graph point is half way between lastHourToShow.Temp & (lastHourToShow + 1).Temp
                int lastHourTemp = Forecast.Days.Forecastdays[1].HourlyWeather[lastHourToShow].Temp;
                int afterLastHourTemp = Forecast.Days.Forecastdays[1].HourlyWeather[lastHourToShow + 1].Temp;
                int finalTemp = lastHourTemp + ((afterLastHourTemp - lastHourTemp) / 2);
                Graph.Points.Add(new Point(leftPoint, CalculateYValue(finalTemp)));
                Graph.Points.Add(new Point(leftPoint, 150));

                AdornerRectangle.Fill = new VisualBrush(dv) { Stretch = Stretch.None };

                Border.Width = Math.Max((double)(upcomingHours.Count * HourWidth), 200);
                if (Border.Width <= RootGrid.ActualWidth)
                {
                    Border.Cursor = Cursors.Arrow;
                }
                else
                {
                    Border.Cursor = Cursors.ScrollWE;
                }
                rectTranslateTransform.X = 0;
                //Y value of Border =   (rectTranslateTransform.X + Border.ActualWidth)
            }
        }

        private void DrawTextCentered(DrawingContext dc, string TextToDraw, Brush brush, double left, double y)
        {
            FormattedText formattedText = new(TextToDraw, cultureInfo, FlowDirection.LeftToRight, typeFace, 12, brush, 1.0d);
            dc.DrawText(formattedText, new Point(left + ((HourWidth - formattedText.Width) / 2), y));
        }

        private double CalculateYValue(int temp)
        {
            double minMaxDiff = maxTemp - minTemp;
            double tempDiffFromMin = minTemp - temp;
            // 38 = maximum height of the graph from min to max
            double graphHeightDelta = 38.0;
            if (minMaxDiff < 5)
            {
                graphHeightDelta = 10;
            }
            double proportional = (graphHeightDelta / minMaxDiff) * tempDiffFromMin;
            // Y = 64 is minimum, just aboe the precip info location
            return proportional + 64;
        }

        //private static Cursor DrawScrollWECursor(Brush brush)
        //{
        //    DrawingVisual dv = new();
        //    using DrawingContext dc = dv.RenderOpen();
        //    dc.DrawGeometry(brush, null, Geometry.Parse("F1 M 21.3333,10.6667L 16.5373,5.87073L 15.1133,7.29602L 18.4827,10.6667L 15.1133,14.036L 16.5373,15.4628M 6.22133,14.036L 2.85067,10.6667L 6.22133,7.29602L 4.796,5.87073L 0,10.6667L 4.796,15.4628M 14.2427,9.44275C 13.552,9.44275 12.9933,10.0013 12.9933,10.6907C 12.9933,11.38 13.552,11.9387 14.2427,11.9387C 14.9307,11.9387 15.4893,11.38 15.4893,10.6907C 15.4893,10.0013 14.9307,9.44275 14.2427,9.44275 Z M 7.092,9.44275C 6.40267,9.44275 5.844,10.0013 5.844,10.6907C 5.844,11.38 6.40267,11.9387 7.092,11.9387C 7.78,11.9387 8.33867,11.38 8.33867,10.6907C 8.33867,10.0013 7.78,9.44275 7.092,9.44275 Z M 11.9387,10.6907C 11.9387,11.38 11.38,11.9387 10.6907,11.9387C 10.0013,11.9387 9.44267,11.38 9.44267,10.6907C 9.44267,10.0013 10.0013,9.44275 10.6907,9.44275C 11.38,9.44275 11.9387,10.0013 11.9387,10.6907 Z "));
        //    dc.Close();
        //    return Infrastructure.Utilities.CreateCursor(64, 64, dv, new System.Drawing.Point(32, 32));
        //}

        #endregion

        #region Border Mouse Event Handlers

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Border.ActualWidth > RootGrid.ActualWidth)
            {
                dragStart = e.GetPosition(this);
                dragOrigin = new Point(rectTranslateTransform.X, rectTranslateTransform.Y);
                Border.Cursor = Cursors.SizeWE;
                Border.CaptureMouse();
            }
        }
        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Border.ActualWidth > RootGrid.ActualWidth)
            {
                Border.ReleaseMouseCapture();
                Border.Cursor = Cursors.ScrollWE;
            }
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (Border.IsMouseCaptured)
            {
                Vector v = dragStart - e.GetPosition(this);
                double newXValue = Math.Min(dragOrigin.X - v.X, 0);
                double minXValue = Math.Min(RootGrid.ActualWidth - Border.ActualWidth, 0);
                if (newXValue < minXValue) { newXValue = minXValue; }
                rectTranslateTransform.X = newXValue;
            }
        }

        #endregion

    }
}
