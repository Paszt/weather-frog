Imports System.Drawing

Public Class TaskbarIconHelper

    Public Shared Function CreateIcon(Optional Text As String = "?") As Icon
        Using bm As New Bitmap(16, 16)
            Using g As Graphics = Graphics.FromImage(bm)
                Using f As New Font("Arial", 8)
                    g.DrawString(Text, f, Brushes.Gray, 0F, 0F)
                End Using
                'g.DrawString(Text, My.Application.TaskbarFont, Brushes.White, 0F, 0F)
            End Using
            Return Icon.FromHandle(bm.GetHicon)
        End Using
    End Function

    Public Shared Function CreateIcon2(Optional Text As String = "?") As Icon
        Dim textblk As New TextBlock With {
            .Text = "98°",
            .FontFamily = New Media.FontFamily(New Uri("pack://application:,,,/"), "./Resources/#Roboto"),
            .FontSize = 12.0,
            .Foreground = Media.Brushes.White,
            .TextAlignment = TextAlignment.Center,
            .Margin = New Thickness(0.0)
        }
        textblk.Measure(New Windows.Size(18, 18))
        textblk.Arrange(New Rect(New Windows.Size(18, 18)))
        textblk.UpdateLayout()

        Dim rtBmp = New RenderTargetBitmap(18, 18, 96, 96, PixelFormats.Pbgra32)
        rtBmp.Render(textblk)
        Dim encoder = New PngBitmapEncoder()
        encoder.Frames.Add(BitmapFrame.Create(rtBmp))
        Dim stream = New IO.MemoryStream()
        encoder.Save(stream)
        Dim bmp = New Bitmap(stream)
        Dim Hicon As IntPtr = bmp.GetHicon()
        Return Icon.FromHandle(Hicon)
    End Function

    Public Shared Function CreateIcon3() As Icon
        Dim dv As New DrawingVisual()
        Using dc As DrawingContext = dv.RenderOpen()
            Dim temperatureText = New FormattedText(My.Application.CurrentWeatherConditions.TemperatureRounded & "°",
                                                    New Globalization.CultureInfo("en-us"),
                                                    FlowDirection.LeftToRight,
                                                    Fonts.GetTypefaces(New Uri("pack://application:,,,/"), "./resources/").First(),
                                                    12, Media.Brushes.White, 1.0)
            dc.DrawText(temperatureText, New Windows.Point(0, 0))
        End Using
        Dim rbmp = New RenderTargetBitmap(16, 16, 96.0, 96.0, PixelFormats.Pbgra32)
        rbmp.Render(dv)
        Dim encoder = New PngBitmapEncoder()
        encoder.Frames.Add(BitmapFrame.Create(rbmp))
        Dim stream = New IO.MemoryStream()
        encoder.Save(stream)
        Dim bmp = New Bitmap(stream)
        Dim Hicon As IntPtr = bmp.GetHicon()
        Return Icon.FromHandle(Hicon)

    End Function

End Class
