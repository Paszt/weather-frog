using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weatherfrog.WeatherApi.Models;

namespace weatherfrog.Resources
{
    class ForecastDesignData : WeatherApi.Models.BaseModel
    {
        private Forecast forecast;
        public Forecast Forecast { get => forecast; set => SetProperty(ref forecast, value); }

        public ForecastDesignData()
        {
            Forecast = new()
            {
                CurrentWeather = new()
                {
                    TempF = 54,
                    FeelsLikeF = 52,
                    IsDay = false,
                    Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" },
                },
                Days = new()
                {
                    Forecastdays = new()
                    {
                        new Forecastday()
                        {
                            HourlyWeather = new()
                            {
                                new Hour() { TempF = 32, Time = DateTime.Now.AddHours(0),  Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0,  IsDay = true },
                                new Hour() { TempF = 34, Time = DateTime.Now.AddHours(1),  Condition = new() { Code = 1000, Text = "Sunny" }                      , ChanceOfRain = 90, ChanceOfSnow = 0,  IsDay = true },
                                new Hour() { TempF = 38, Time = DateTime.Now.AddHours(2),  Condition = new() { Code = 1114, Text = "Blowing snow" }               , ChanceOfRain = 0,  ChanceOfSnow = 70, IsDay = true },
                                new Hour() { TempF = 42, Time = DateTime.Now.AddHours(3),  Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0,  IsDay = false },
                                new Hour() { TempF = 47, Time = DateTime.Now.AddHours(4),  Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0,  IsDay = false },
                                new Hour() { TempF = 52, Time = DateTime.Now.AddHours(5),  Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0,  IsDay = false },
                                new Hour() { TempF = 74, Time = DateTime.Now.AddHours(6),  Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0,  IsDay = true },
                                new Hour() { TempF = 60, Time = DateTime.Now.AddHours(7),  Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0,  IsDay = true },
                                new Hour() { TempF = 58, Time = DateTime.Now.AddHours(8),  Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0,  IsDay = true },
                                new Hour() { TempF = 42, Time = DateTime.Now.AddHours(9),  Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0,  IsDay = true },
                                new Hour() { TempF = 32, Time = DateTime.Now.AddHours(10), Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0,  IsDay = true },
                                new Hour() { TempF = 28, Time = DateTime.Now.AddHours(11), Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0,  IsDay = true },
                            },
                            WeatherData = new() { DailyWillItSnow = false, DailyWillItRain = true, DailyChanceOfRain = 98, MaxTempF = 64, MinTempF = 42 }
                        },
                        new Forecastday()
                        {
                            HourlyWeather = new()
                            {
                                new Hour() { TempF = 32, Time = DateTime.Now.AddHours(0 + 24), Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0, IsDay = true },
                                new Hour() { TempF = 34, Time = DateTime.Now.AddHours(1 + 24), Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0, IsDay = true },
                                new Hour() { TempF = 38, Time = DateTime.Now.AddHours(2 + 24), Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0, IsDay = true },
                                new Hour() { TempF = 42, Time = DateTime.Now.AddHours(3 + 24), Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0, IsDay = true },
                                new Hour() { TempF = 47, Time = DateTime.Now.AddHours(4 + 24), Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0, IsDay = true },
                                new Hour() { TempF = 52, Time = DateTime.Now.AddHours(5 + 24), Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0, IsDay = true },
                                new Hour() { TempF = 64, Time = DateTime.Now.AddHours(6 + 24), Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0, IsDay = true },
                                new Hour() { TempF = 60, Time = DateTime.Now.AddHours(7 + 24), Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0, IsDay = true },
                                new Hour() { TempF = 58, Time = DateTime.Now.AddHours(8 + 24), Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0, IsDay = true },
                                new Hour() { TempF = 42, Time = DateTime.Now.AddHours(9 + 24), Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0, IsDay = true },
                                new Hour() { TempF = 32, Time = DateTime.Now.AddHours(10 + 24), Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0, IsDay = true },
                                new Hour() { TempF = 28, Time = DateTime.Now.AddHours(11 + 24), Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }, ChanceOfRain = 90, ChanceOfSnow = 0, IsDay = true },
                            }
                        },
                    }
                },
                Location = new() { Name = "Monkey's Eyebrow", Region = "Kentucky", Localtime = DateTime.Now.ToString() }
            };
        }
    }
}
