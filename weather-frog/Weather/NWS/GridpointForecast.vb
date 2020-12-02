Namespace NWS
    Partial Public Class GridpointForecast

        Public Property Geometry As PolygonGeometry
        Public Property Properties As Properties
    End Class

    Partial Public Class Properties
        Public Property Updated As DateTimeOffset
        Public Property Units As String
        Public Property GeneratedAt As DateTimeOffset
        Public Property UpdateTime As DateTimeOffset
        Public Property ValidTimes As String
        Public Property Elevation As QuantitativeValue
        Public Property Periods As List(Of ForecastPeriod)
    End Class

    Partial Public Class ForecastPeriod
        Public Property Number As Long
        Public Property Name As String
        Public Property StartTime As DateTimeOffset
        Public Property EndTime As DateTimeOffset
        Public Property IsDaytime As Boolean
        Public Property Temperature As Long
        Public Property TemperatureUnit As String
        Public Property TemperatureTrend As Object
        Public Property WindSpeed As String
        Public Property WindDirection As String
        Public Property Icon As Uri
        Public Property ShortForecast As String
        Public Property DetailedForecast As String
    End Class

End Namespace
