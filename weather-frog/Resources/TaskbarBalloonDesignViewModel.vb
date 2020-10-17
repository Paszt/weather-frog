Public Class TaskbarBalloonDesignViewModel

    Public Property CurrentWeatherConditions As CurrentWeather.Datum

    Public Sub New()
        Dim weathr = New CurrentWeather.Weather() With {
            .Description = "Partly Cloudy",
            .Icon = "c02d"}

        CurrentWeatherConditions = New CurrentWeather.Datum() With {
            .CityName = "Monkey's Eyebrow",
            .StateCode = "KY",
            .Temperature = 70.2,
            .ApparentTemp = 72.2,
            .Weather = weathr}

    End Sub

End Class
