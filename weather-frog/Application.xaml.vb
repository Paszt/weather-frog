Imports Hardcodet.Wpf.TaskbarNotification
Imports weatherfrog.Extensions
Imports weatherfrog.Infrastructure

Class Application

#Region " Class Variables "

    Private NotifyIcon As TaskbarIcon
    Private WithEvents OptionsWin As OptionsWindow
    Private tmr As Timers.Timer

#End Region

    Public Property CurrentWeatherConditions As CurrentWeather.Datum
    Public Property MinutesSinceLastUpdate As Integer

    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        CurrentWeatherConditions = My.Settings.WeatherData.FromJSON(Of CurrentWeather).Data(0)
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
            'GetCurrentWeatherUpdateDisplay()
            'StartTimer()
        End If

        'DEBUG:
        GetCurrentWeatherUpdateDisplay()
        'Wallpaper.DrawAndApply(Nothing)
    End Sub

    Private Sub StartTimer()
        tmr = New Timers.Timer(TimeSpan.FromMinutes(10).TotalMilliseconds())
        AddHandler tmr.Elapsed, AddressOf GetCurrentWeatherUpdateDisplay
        tmr.Enabled = True
    End Sub

    Private Sub GetCurrentWeatherUpdateDisplay(Optional sender As Object = Nothing, Optional e As Timers.ElapsedEventArgs = Nothing)
        Weather.UpdateCurrentWeather()
        If My.Settings.UpdateDesktop Then
            Wallpaper.DrawAndApply()
        End If
        'TODO: Update Taskbar Icon
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
        OptionsWin.Show()
    End Sub

    Public ReadOnly Property CloseOptionsCommand As ICommand
        Get
            Return New RelayCommand(Sub()
                                        OptionsWin.Close()
                                        'At the first startup, if user hasn't entered City & API key
                                        If CurrentWeatherConditions Is Nothing Then
                                            GetCurrentWeatherUpdateDisplay()
                                        End If
                                    End Sub)
        End Get
    End Property

End Class
