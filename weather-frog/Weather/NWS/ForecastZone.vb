Namespace NWS

    Partial Public Class ForecastZone
        Public Property Geometry As MultiPolygonGeometry
        Public Property Properties As ForecastZoneProperties
    End Class

    Partial Public Class MultiPolygonGeometry
        Public Property Type As String
        Public Property Coordinates As List(Of List(Of List(Of List(Of Double))))
    End Class

    Partial Public Class ForecastZoneProperties
        Public Property Name As String
        Public Property State As String
        Public Property ObservationStations As List(Of Uri)
    End Class

End Namespace
