Public Class ComparisonConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object,
                            ByVal targetType As Type,
                            ByVal parameter As Object,
                            ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Return value?.Equals(parameter)
    End Function

    Public Function ConvertBack(ByVal value As Object,
                                ByVal targetType As Type,
                                ByVal parameter As Object,
                                ByVal culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return If(value?.Equals(True) = True, parameter, Binding.DoNothing)
    End Function
End Class