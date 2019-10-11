Imports weatherfrog.Extensions
Imports weatherfrog.Infrastructure

Public Class Weather

    Const currentWeatherUrlFormat = "http://api.weatherbit.io/v2.0/current?city={0}&units={1}&key={2}"
    Const minimumMinutesToUpdate = 5

    Public Shared Sub UpdateCurrentWeather()
        If (Date.Now - My.Settings.LatestWeatherApiRequestDate).TotalMinutes < minimumMinutesToUpdate Then
            Exit Sub
        End If
        My.Settings.LatestWeatherApiRequestDate = Date.Now
        Dim json As String = WebResources.DownloadString(String.Format(currentWeatherUrlFormat,
                                                                       My.Settings.City, My.Settings.Units,
                                                                       My.Settings.WeatherbitApiKey))
        Dim cw = json.FromJSON(Of CurrentWeather)
        If cw.Data IsNot Nothing AndAlso cw.Data.Count > 0 Then
            My.Application.CurrentWeatherConditions = cw.Data(0)
            My.Settings.WeatherData = json
        End If
        My.Settings.Save()
    End Sub

End Class
