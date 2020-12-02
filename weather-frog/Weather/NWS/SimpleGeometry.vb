Imports System.Runtime.Serialization

Namespace NWS

    ''' <summary>
    ''' Coordinates of this type of geometry are in a flat array/list, for example: a point, or a polygon
    ''' </summary>
    <DataContract>
    Public Class SimpleGeometry

        <DataMember(Name:="type")>
        Public Property Type As String

        <DataMember(Name:="coordinates")>
        Public Property Coordinates As List(Of Double)

    End Class

End Namespace