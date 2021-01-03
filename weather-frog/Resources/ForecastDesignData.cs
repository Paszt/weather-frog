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
        public Forecast Forecast;

        public ForecastDesignData()
        {
            Forecast = new()
            {
                Days = new()
                {
                    Forecastdays = new()
                    {
                        new Forecastday()
                        {
                            HourlyWeather = new()
                            {
                                new Hour() { TempF = 32, Time = DateTime.Now.AddHours(1) },
                                new Hour() { TempF = 34, Time = DateTime.Now.AddHours(2) },
                                new Hour() { TempF = 38, Time = DateTime.Now.AddHours(3) },
                                new Hour() { TempF = 42, Time = DateTime.Now.AddHours(4) },
                                new Hour() { TempF = 47, Time = DateTime.Now.AddHours(5) },
                                new Hour() { TempF = 52, Time = DateTime.Now.AddHours(6) },
                                new Hour() { TempF = 64, Time = DateTime.Now.AddHours(7) },
                                new Hour() { TempF = 60, Time = DateTime.Now.AddHours(8) },
                                new Hour() { TempF = 58, Time = DateTime.Now.AddHours(9) },
                                new Hour() { TempF = 42, Time = DateTime.Now.AddHours(10) },
                                new Hour() { TempF = 32, Time = DateTime.Now.AddHours(11) },
                                new Hour() { TempF = 28, Time = DateTime.Now.AddHours(12) },
                            }
                        }
                    }
                }
            };
            NotifyPropertyChanged("Forecast");
        }
    }
}
