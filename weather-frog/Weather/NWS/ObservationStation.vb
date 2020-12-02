Namespace NWS
    Partial Public Class ObservationStation
        Public Property Geometry As SimpleGeometry
        Public Property Properties As ObservationStationProperties
    End Class

    Partial Public Class ObservationStationProperties
        Public Property Elevation As QuantitativeValue
        Public Property StationIdentifier As String
        Public Property Name As String

    End Class

End Namespace
