Imports System.Security
Imports System.Runtime.InteropServices

Namespace WinApi

    <SuppressUnmanagedCodeSecurity>
    Friend NotInheritable Class UnsafeNativeMethods

        <DllImport("user32.dll")>
        Public Shared Function ReleaseCapture() As Boolean
        End Function

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
        Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
        End Function

        '<DllImport("user32.dll", EntryPoint:="SetClassLongPtrA", SetLastError:=True, CharSet:=CharSet.Ansi)> _
        'Public Shared Function SetClassLongPtr(hWnd As IntPtr, <MarshalAs(UnmanagedType.I4)> nIndex As ClassLongFlags, newLong As Integer) As Integer
        'End Function

        <DllImport("user32.dll", EntryPoint:="SetClassLong")>
        Friend Shared Function SetClassLongPtr32(hWnd As IntPtr, nIndex As Integer, dwNewLong As UInteger) As UInteger
        End Function

        <DllImport("user32.dll", EntryPoint:="SetClassLongPtr")>
        Friend Shared Function SetClassLongPtr64(hWnd As IntPtr, nIndex As Integer, dwNewLong As IntPtr) As IntPtr
        End Function

        <DllImport("user32.dll", ExactSpelling:=True)>
        Public Shared Function MonitorFromWindow(ByVal handle As IntPtr, ByVal flags As Integer) As IntPtr
        End Function

        <DllImport("user32.dll")>
        Public Shared Function GetMonitorInfo(hMonitor As IntPtr, ByRef lpmi As MONITORINFO) As Boolean
        End Function

        <System.Runtime.InteropServices.DllImport("user32.dll")>
        Public Shared Function SetWindowPlacement(ByVal hWnd As IntPtr, ByRef lpwndpl As WINDOWPLACEMENT) As Boolean
        End Function

        <System.Runtime.InteropServices.DllImport("user32.dll")>
        Public Shared Function GetWindowPlacement(ByVal hWnd As IntPtr, ByRef lpwndpl As WINDOWPLACEMENT) As Boolean
        End Function

    End Class

    Public NotInheritable Class Constants

        Public Const WM_SYSCOMMAND As Integer = &H112
        Public Const WM_SETTEXT As Integer = &HC
        Public Const WM_INITMENU As Integer = &H116
        Public Const WM_MOVE As Integer = &H3
        Public Const WM_NCHITTEST As Integer = &H84
        Public Const WM_NCLBUTTONDOWN As Integer = &HA1
        Public Const WM_NCCALCSIZE As Integer = &H83
        Public Const WM_NCPAINT As Integer = &H85
        Public Const WM_NCACTIVATE As Integer = &H86
        Public Const WM_GETMINMAXINFO As Integer = &H24
        Public Const WM_WINDOWPOSCHANGING = &H46
        Public Const WM_CREATE As Integer = &H1

        Public Const WS_MAXIMIZE As Long = &H1000000
        Public Const GCLP_HBRBACKGROUND As Integer = -&HA
        Public Const HT_CAPTION As Integer = &H2
        Public Const HTLEFT As Integer = &HA
        Public Const HTRIGHT As Integer = &HB
        Public Const HTTOP As Integer = &HC
        Public Const HTTOPLEFT As Integer = &HD
        Public Const HTTOPRIGHT As Integer = &HE
        Public Const HTBOTTOM As Integer = &HF
        Public Const HTBOTTOMLEFT As Integer = &H10
        Public Const HTBOTTOMRIGHT As Integer = &H11
        Public Const TPM_RETURNCMD As UInteger = &H100
        Public Const TPM_LEFTBUTTON As UInteger = &H0
        Public Const SW_SHOWNORMAL As Integer = 1
        Public Const SW_SHOWMINIMIZED As Integer = 2
        Public Const SYSCOMMAND As UInteger = &H112

        Public Const MONITOR_DEFAULTTONEAREST As Integer = &H2

        Public Const SC_MAXIMIZE As Integer = &HF030
        Public Const SC_SIZE As Integer = &HF000
        Public Const SC_MINIMIZE As Integer = &HF020
        Public Const SC_RESTORE As Integer = &HF120
        Public Const SC_MOVE As Integer = &HF010
        Public Const MF_GRAYED As Integer = &H1
        Public Const MF_BYCOMMAND As Integer = &H0
        Public Const MF_ENABLED As Integer = &H0

        Public Const SWP_NOSIZE As UInteger = &H1
        Public Const SWP_NOMOVE As UInteger = &H2
        Public Const SWP_NOZORDER As UInteger = &H4
        Public Const SWP_NOREDRAW As UInteger = &H8
        Public Const SWP_NOACTIVATE As UInteger = &H10

        ' The frame changed: send WM_NCCALCSIZE 
        Public Const SWP_FRAMECHANGED As UInteger = &H20
        Public Const SWP_SHOWWINDOW As UInteger = &H40
        Public Const SWP_HIDEWINDOW As UInteger = &H80
        Public Const SWP_NOCOPYBITS As UInteger = &H100
        ' Don’t do owner Z ordering 
        Public Const SWP_NOOWNERZORDER As UInteger = &H200
        ' Don’t send WM_WINDOWPOSCHANGING
        Public Const SWP_NOSENDCHANGING As UInteger = &H400

        Public Const TOPMOST_FLAGS As UInteger = SWP_NOACTIVATE Or SWP_NOOWNERZORDER Or SWP_NOSIZE Or SWP_NOMOVE Or SWP_NOREDRAW Or SWP_NOSENDCHANGING

        Public Const EM_SETMODIFY = &HB9

    End Class

    Public Structure POINTAPI

        Public x As Integer
        Public y As Integer

    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure MINMAXINFO

        Public ptReserved As POINTAPI
        Public ptMaxSize As POINTAPI
        Public ptMaxPosition As POINTAPI
        Public ptMinTrackSize As POINTAPI
        Public ptMaxTrackSize As POINTAPI

    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure WINDOWPOS

        Public hwnd As IntPtr
        Public hwndInsertAfter As IntPtr
        Public x As Integer
        Public y As Integer
        Public cx As Integer
        Public cy As Integer
        Public flags As Integer

    End Structure

    Public Enum SetWindowPosFlags

        NOSIZE = &H1
        NOMOVE = &H2
        NOZORDER = &H4
        NOREDRAW = &H8
        NOACTIVATE = &H10
        DRAWFRAME = &H20
        FRAMECHANGED = &H20
        SHOWWINDOW = &H40
        HIDEWINDOW = &H80
        NOCOPYBITS = &H100
        NOOWNERZORDER = &H200
        NOREPOSITION = &H200
        NOSENDCHANGING = &H400
        DEFERERASE = &H2000
        ASYNCWINDOWPOS = &H4000

    End Enum

    <StructLayout(LayoutKind.Sequential, Pack:=0)>
    Public Structure RECTAPI
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
        Public Shared ReadOnly Empty As RECTAPI

        Public ReadOnly Property Width() As Integer
            Get
                Return Math.Abs(Me.Right - Me.Left)
            End Get
        End Property
        ' Abs needed for BIDI OS

        Public ReadOnly Property Height() As Integer
            Get
                Return Me.Bottom - Me.Top
            End Get
        End Property

        Public Sub New(left As Integer, top As Integer, right As Integer, bottom As Integer)
            Me.Left = left
            Me.Top = top
            Me.Right = right
            Me.Bottom = bottom
        End Sub

        Public Sub New(rcSrc As RECTAPI)
            Me.Left = rcSrc.Left
            Me.Top = rcSrc.Top
            Me.Right = rcSrc.Right
            Me.Bottom = rcSrc.Bottom
        End Sub

        Public ReadOnly Property IsEmpty() As Boolean
            Get
                ' BUGBUG : On Bidi OS (hebrew arabic) left > right
                Return Me.Left >= Me.Right OrElse Me.Top >= Me.Bottom
            End Get
        End Property

        Public Overrides Function ToString() As String
            If Me = Empty Then
                Return "RECT {Empty}"
            End If
            Return "RECT { left : " & Me.Left & " / top : " & Me.Top & " / right : " & Me.Right & " / bottom : " & Me.Bottom & " }"
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            If Not (TypeOf obj Is RECTAPI) Then
                Return False
            End If
            Return (Me = CType(obj, RECTAPI))
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.Left.GetHashCode() + Me.Top.GetHashCode() + Me.Right.GetHashCode() + Me.Bottom.GetHashCode()
        End Function

        Public Shared Operator =(rect1 As RECTAPI, rect2 As RECTAPI) As Boolean
            Return (rect1.Left = rect2.Left AndAlso rect1.Top = rect2.Top AndAlso rect1.Right = rect2.Right AndAlso rect1.Bottom = rect2.Bottom)
        End Operator

        Public Shared Operator <>(rect1 As RECTAPI, rect2 As RECTAPI) As Boolean
            Return Not (rect1 = rect2)
        End Operator

    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Public Class MONITORINFO

        Public cbSize As Integer = Marshal.SizeOf(GetType(MONITORINFO))
        Public rcMonitor As New RECTAPI()
        Public rcWork As New RECTAPI()
        Public dwFlags As Integer = 0

    End Class

    ' WINDOWPLACEMENT stores the position, size, and state of a window
    <Serializable>
    <System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)>
    Public Structure WINDOWPLACEMENT
        Public length As Integer
        Public flags As Integer
        Public showCmd As ShowWindowCommands
        Public minPosition As WinApi.POINTAPI
        Public maxPosition As WinApi.POINTAPI
        Public normalPosition As WinApi.RECTAPI
    End Structure

    Public Enum ShowWindowCommands As Integer
        Hide = 0
        Normal = 1
        ShowMinimized = 2
        Maximize = 3
        ShowMaximized = 3
        ShowNoActivate = 4
        Show = 5
        Minimize = 6
        ShowMinNoActive = 7
        ShowNA = 8
        Restore = 9
        ShowDefault = 10
        ForceMinimize = 11
    End Enum

End Namespace
