Partial Friend NotInheritable Class MySettings

    Public Property UnitChoices As New Dictionary(Of String, String) From {
        {"M", "Metric     (Celcius, m/s, mm)"},
        {"S", "Scientific (Kelvin, m/s, mm)"},
        {"I", "Imperial   (Fahrenheit, mph, in)"}
    }

End Class
