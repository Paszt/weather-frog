using System;
using System.Globalization;
using System.Windows.Data;

namespace weatherfrog.Infrastructure
{
    // Used in Animated Expander
    public class MultiplyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double result = 1.0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] is double @double)
                    result *= @double;
            }
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
            throw new Exception("Not implemented");
    }
}
