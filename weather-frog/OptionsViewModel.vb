Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports weatherfrog.Infrastructure

Namespace ViewModels

    Public Class OptionsViewModel
        Implements INotifyPropertyChanged

        Public Sub New()
            City = My.Settings.City
            Units = My.Settings.Units
            WeatherbitApiKey = My.Settings.WeatherbitApiKey
            TaskbarIconStyle = CType(My.Settings.TaskbarIconStyle, TaskbarIconStyle)
        End Sub

#Region " Properties "

        Private _isDirty As Boolean = False
        Public Property IsDirty As Boolean
            Get
                Return _isDirty
            End Get
            Set(value As Boolean)
                If SetProperty(_isDirty, value) Then
                    OnPropertyChanged("IsNotDirty")
                End If
            End Set
        End Property

        Public ReadOnly Property IsNotDirty As Boolean
            Get
                Return Not IsDirty
            End Get
        End Property

        Private _weatherbitApiKey As String
        Public Property WeatherbitApiKey As String
            Get
                Return _weatherbitApiKey
            End Get
            Set(value As String)
                SetProperty(_weatherbitApiKey, value)
            End Set
        End Property

        Private _units As String = "I"
        Public Property Units As String
            Get
                Return _units
            End Get
            Set(value As String)
                SetProperty(_units, value)
            End Set
        End Property

        Private _city As String
        Public Property City As String
            Get
                Return _city
            End Get
            Set(value As String)
                SetProperty(_city, value)
            End Set
        End Property

        Private _taskbarIconStyle As TaskbarIconStyle
        Public Property TaskbarIconStyle As TaskbarIconStyle
            Get
                Return _taskbarIconStyle
            End Get
            Set(value As TaskbarIconStyle)
                SetProperty(_taskbarIconStyle, value)
            End Set
        End Property


        Public Property UnitChoices As New Dictionary(Of String, String) From {
            {"M", "Metric     (Celcius, m/s, mm)"},
            {"S", "Scientific (Kelvin, m/s, mm)"},
            {"I", "Imperial   (Fahrenheit, mph, in)"}
        }

#End Region

        Public ReadOnly Property UpdateOptionsCommand As ICommand
            Get
                Return New RelayCommand(
                    Sub()
                        My.Settings.City = City
                        My.Settings.Units = Units
                        My.Settings.WeatherbitApiKey = WeatherbitApiKey
                        My.Settings.TaskbarIconStyle = TaskbarIconStyle
                        My.Settings.Save()
                        My.Application.CloseOptions(True)
                    End Sub,
                    Function() As Boolean
                        Return IsDirty
                    End Function)
            End Get
        End Property

        Public ReadOnly Property CancelCommand As ICommand
            Get
                Return New RelayCommand(Sub()
                                            My.Application.CloseOptions()
                                        End Sub)
            End Get
        End Property

#Region " INotifyPropertyChanged "

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Protected Function SetProperty(Of T)(ByRef storage As T, value As T,
                                     <CallerMemberName> Optional propertyName As String = Nothing) As Boolean
            If Equals(storage, value) Then
                Return False
            End If
            storage = value
            OnPropertyChanged(propertyName)
            Return True
        End Function

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional propertyName As String = Nothing)
            Dim propertyChanged As PropertyChangedEventHandler = PropertyChangedEvent
            If propertyChanged IsNot Nothing Then
                propertyChanged(Me, New PropertyChangedEventArgs(propertyName))
            End If
            'update isDirty
            If propertyName IsNot "IsDirty" Then
                IsDirty = Not (My.Settings.City = City AndAlso
                               My.Settings.Units = Units AndAlso
                               My.Settings.WeatherbitApiKey = WeatherbitApiKey AndAlso
                               My.Settings.TaskbarIconStyle = TaskbarIconStyle)
            End If
        End Sub

#End Region

    End Class

End Namespace
