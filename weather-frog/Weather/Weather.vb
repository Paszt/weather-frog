Imports weatherfrog.Extensions
Imports weatherfrog.Infrastructure

Public Class Weather

    Private Const currentWeatherUrlFormat = "http://api.weatherbit.io/v2.0/current?city={0}&units={1}&key={2}"
    Private Const minimumMinutesToUpdate = 5

    Public Shared Sub UpdateCurrentWeather(Optional ForceUpdate As Boolean = False)
        If Not ForceUpdate AndAlso (Date.Now - My.Settings.LatestWeatherApiRequestDate).TotalMinutes < minimumMinutesToUpdate Then
            Exit Sub
        End If
        My.Settings.LatestWeatherApiRequestDate = Date.Now
        Dim json As String = WebResources.DownloadString(String.Format(currentWeatherUrlFormat,
                                                                       My.Settings.City, My.Settings.Units,
                                                                       My.Settings.WeatherbitApiKey))
        Dim cw = json.FromJSON(Of CurrentWeather)
        If cw?.Data IsNot Nothing AndAlso cw?.Data?.Count > 0 Then
            My.Application.CurrentWeatherConditions = cw.Data(0)
        Else
            My.Application.CurrentWeatherConditions = EmptyWeather()
        End If
        My.Settings.WeatherData = json
        My.Settings.Save()
    End Sub

    Private Shared ReadOnly Property EmptyWeather() As CurrentWeather.Datum
        Get
            Return New CurrentWeather.Datum() With {.Weather = New CurrentWeather.Weather}
        End Get
    End Property

End Class
