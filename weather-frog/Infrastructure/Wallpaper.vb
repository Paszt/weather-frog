Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

Public NotInheritable Class Wallpaper

#Region "Win32 API "

    Const SPI_SETDESKWALLPAPER As Integer = 20
    Const SPIF_UPDATEINIFILE As Integer = &H1
    Const SPIF_SENDWININICHANGE As Integer = &H2

    <DllImport("user32", CharSet:=CharSet.Auto)>
    Public Shared Function SystemParametersInfo(
            ByVal intAction As Integer,
            ByVal intParam As Integer,
            ByVal strParam As String,
            ByVal intWinIniFlag As Integer) As Integer
        ' returns non-zero value if function succeeds
    End Function

#End Region

#Region " Class Variables "

    Private Shared weatherData As CurrentWeather.Datum
    Private Shared drawContext As DrawingContext
    Private Shared ReadOnly weatherIconWidth As Integer = 176

#End Region

    Public Shared Sub DrawAndApply()
        weatherData = My.Application.CurrentWeatherConditions
        'https://docs.microsoft.com/en-us/dotnet/framework/wpf/graphics-multimedia/how-to-encode-a-visual-to-an-image-file
        Dim dv As DrawingVisual = Draw()
        Dim rbmp = New RenderTargetBitmap(CInt(SystemParameters.PrimaryScreenWidth),
                                          CInt(SystemParameters.PrimaryScreenHeight),
                                          96.0, 96.0, PixelFormats.Pbgra32)
        rbmp.Render(dv)
        Dim enc As New BmpBitmapEncoder()
        enc.Frames.Add(BitmapFrame.Create(rbmp))
        Dim tempPath = Path.Combine(Path.GetTempPath, "wallpaper.bmp")
        Using stm = File.Create(tempPath)
            enc.Save(stm)
        End Using
        'Set registry keys to center the image
        Dim key As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\Desktop", True)
        key.SetValue("WallpaperStyle", "1")
        key.SetValue("TileWallpaper", "0")
        'Set Desktop Wallpaper
        SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, tempPath, SPIF_UPDATEINIFILE Or SPIF_SENDWININICHANGE)
    End Sub

    Private Shared Function Draw() As DrawingVisual
        Dim dv As New DrawingVisual()
        Using dc As DrawingContext = dv.RenderOpen()
            drawContext = dc
            DrawBackgndGradient()
            DrawFrogIllustration()
            DrawText()
            DrawWeatherIcon()
        End Using
        Return dv
    End Function

    Private Shared Sub DrawBackgndGradient()
        'initialize as day gradient
        Dim gradientBrush = New LinearGradientBrush(New GradientStopCollection() From {
            New GradientStop(CType(ColorConverter.ConvertFromString("#17a3e0"), Color), 0.05),
            New GradientStop(CType(ColorConverter.ConvertFromString("#d5f0fa"), Color), 0.698)
        }, 90.0)
        If weatherData.PartOfDay = "n" Then
            gradientBrush = New LinearGradientBrush(CType(ColorConverter.ConvertFromString("#814efe"), Color),
                                                    CType(ColorConverter.ConvertFromString("#9066ff"), Color),
                                                    90.0)
        End If
        drawContext.DrawRectangle(gradientBrush, Nothing, SystemParameters.WorkArea)
    End Sub

    Private Shared Sub DrawFrogIllustration()

    End Sub

    Private Shared Sub DrawText()
        Dim screenWidth = SystemParameters.PrimaryScreenWidth
        Dim screenHeight = SystemParameters.PrimaryScreenHeight
        Dim leftTextLeft = screenWidth * 0.09

        Dim tf As Typeface = Fonts.GetTypefaces(New Uri("pack://application:,,,/"), "./resources/").First()
        Dim ci = New Globalization.CultureInfo("en-us")
        'Temperature
        Dim temperatureText = New FormattedText(weatherData.Temperature.ToString(), ci,
                                                FlowDirection.LeftToRight,
                                                tf, 120, Brushes.White, 1.0)
        drawContext.DrawText(temperatureText, New Point(leftTextLeft, 120))
        Dim temperatureUnitsString = "°"
        Select Case My.Settings.Units
            Case "M"
                temperatureUnitsString &= "C"
            Case "S"
                temperatureUnitsString = "K"
            Case "I"
                temperatureUnitsString &= "F"
        End Select
        Dim temperatureUnitsText = New FormattedText(temperatureUnitsString, ci, FlowDirection.LeftToRight,
                                                     tf, 80, Brushes.White, 1.0)
        drawContext.DrawText(temperatureUnitsText, New Point(leftTextLeft + temperatureText.Width + 5, 130))
        'Feels Like
        Dim apparentTempText = New FormattedText("Feels like " & weatherData.ApparentTemp & "°", ci,
                                                 FlowDirection.LeftToRight,
                                                 tf, 40, Brushes.White, 1.0)
        drawContext.DrawText(apparentTempText, New Point(leftTextLeft + 10, 300))
        'Weather Description
        Dim descriptionCenter As Double = (screenWidth * 0.77) + (weatherIconWidth / 2)
        Dim descriptionText = New FormattedText(weatherData.Weather.Description, ci, FlowDirection.LeftToRight,
                                                tf, 40, Brushes.White, 1.0) With {.TextAlignment = TextAlignment.Center}
        drawContext.DrawText(descriptionText, New Point(descriptionCenter, 300))
    End Sub

    Private Shared Sub DrawWeatherIcon()
        Dim WeatherIconRect = New Rect(New Point(SystemParameters.PrimaryScreenWidth * 0.77, 97.0),
                                       New Size(weatherIconWidth, weatherIconWidth))
        drawContext.DrawImage(weatherData.WeatherIcon, WeatherIconRect)
    End Sub

End Class
