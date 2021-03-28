// notifyicon for .Net Core 3.1 and .Net 5 WPF: https://github.com/HavenDV/Hardcodet.NotifyIcon.Wpf.NetCore

using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using weatherfrog.WeatherApi.Models;
using weatherfrog.Infrastructure;
using System.Windows.Data;
using System.Windows.Media;
using System.IO;
using System.Text.Json;
using weatherfrog.Resources;
using System.Xaml.Schema;
using System.Collections.Generic;

namespace weatherfrog
{
    public partial class App : Application, INotifyPropertyChanged
    {
        private static TaskbarIcon notifyIcon;
        private static readonly TimeSpan UpdateWeatherInterval = TimeSpan.FromMinutes(5);
        private Timer updateWeatherTimer;
        private DesktopWallpaper desktopWallpaper;

        // Used for Singleton check
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "mutex is used in separate instance")]
        private static Mutex mutex = null;

        public new static App Current => (App)Application.Current;
        public static Brush DefaultBackgroundBrush =>
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9EABA2"));

        void App_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0 && e.Args[0] == "-i")
            {
                Illustrations.IllustrationWindow illustrationWindow = new();
                illustrationWindow.ShowDialog();
                Current.Shutdown();
            }
            else if (e.Args.Length > 0 && e.Args[0] == "-f")
            {
                Illustrations.FilenameEditorWindow filenameEditorWindow = new();
                filenameEditorWindow.ShowDialog();
                Current.Shutdown();
            }
            else
            {
                // Singleton check
                const string appName = "paszt:weather-frog";
                mutex = new Mutex(true, appName, out bool createdNew);
                if (!createdNew)
                {
                    //app is already running! Exiting the application  
                    MessageBox.Show("Weather Frog is already running. Check the system tray in the task bar.",
                        "Weather Frog Already Running", MessageBoxButton.OK, MessageBoxImage.Information);
                    Current.Shutdown();
                }

                WeatherApi.Configuration.UserAgent = typeof(App).Assembly.GetName().Name + "/" +
                    typeof(App).Assembly.GetName().Version.ToString() + " (https://github.com/Paszt/weather-frog)";

                if (My.Settings.ApiKeyValidated) Begin();
                else
                {
                    if (OptionsWindow.Instance.ShowDialog() == false) Current?.Shutdown();
                    else Begin();
                }
            }
        }

        private void Begin()
        {
            WeatherApi.Configuration.ApiKey = My.Settings.WeatherApiKey;
            notifyIcon = new()
            {
                Icon = Utilities.CreateIcon(16, 16, (ImageSource)FindResource("FrogHeadDrawingImage")),
                DataContext = Current,
                ContextMenu = (System.Windows.Controls.ContextMenu)FindResource("NotifyIconMenu"),
                TrayPopup = new TaskbarBalloon(),
                //ToolTip = new Resources.TaskbarBalloon(),
            };
            // Add bindings to notifyicon
            MultiBinding toolTipMultiBinding = new MultiBinding()
            {
                StringFormat = "Weather Frog \n{0}° {1}\n{2}\nLast updated: {3}",
                Bindings = {
                    new Binding("Forecast.CurrentWeather.Temp") { Source = this },
                    new Binding("Forecast.CurrentWeather.Condition.Text") { Source = this },
                    new Binding("Forecast.Location.DisplayName") { Source = this },
                    new Binding(nameof(LastUpdatedTimeString)) {Source = this},
                }
            };
            BindingOperations.SetBinding(notifyIcon, TaskbarIcon.ToolTipTextProperty, toolTipMultiBinding);

            Locations = My.Settings.Locations;

            SystemEvents sysEvents = new();
            sysEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
            sysEvents.ResumedFromSuspension += SystemEvents_ResumedFromSuspension;
            sysEvents.AboutToSuspend += SystemEvents_AboutToSuspend;

            if (My.Settings.UpdateDesktop)
                desktopWallpaper = DesktopWallpaper.FromSystemParameters();

            updateWeatherTimer = new((e) => UpdateWeather(), null, TimeSpan.Zero, UpdateWeatherInterval);
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e) =>
            desktopWallpaper?.Update(forecast, true);

        // update weather immediately upon waking up from suspension
        private void SystemEvents_ResumedFromSuspension(object sender, EventArgs e) =>
            updateWeatherTimer.Change(TimeSpan.Zero, UpdateWeatherInterval);

        private void SystemEvents_AboutToSuspend(object sender, EventArgs e)
        {
            desktopWallpaper?.Offline();
            notifyIcon.Icon = Utilities.CreateTaskbarIcon("??");
        }

        #region Properties

        private Forecast forecast;
        public Forecast Forecast
        {
            get => forecast;
            set
            {
                if (SetProperty(ref forecast, value))
                {
                    NotifyPropertyChanged(nameof(BackgroundBrush));
                    NotifyPropertyChanged(nameof(LastUpdatedTimeString));
                }
            }
        }

        public Brush BackgroundBrush => Forecast?.CurrentWeather?.BackgroundBrush is null
            ? DefaultBackgroundBrush
            : (Forecast?.CurrentWeather?.BackgroundBrush);

        public string LastUpdatedTimeString =>
            !string.IsNullOrEmpty(Forecast?.CurrentWeather?.LastUpdated) &&
            DateTimeOffset.TryParse(Forecast.CurrentWeather?.LastUpdated, out DateTimeOffset dto)
                ? dto.ToString("t")
                : null;

        private List<string> locations;
        /// <summary>The taskbar icon Locations context menu is bound to this.</summary>
        public List<string> Locations { get => locations; set => SetProperty(ref locations, value); }

        // Used in NotifyIconResources.xaml > NotifyIconMenu
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0025:Use expression body for properties", Justification = "#if Directive")]
        public static Visibility IsDebug
        {
#if DEBUG
            get => Visibility.Visible;
#else
            get => Visibility.Collapsed;
#endif
        }

        #endregion

        private async void UpdateWeather()
        {
            if (My.Settings.ApiKeyValidated)
            {
                try
                {
                    Forecast = await WeatherApi.Api.GetForecastAsync(My.Settings.Location);
                    // TODO: Find out why Forecast is null after resume from suspension. Remove this:
                    if (Forecast == null) throw new ArgumentException("After calling GetForecastAsync, Forecast is Null!");
                    // write Forecast json to file (for debug). Not including #if DEBUG directive here so that
                    // any alpha/beta testers can have acces to the file.
                    string forecastPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "forecast.json");
                    try
                    {
                        File.WriteAllText(forecastPath, JsonSerializer.Serialize(Forecast, new() { WriteIndented = true }));
                    }
                    catch (Exception) { }

                    notifyIcon.Icon = Utilities.CreateTaskbarIcon(forecast.CurrentWeather);

                    // This method, UpdateWeather, is called after the options window is closed with changes. 
                    // Check the state of desktopWallpaper and Settings.UpdateDesktop to determine if 
                    // the user just changed the setting.
                    if (desktopWallpaper != null && !My.Settings.UpdateDesktop)
                    {
                        desktopWallpaper.Offline("Desktop Updating is turned off.");
                        desktopWallpaper = null;
                    }
                    else if (desktopWallpaper == null && My.Settings.UpdateDesktop)
                        desktopWallpaper = DesktopWallpaper.FromSystemParameters();

                    if (forecast?.CurrentWeather != null) desktopWallpaper?.Update(Forecast);
                    else desktopWallpaper?.Offline("Something went wrong.\nCurrent weather is not available.");
                }
                catch (Exception)
                {
                    Forecast = null;
                    desktopWallpaper.NetworkError();
                    updateWeatherTimer.Change(TimeSpan.FromSeconds(5), UpdateWeatherInterval);
                }
            }
            else
            {
                throw new Exception("API key has not been validated.");
            }
        }

        #region notify icon commands

        private RelayCommand exitAppCommand;
        public RelayCommand ExitAppCommand => exitAppCommand ??= new(() => Current.Shutdown());

        private RelayCommand showOptionsCommand;
        public RelayCommand ShowOptionsCommand => showOptionsCommand ??= new(() =>
        {
            if (OptionsWindow.Instance.ShowDialog().Value)
                updateWeatherTimer.Change(TimeSpan.Zero, UpdateWeatherInterval);
        });

        private RelayCommand showForecastCommand;
        public RelayCommand ShowForecastCommand => showForecastCommand ??= new(() => ForecastWindow.Instance?.Show());

        private RelayCommand showWeatherIconsWindowCommand;
        public RelayCommand ShowWeatherIconsWindowCommand => showWeatherIconsWindowCommand ??= new(() =>
            WeatherIconsDisplayWindow.Instance?.Show());

        private RelayCommand showPopupWindowCommand;
        public RelayCommand ShowPopupWindowCommand => showPopupWindowCommand ??= new(() => Popupwindow.Instance.Show());

        private RelayCommand<string> changeLocationCommand;
        public RelayCommand<string> ChangeLocationCommand => changeLocationCommand ??=
            new RelayCommand<string>(value =>
            {
                My.Settings.Location = value;
                updateWeatherTimer.Change(TimeSpan.Zero, UpdateWeatherInterval);
            });

        #endregion

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            updateWeatherTimer?.Dispose();
            desktopWallpaper?.Offline();
            //TODO: Add option to let user select if they want to see somethingwrong.png or revert back to desktop state 
            //      before Weather Frog was run. Will need to figure out how to read, save, and re-apply that state; 
            //      probably reading/writing from/to the registry.
        }

        #region INotifyPropertyChanged

        /// <summary>
        /// Property changed event for observer pattern
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises event when a property is changed
        /// </summary>
        /// <param name="propertyName">Name of the changed property</param>
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;
            storage = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}
