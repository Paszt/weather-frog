Namespace Infrastructure

    Public Class WebResources

        Public Overloads Shared Function DownloadString(address As String) As String
            Using _client As New WebClientEx()
                Return _client.DownloadString(address)
            End Using
        End Function

        Public Overloads Shared Function DownloadString(address As Uri) As String
            Using _client As New WebClientEx()
                Return _client.DownloadString(address)
            End Using
        End Function

        Public Shared Sub DownloadFile(address As String, fileName As String)
            Using _client As New WebClientEx()
                _client.DownloadFile(address, fileName)
            End Using
        End Sub

    End Class

End Namespace
