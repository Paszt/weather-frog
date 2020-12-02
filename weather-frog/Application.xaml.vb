Imports System.ComponentModel
Imports Hardcodet.Wpf.TaskbarNotification
Imports weatherfrog.Extensions
Imports weatherfrog.Infrastructure

Class Application
    Implements INotifyPropertyChanged

#Region " Class Variables "

    Private NotifyIcon As TaskbarIcon
    Private WithEvents OptionsWin As OptionsWindow
    Private tmr As Timers.Timer
    Private _CurrentWeatherConditions As CurrentWeather.Datum

#End Region

    Public Property CurrentWeatherConditions As CurrentWeather.Datum
        Get
            Return _CurrentWeatherConditions
        End Get
        Set
            SetProperty(_CurrentWeatherConditions, Value)
        End Set
    End Property

    Public Property MinutesSinceLastUpdate As Integer

    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        If Not String.IsNullOrEmpty(My.Settings.WeatherData) Then
            CurrentWeatherConditions = My.Settings.WeatherData.FromJSON(Of CurrentWeather).Data(0)
        End If
        Weather.UpdateCurrentWeather()

        NotifyIcon = New TaskbarIcon With {
                    .Icon = TaskbarIconHelper.CreateIcon3(),
                    .ToolTipText = "Weather Frog",
                    .TrayToolTip = New TaskbarBalloon(),
                    .ContextMenu = CType(FindResource("NotifyIconMenu"), ContextMenu),
                    .DataContext = Current
                }

        If String.IsNullOrWhiteSpace(My.Settings.WeatherbitApiKey) Or String.IsNullOrWhiteSpace(My.Settings.City) Then
            ShowOptions()
        Else
            'DEBUG: Commented out until needed
            GetCurrentWeatherUpdateDisplay()
            'StartTimer()
        End If

        'DEBUG:
        'GetCurrentWeatherUpdateDisplay()
        'Wallpaper.DrawAndApply(Nothing)
    End Sub

    Private Sub StartTimer()
        tmr = New Timers.Timer(TimeSpan.FromMinutes(10).TotalMilliseconds())
        AddHandler tmr.Elapsed, Sub()
                                    GetCurrentWeatherUpdateDisplay()
                                End Sub
        tmr.Enabled = True
    End Sub

    Friend Sub GetCurrentWeatherUpdateDisplay(Optional ForceUpdate As Boolean = False)
        Weather.UpdateCurrentWeather(ForceUpdate)
        If My.Settings.UpdateDesktop Then
            Wallpaper.DrawAndApply()
        End If
        'TODO: Update Taskbar Icon
        NotifyIcon.Icon = TaskbarIconHelper.CreateIcon3
    End Sub

    Public ReadOnly Property ExitAppCommand As ICommand
        Get
            Return New RelayCommand(Sub()
                                        NotifyIcon.CloseBalloon()
                                        Current.Shutdown()
                                        'TODO: Possibly change the desktop background to indicate the service has stopped,
                                        '      or maybe revert to some default or previous background. (Could create an
                                        '      options to select between these choices).
                                    End Sub)
        End Get
    End Property

    Public ReadOnly Property ShowOptionsCommand As ICommand
        Get
            Return New RelayCommand(Sub()
                                        ShowOptions()
                                    End Sub)
        End Get
    End Property

    Private Sub ShowOptions()
        OptionsWin = New OptionsWindow()
        AddHandler OptionsWin.SourceInitialized,
            Sub()
                OptionsWin.SetPlacement(My.Settings.OptionsWindowPosition)
            End Sub
        AddHandler OptionsWin.Closing,
            Sub()
                My.Settings.OptionsWindowPosition = OptionsWin.GetPlacement()
                My.Settings.Save()
            End Sub
        OptionsWin.ShowDialog()
    End Sub

    Friend Sub CloseOptions(Optional ForceUpdate As Boolean = False)
        OptionsWin.Close()
        GetCurrentWeatherUpdateDisplay(ForceUpdate)
    End Sub

#Region " INotifyPropertyChanged "

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Protected Function SetProperty(Of T)(ByRef storage As T, value As T,
                                         <Runtime.CompilerServices.CallerMemberName> Optional propertyName As String = Nothing) As Boolean
        If Equals(storage, value) Then
            Return False
        End If
        storage = value
        OnPropertyChanged(propertyName)
        Return True
    End Function

    Protected Sub OnPropertyChanged(<Runtime.CompilerServices.CallerMemberName> Optional propertyName As String = Nothing)
        PropertyChangedEvent?(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

#End Region

End Class
