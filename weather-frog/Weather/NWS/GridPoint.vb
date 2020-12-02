'' NWS stands for National Weather Service

Namespace NWS

    Public Class GridPoint

        Public Property Properties As GridPointProperties

    End Class

    Public Class GridPointProperties

        Public Property Cwa As String

        Public Property GridId As String

        Public Property GridX As Integer

        Public Property GridY As Integer

        Public Property RelativeLocation As RelativeLocation

        Public Property ForecastZone As String

    End Class

    Public Class RelativeLocation

        Public Property Type As String

        Public Property Geometry As SimpleGeometry

        Public Property Properties As RelativeLocationProperties

    End Class

    Public Class RelativeLocationProperties

        Public Property City As String

        Public Property State As String

        Public Property Distance As QuantitativeValue

        Public Property Bearing As QuantitativeValue

    End Class

End Namespace