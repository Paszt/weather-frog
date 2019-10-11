Imports System.Net
Imports System.Net.Sockets

Namespace Infrastructure

    Public Class WebClientEx
        Inherits WebClient

        Public Sub New()
            MyBase.New()
            Encoding = Text.Encoding.UTF8
        End Sub

        Protected Overrides Function GetWebRequest(address As Uri) As WebRequest
            Dim request As HttpWebRequest = CType(MyBase.GetWebRequest(address), HttpWebRequest)
            If request IsNot Nothing Then
                request.UserAgent = "weather-frog/0.0.1 (https://github.com/Paszt/weather-frog)"
            End If
            Return request
        End Function

        Public Shadows Function DownloadString(address As String) As String
            Return PerformRemoteAction(AddressOf MyBase.DownloadString, address)
        End Function

        Public Shadows Function DownloadString(uri As Uri) As String
            Return PerformRemoteAction(AddressOf MyBase.DownloadString, uri)
        End Function

#Region " Retry on Transient Errors "

        ''' <summary>
        ''' Performs a method with a single string parameter which communicates with a network resource. 
        ''' If the method fails with a transient type error, the action is retried an amount of times 
        ''' not to exceed the number specified by RetryCount.
        ''' </summary>
        ''' <typeparam name="T">The type of the return value.</typeparam>
        ''' <param name="Action">The method to perform.</param>
        ''' <param name="ActionParameter">The method's <see cref="System.String"/> parameter.</param>
        ''' <param name="RetryInterval">
        ''' The length of time to wait before retrying if the method 
        ''' results in a transient exception.
        ''' </param>
        ''' <param name="RetryCount">The maximum number of times to retry the method.</param>
        ''' <returns>The return value of the specified method.</returns>
        Public Shared Function PerformRemoteAction(Of T)(Action As Func(Of String, T),
                                                     ActionParameter As String,
                                                     RetryInterval As TimeSpan,
                                                     RetryCount As Integer) As T
            Dim currentRetry As Integer = 0
            While True
                Try
                    ' Calling external service.
                    Return Action(ActionParameter)
                Catch ex As Exception
                    Trace.TraceError("Operation Exception")
                    currentRetry += 1
                    If currentRetry > RetryCount OrElse Not IsExceptionTransient(ex) Then
                        ' If this is not a transient error or retry count has
                        ' been reached rethrow the exception. 
                        Throw
                    End If
                End Try
                ' Wait to retry the operation.
                System.Threading.Thread.Sleep(RetryInterval)
            End While
        End Function

        ''' <summary>
        ''' Performs a method with a single string parameter which communicates with a network resource. 
        ''' If the method fails with a transient type error, the action is retried 2 times, pausing 1 second 
        ''' between tries.
        ''' </summary>
        ''' <typeparam name="T">The type of the return value.</typeparam>
        ''' <param name="Action">The method to perform.</param>
        ''' <param name="ActionParameter">The method's string parameter.</param>
        Public Shared Function PerformRemoteAction(Of T)(Action As Func(Of String, T), ActionParameter As String) As T
            Return PerformRemoteAction(Of T)(Action, ActionParameter, TimeSpan.FromSeconds(1), 2)
        End Function

        Public Shared Function PerformRemoteAction(Of T)(Action As Func(Of Uri, T),
                                                     ActionParameter As Uri) As T
            Dim currentRetry As Integer = 0
            While True
                Try
                    ' Calling external service.
                    Return Action(ActionParameter)
                Catch ex As Exception
                    Trace.TraceError("Operation Exception")
                    currentRetry += 1
                    If currentRetry > 2 OrElse Not IsExceptionTransient(ex) Then
                        ' If this is not a transient error or retry count has
                        ' been reached rethrow the exception. 
                        Throw
                    End If
                End Try
                ' Wait to retry the operation.
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1))
            End While
        End Function


        Private Shared ReadOnly TransientHttpStatusCodes As Integer() = New Integer() {500, 504, 503, 408}
        Private Shared ReadOnly TransientWebExceptionStatuses As WebExceptionStatus() =
        New WebExceptionStatus() {WebExceptionStatus.ConnectionClosed,
                                  WebExceptionStatus.Timeout,
                                  WebExceptionStatus.RequestCanceled,
                                  WebExceptionStatus.KeepAliveFailure,
                                  WebExceptionStatus.PipelineFailure,
                                  WebExceptionStatus.ReceiveFailure,
                                  WebExceptionStatus.ConnectFailure,
                                  WebExceptionStatus.SendFailure}
        Private Shared ReadOnly TransientSocketErrorCodes As SocketError() = New SocketError() {SocketError.ConnectionRefused,
                                                                                            SocketError.TimedOut}

        ''' <summary>
        ''' Determines if the exception is a transient type exception.
        ''' </summary>
        ''' <param name="Exc">The exception to check.</param>
        ''' <returns>True if the exception is determined to be transient, false otherwise.</returns>
        Public Shared Function IsExceptionTransient(Exc As Exception) As Boolean
            Dim webExc = TryCast(Exc, WebException)
            If webExc IsNot Nothing Then
                If TransientWebExceptionStatuses.Contains(webExc.Status) Then
                    Return True
                End If
                If webExc.Status = WebExceptionStatus.ProtocolError Then
                    Dim httpWebResp As HttpWebResponse = TryCast(webExc.Response, HttpWebResponse)
                    If httpWebResp IsNot Nothing AndAlso
                    TransientHttpStatusCodes.Contains(CInt(httpWebResp.StatusCode)) Then
                        Return True
                    End If
                End If
                Return False
            Else
                Dim socketExc = TryCast(Exc, SocketException)
                If socketExc IsNot Nothing AndAlso TransientSocketErrorCodes.Contains(socketExc.SocketErrorCode) Then
                    Return True
                End If
            End If
            Return False
        End Function

#End Region


    End Class

End Namespace
