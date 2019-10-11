Imports System.Runtime.Serialization

<DataContract>
Public Class CurrentWeather

    <DataMember(Name:="data")>
    Public Property Data As List(Of Datum)

    <DataMember(Name:="count")>
    Public Property Count As Integer

    <DataContract, Serializable>
    Public Class Datum

        <DataMember(Name:="wind_cdir")>
        Public Property WindDirectionAbbreviated As String

        <DataMember(Name:="rh")>
        Public Property RelativeHumidity As Integer

        ''' <summary>
        ''' The current part of the day. (d = day / n = night)
        ''' </summary>
        <DataMember(Name:="pod")>
        Public Property PartOfDay As String

        <DataMember(Name:="lon")>
        Public Property Longitude As String

        ''' <summary>
        ''' Atmospheric pressure in mb
        ''' </summary>
        <DataMember(Name:="pres")>
        Public Property Pressure As Double

        <DataMember(Name:="timezone")>
        Public Property Timezone As String

        ''' <summary>
        ''' Last observation time (YYYY-MM-DD HH:MM)
        ''' </summary>
        <DataMember(Name:="ob_time")>
        Public Property ObservationTime As String

        <DataMember(Name:="country_code")>
        Public Property CountryCode As String

        ''' <summary>
        ''' Cloud coverage in percent
        ''' </summary>
        <DataMember(Name:="clouds")>
        Public Property CloudCoverage As Integer

        <DataMember(Name:="vis")>
        Public Property Visability As Double

        <DataMember(Name:="wind_spd")>
        Public Property WindSpeed As Double

        <DataMember(Name:="wind_cdir_full")>
        Public Property WindDirectionFull As String

        ''' <summary>
        ''' Feels like
        ''' </summary>
        <DataMember(Name:="app_temp")>
        Public Property ApparentTemp As Double

        ''' <summary>
        ''' State abbreviation/code
        ''' </summary>
        <DataMember(Name:="state_code")>
        Public Property StateCode As String

        ''' <summary>
        ''' Last observation time (Unix timestamp)
        ''' </summary>
        <DataMember(Name:="ts")>
        Public Property TimeStamp As Integer

        ''' <summary>
        ''' Solar hour angle (degrees)
        ''' </summary>
        <DataMember(Name:="h_angle")>
        Public Property HourAngle As Integer

        <DataMember(Name:="dewpt")>
        Public Property DewPoint As Double

        <DataMember(Name:="weather")>
        Public Property Weather As Weather

        ''' <summary>
        ''' UV Index (0-11+)
        ''' </summary>
        <DataMember(Name:="uv")>
        Public Property UvIndex As Double

        ''' <summary>
        ''' Air Quality Index [US - EPA standard 0 - +500]
        ''' </summary>
        <DataMember(Name:="aqi")>
        Public Property AirQualityIndex As Integer

        ''' <summary>
        ''' Source station ID
        ''' </summary>
        <DataMember(Name:="station")>
        Public Property Station As String

        ''' <summary>
        ''' Wind direction (degrees)
        ''' </summary>
        <DataMember(Name:="wind_dir")>
        Public Property WindDirection As Integer

        ''' <summary>
        ''' Solar elevation angle (degrees)
        ''' </summary>
        <DataMember(Name:="elev_angle")>
        Public Property ElevationAngle As Double

        ''' <summary>
        ''' Current cycle hour (YYYY-MM-DD:HH)
        ''' </summary>
        <DataMember(Name:="datetime")>
        Public Property Datetime As String

        ''' <summary>
        ''' Liquid equivalent precipitation rate (default mm/hr)
        ''' </summary>
        <DataMember(Name:="precip")>
        Public Property PrecipitationRate As Integer

        ''' <summary>
        ''' Global horizontal solar irradiance (W/m^2) [Clear Sky]
        ''' </summary>
        <DataMember(Name:="ghi")>
        Public Property GlobalHorizontalSolarIrradiance As Double

        ''' <summary>
        ''' Direct normal solar irradiance (W/m^2) [Clear Sky]
        ''' </summary>
        <DataMember(Name:="dni")>
        Public Property DirectNormalSolarIrradiance As Double

        ''' <summary>
        ''' Diffuse horizontal solar irradiance (W/m^2) [Clear Sky]
        ''' </summary>
        <DataMember(Name:="dhi")>
        Public Property DiffuseHorizontalSolarIrradiance As Double

        ''' <summary>
        ''' Estimated Solar Radiation (W/m^2)
        ''' </summary>
        <DataMember(Name:="solar_rad")>
        Public Property SolarRadiation As Double

        <DataMember(Name:="city_name")>
        Public Property CityName As String

        ''' <summary>
        ''' Sunrise time (HH:MM)
        ''' </summary>
        <DataMember(Name:="sunrise")>
        Public Property SunriseTime As String

        ''' <summary>
        ''' Sunset time (HH:MM)
        ''' </summary>
        <DataMember(Name:="sunset")>
        Public Property SunsetTime As String

        <DataMember(Name:="temp")>
        Public Property Temperature As Double

        <IgnoreDataMember>
        Public ReadOnly Property TemperatureRounded As Integer
            Get
                Return CInt(Temperature)
            End Get
        End Property

        <DataMember(Name:="lat")>
        Public Property Latitude As String

        ''' <summary>
        ''' Sea level pressure (mb)
        ''' </summary>
        <DataMember(Name:="slp")>
        Public Property SeaLevelPressure As Double

        <IgnoreDataMember>
        Public ReadOnly Property WeatherIcon As DrawingImage
            Get
                Select Case Weather.Icon
                    Case "c01d"
                        Return CType(Windows.Application.Current.FindResource("SunnyDrawingImage"), DrawingImage)
                    Case "c01n"
                        Return CType(Windows.Application.Current.FindResource("ClearNightDrawingImage"), DrawingImage)
                    Case "c02d"
                        Return CType(Windows.Application.Current.FindResource("MostlySunnyDrawingImage"), DrawingImage)
                    Case "c02n"
                        Return CType(Windows.Application.Current.FindResource("PeriodicCloudsNightDrawingImage"), DrawingImage)
                    Case "c03d"
                        Return CType(Windows.Application.Current.FindResource("MostlyCloudyDayDrawingImage"), DrawingImage)
                    Case "c03n"
                        Return CType(Windows.Application.Current.FindResource("MostlyCloudyNightDrawingImage"), DrawingImage)
                    Case "c04d", "c04n"
                        Return CType(Windows.Application.Current.FindResource("CloudyDrawingImage"), DrawingImage)
                    Case "a01d", "a01n", "a02d", "a02n", "a03d", "a03n", "a04d", "a04n", "a05d", "a05n", "a06d", "a06n"
                        Return CType(Windows.Application.Current.FindResource("HazeDrawingImage"), DrawingImage)
                    Case Else
                        Return CType(Windows.Application.Current.FindResource("UnknownDrawingImage"), DrawingImage)
                End Select
            End Get
        End Property


    End Class

    <DataContract>
    Public Class Weather

        <DataMember(Name:="icon")>
        Public Property Icon As String

        <DataMember(Name:="code")>
        Public Property Code As String

        <DataMember(Name:="description")>
        Public Property Description As String

    End Class

End Class
