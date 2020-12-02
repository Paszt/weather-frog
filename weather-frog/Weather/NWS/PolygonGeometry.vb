Namespace NWS

    Partial Public Class PolygonGeometry
        Public Property Type As String
        Public Property Coordinates As List(Of List(Of List(Of Double)))

        Public Function ToLocationCollection() As Microsoft.Maps.MapControl.WPF.LocationCollection
            Dim lc As New Microsoft.Maps.MapControl.WPF.LocationCollection()
            For Each coord In Coordinates.First()
                lc.Add(New Microsoft.Maps.MapControl.WPF.Location(Convert.ToDouble(coord(1).ToString("N3")),
                                                                  Convert.ToDouble(coord(0).ToString("N3"))))
            Next
            Return lc
        End Function

    End Class

End Namespace
