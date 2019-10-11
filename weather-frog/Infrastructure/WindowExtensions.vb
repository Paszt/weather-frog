Option Strict On

Imports weatherfrog.WinApi

Public Module WindowExtensions

#Region "Properties"

    Private Property Encoding As System.Text.Encoding = New System.Text.UTF8Encoding()
    Private Property Serializer As New System.Xml.Serialization.XmlSerializer(GetType(WINDOWPLACEMENT))

#End Region

#Region "Extension Methods"

    <System.Runtime.CompilerServices.Extension>
    Public Sub SetPlacement(window As Window, placementXml As String)
        If Not String.IsNullOrEmpty(placementXml) Then
            SetPlacement(New System.Windows.Interop.WindowInteropHelper(window).Handle, placementXml)
        End If
    End Sub

    '<System.Runtime.CompilerServices.Extension> _
    'Public Sub SetPlacement(form As System.Windows.Forms.Form, placementXml As String)
    '	If Not String.IsNullOrEmpty(placementXml) Then
    '		SetPlacement(form.Handle, placementXml)
    '	End If
    'End Sub

    <System.Runtime.CompilerServices.Extension>
    Public Function GetPlacement(window As Window) As String
        Return GetPlacement(New System.Windows.Interop.WindowInteropHelper(window).Handle)
    End Function

    '<System.Runtime.CompilerServices.Extension> _
    'Public Function GetPlacement(form As System.Windows.Forms.Form) As String
    '	Return GetPlacement(form.Handle)
    'End Function

#End Region

#Region "Supporting Methods"

    Private Sub SetPlacement(windowHandle As IntPtr, placementXml As String)
        If String.IsNullOrEmpty(placementXml) Then
            Return
        End If
        Dim placement As WINDOWPLACEMENT
        Dim xmlBytes As Byte() = Encoding.GetBytes(placementXml)
        Try
            Using memoryStream As New System.IO.MemoryStream(xmlBytes)
                placement = DirectCast(Serializer.Deserialize(memoryStream), WINDOWPLACEMENT)
            End Using
            placement.length = System.Runtime.InteropServices.Marshal.SizeOf(GetType(WINDOWPLACEMENT))
            placement.flags = 0
            placement.showCmd = (If(placement.showCmd = ShowWindowCommands.ShowMinimized, ShowWindowCommands.Normal, placement.showCmd))
            UnsafeNativeMethods.SetWindowPlacement(windowHandle, placement)
            ' Parsing placement XML failed. Fail silently.
        Catch generatedExceptionName As InvalidOperationException
        End Try
    End Sub

    Private Function GetPlacement(windowHandle As IntPtr) As String
        Dim placement As New WINDOWPLACEMENT()
        UnsafeNativeMethods.GetWindowPlacement(windowHandle, placement)
        Using memoryStream As New System.IO.MemoryStream
            Using xmlTextWriter As New System.Xml.XmlTextWriter(memoryStream, System.Text.Encoding.UTF8)
                Serializer.Serialize(xmlTextWriter, placement)
                Dim xmlBytes As Byte() = memoryStream.ToArray()
                Return Encoding.GetString(xmlBytes)
            End Using
        End Using
    End Function

#End Region

End Module
