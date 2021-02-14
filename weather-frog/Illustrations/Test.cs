using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows.Media;

namespace weatherfrog.Illustrations
{
    public class Test
    {
        readonly List<Illustration> illustrations = new()
        {
            new Illustration() { AlignmentX = AlignmentX.Left, TimeOfDay = TimeOfDay.Night, WeatherCondition = WeatherCondition.Sleet },
            new Illustration() { AlignmentX = AlignmentX.Center, TimeOfDay = TimeOfDay.Day, WeatherCondition = WeatherCondition.Rain },
        };

        public Test() =>  Console.WriteLine(JsonSerializer.Serialize(illustrations, new() { WriteIndented = true }));

    }
}
