Imports System.IO
Imports System.Text
Imports System.Runtime.Serialization.Json

Namespace Extensions

    Public Module JsonExtensions

        ''' <summary>
        ''' Creates an object from JSON
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="json"></param>
        ''' <returns></returns>
        <System.Runtime.CompilerServices.Extension>
        Public Function FromJSON(Of T)(json As String) As T
            If String.IsNullOrEmpty(json) Then
                Return Nothing
            End If

            Try
                Using ms = New MemoryStream(Encoding.UTF8.GetBytes(json.ToCharArray()))
                    Dim ser = New DataContractJsonSerializer(GetType(T))
                    Return DirectCast(ser.ReadObject(ms), T)
                End Using
            Catch generatedExceptionName As Exception
                Return Nothing
            End Try
        End Function

    End Module

End Namespace
