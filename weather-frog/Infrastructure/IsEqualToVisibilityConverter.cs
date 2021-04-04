using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace weatherfrog.Infrastructure
{
    public class IsEqualToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
            values.Length < 1
                ? Visibility.Visible
                : values.Select(s => s.ToString().ToLower()).Distinct().Count() == 1
                    ? Visibility.Collapsed
                    : Visibility.Visible;

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
