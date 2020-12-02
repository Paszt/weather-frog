Imports System.Device.Location
Imports Microsoft.Maps.MapControl.WPF
Imports weatherfrog.Extensions

'https://api.weather.gov/openapi.json
'https://www.weather.gov/documentation/services-web-api (click Specification Tab)
'https://api.weather.gov/stations/{observation-station}/observations/current
'https://api.weather.gov/points/35.516,-77.039/stations - get a list of observation stations for a given location (forwards)  


'' https://api.weather.gov/points/35.516,-77.039 
'' use properties > forecast from response of above points call,
'' https://api.weather.gov/gridpoints/MHX/41,59/forecast
'' the response from the above url gives you the geometry For the appropriate 2.5km grid used for the forecast, as well as a 7 day forecast

'' (note: https://api.weather.gov/points/35.516,-77.039/forecast will forward to https://api.weather.gov/gridpoints/MHX/41,59/forecast)

'' use properties > forecastZone from points call which gives a url to get list of observationStations
'' https://api.weather.gov/zones/forecast/NCZ080 
'' the observationStation selected by the user will be used for current conditions (observations).
'' call each resulting url in properties>obervationStations to get point coordinates of the obervationStation
'' https://api.weather.gov/stations/KOCW  
'' https://api.weather.gov/stations/KEWN
'' etc.

'' to get current weather, use obervationStation from above: 
'' https://api.weather.gov/stations/KOCW/observations/latest  


Public Class StationSelector

    Private ReadOnly centerPushPin As New Pushpin
    Private ReadOnly forecastZoneLayer As New MapLayer
    Private ReadOnly observationStationLayer As New MapLayer

    Private Const pointApiUrlBase = "https://api.weather.gov/points/"
    Private Const forecastUrlFormat = "https://api.weather.gov/gridpoints/{0}/{1},{2}/forecast"

    Public Property Stations As New ObjectModel.ObservableCollection(Of NWS.ObservationStation)

    Private Sub Search_Click(sender As Object, e As RoutedEventArgs)
        'Dim Polygon = New MapPolygon With {
        '    .Fill = New SolidColorBrush(Colors.Blue),
        '    .Opacity = 0.5,
        '    .Locations = New LocationCollection() From {
        '        New Location(35.5291, -77.0641),
        '        New Location(35.5069, -77.0677),
        '        New Location(35.5039, -77.0404),
        '        New Location(35.5262, -77.0368),
        '        New Location(35.5291, -77.0641)
        '    }
        '}
        'forecastZoneLayer.Children.Clear()
        'forecastZoneLayer.Children.Add(Polygon)

        'Dim pin As New Pushpin With {.Location = New Location(35.532862, -77.03158)}
        'observationStationLayer.Children.Add(pin)





        '' get grid properties and forecast zone
        Dim json As String = Infrastructure.WebResources.DownloadString(pointApiUrlBase & GetCurrentLocationString())
        Dim p As NWS.GridPoint = json.FromJSONDotNet(Of NWS.GridPoint)

        ' use forecast zone to get observation stations
        json = Infrastructure.WebResources.DownloadString(p.Properties.ForecastZone)
        Dim fz As NWS.ForecastZone = json.FromJSONDotNet(Of NWS.ForecastZone)

        observationStationLayer.Children.Clear()
        Stations.Clear()
        For Each stationUrl In fz.Properties.ObservationStations
            json = Infrastructure.WebResources.DownloadString(stationUrl)
            Dim station As NWS.ObservationStation = json.FromJSONDotNet(Of NWS.ObservationStation)
            Dim pin As New Pushpin() With {
                .Location = New Location(Convert.ToDouble(station.Geometry.Coordinates(1).ToString("N3")),
                                         Convert.ToDouble(station.Geometry.Coordinates(0).ToString("N3"))),
                .Content = station.Properties.StationIdentifier
            }
            AddHandler pin.MouseLeftButtonUp, AddressOf Station_MouseUp

            observationStationLayer.Children.Add(pin)
            Stations.Add(station)
        Next

        Dim forecastUrl = String.Format(forecastUrlFormat, p.Properties.GridId, p.Properties.GridX, p.Properties.GridY)
        json = Infrastructure.WebResources.DownloadString(forecastUrl)
        Dim gpf As NWS.GridpointForecast = json.FromJSONDotNet(Of NWS.GridpointForecast)
        UpdateGridForecastPolygon(gpf)
    End Sub

    Private Sub UpdateGridForecastPolygon(gpf As NWS.GridpointForecast)
        forecastZoneLayer.Children.Clear()
        forecastZoneLayer.Children.Add(New MapPolygon With {
                                       .Fill = New SolidColorBrush(Colors.Blue),
                                       .Opacity = 0.5,
                                       .Locations = gpf.Geometry.ToLocationCollection})
    End Sub

    Private Sub Station_MouseUp(sender As Object, e As MouseButtonEventArgs)
        Dim pin As Pushpin = DirectCast(sender, Pushpin)
        MessageBox.Show(pin.Content.ToString())
    End Sub

    Private Sub Map_ViewChangeOnFrame(sender As Object, e As MapEventArgs) Handles Map.ViewChangeOnFrame
        LatLong.Text = GetCurrentLocation().ToString()
        'centerPushPin.Location = GetCurrentLocation()

    End Sub

    'Private Function GetLatLong() As String
    '    Return Math.Round(Map.BoundingRectangle.Center.Latitude, 4) & "," & Math.Round(Map.BoundingRectangle.Center.Longitude, 4)
    'End Function

    Private Function GetCurrentLocation() As Location
        Return New Location(Math.Round(Map.BoundingRectangle.Center.Latitude, 4), Math.Round(Map.BoundingRectangle.Center.Longitude, 4))
    End Function

    Private Function GetCurrentLocationString() As String
        Return Math.Round(Map.BoundingRectangle.Center.Latitude, 4) & "," & Math.Round(Map.BoundingRectangle.Center.Longitude, 4)
    End Function

    Private Sub StationSelector_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim watcher = New GeoCoordinateWatcher()
        If watcher.TryStart(False, TimeSpan.FromSeconds(3)) Then
            If Not Double.IsNaN(watcher.Position.Location.Latitude) Then
                Map.SetView(New Location(watcher.Position.Location.Latitude, watcher.Position.Location.Longitude), 10.0)
            Else
                Map.SetView(New Location(38.4, -98.8), 4.0)
            End If
        End If


        testImg.Source = GetPushpinImage(Colors.Red, Colors.White, Colors.White).Source

        Map.Children.Add(forecastZoneLayer)
        Map.Children.Add(observationStationLayer)
        Map.Children.Add(centerPushPin)
        'centerPushPinImgLayer = New MapLayer
        'centerPushPinImg = GetPushpinImage(Colors.Red, Colors.White)
        'centerPushPinImgLayer.AddChild(centerPushPinImg, GetCurrentLocation, PositionOrigin.BottomCenter)
        'Map.Children.Add(centerPushPinImgLayer)

        StationsListView.ItemsSource = Stations
    End Sub

    Private Function GetPushpinImage(baseColor As Color,
                                     dotColor As Color,
                                     Optional outlineColor As Color = Nothing) As Image
        Dim dg As New DrawingGroup()
        'add base
        Dim outlinePen As New Pen
        If outlineColor <> Nothing Then
            outlinePen = New Pen(New SolidColorBrush(outlineColor), 2.0)
        End If
        dg.Children.Add(New GeometryDrawing(New SolidColorBrush(baseColor),
                                            outlinePen,
                                            Geometry.Parse("F1 M 14,40C 16.3747,40 15.56,35.196 19.848,30.1267C 24.7147,24.3733 28,21.6666 28,14C 28,6.26794 21.732,0 14,0C 6.268,0 0,6.26794 0,14C 0,21.6666 3.28533,24.3733 8.152,30.1267C 12.44,35.196 11.6253,40 14,40 Z ")))
        'Add Dot
        dg.Children.Add(New GeometryDrawing(New SolidColorBrush(dotColor),
                                            Nothing,
                                            New EllipseGeometry(New Point(14, 14), 4, 4)))
        Dim di As New DrawingImage(dg)
        di.Freeze()
        Return New Image With {.Source = di, .Width = 21, .Height = 30}
    End Function

    Private Sub MyLocationButton_Click(sender As Object, e As RoutedEventArgs) Handles MyLocationButton.Click
        Dim watcher = New GeoCoordinateWatcher()
        If watcher.TryStart(False, TimeSpan.FromSeconds(3)) Then
            If Not Double.IsNaN(watcher.Position.Location.Latitude) Then
                Map.SetView(New Location(watcher.Position.Location.Latitude, watcher.Position.Location.Longitude), 5.0)
            End If
        End If
    End Sub

End Class
