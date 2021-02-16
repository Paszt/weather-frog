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

namespace weatherfrog
{
    public partial class App : Application, INotifyPropertyChanged
    {
        private static TaskbarIcon notifyIcon;
        //private static OptionsWindow optionsWindow = null;
        private static readonly TimeSpan UpdateWeatherInterval = TimeSpan.FromMinutes(10);
        private Timer updateWeatherTimer;
        private DesktopWallpaper desktopWallpaper;

        // Used for Singleton check
#pragma warning disable IDE0052 // Remove unread private members
        private static Mutex mutex = null;
#pragma warning restore IDE0052 // Remove unread private members

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

                //Resources.WeatherIconsDisplayWindow widw = new();
                //widw.Show();

                //Popupwindow pu = new();
                //pu.Show();

                //ForecastWindow fs = new();
                //fs.Show();

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
            updateWeatherTimer = new((e) => UpdateWeather(), null, TimeSpan.Zero, UpdateWeatherInterval);
            notifyIcon = new()
            {
                Icon = Utilities.CreateIcon(16, 16, (ImageSource)FindResource("FrogHeadDrawingImage")),
                DataContext = Current,
                ContextMenu = (System.Windows.Controls.ContextMenu)FindResource("NotifyIconMenu"),
                TrayPopup = new Resources.TaskbarBalloon(),
                //ToolTip = new Resources.TaskbarBalloon(),
            };
            MultiBinding toolTipMultiBinding = new MultiBinding()
            {
                StringFormat = "Weather Frog \n{0}° {1}\n{2}",
                Bindings = {
                    new Binding("Forecast.CurrentWeather.Temp") { Source = this },
                    new Binding("Forecast.CurrentWeather.Condition.Text") { Source = this },
                    new Binding("Forecast.Location.DisplayName") { Source = this },
                }
            };
            BindingOperations.SetBinding(notifyIcon, TaskbarIcon.ToolTipTextProperty, toolTipMultiBinding);

            SystemEvents sysEvents = new();
            sysEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
            sysEvents.ResumedFromSuspension += SystemEvents_ResumedFromSuspension;
            sysEvents.AboutToSuspend += SystemEvents_AboutToSuspend;

            desktopWallpaper = new(SystemParameters.WorkArea.Width, SystemParameters.WorkArea.Height);
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            if (My.Settings.ChangeDesktopBackground)
            {
                desktopWallpaper.Width = SystemParameters.WorkArea.Width;
                desktopWallpaper.Height = SystemParameters.WorkArea.Height;
                desktopWallpaper?.Update(Forecast);
            }
        }

        // update weather immediately upon waking up from suspension
        private void SystemEvents_ResumedFromSuspension(object sender, EventArgs e) =>
            updateWeatherTimer.Change(TimeSpan.Zero, UpdateWeatherInterval);

        private void SystemEvents_AboutToSuspend(object sender, EventArgs e)
        {
            if (My.Settings.ChangeDesktopBackground) desktopWallpaper?.Offline();
            notifyIcon.Icon = Utilities.CreateTaskbarIcon("??");
        }

        #region Properties

        private Forecast forecast;
        public Forecast Forecast
        {
            get => forecast;
            set { if (SetProperty(ref forecast, value)) NotifyPropertyChanged(nameof(BackgroundBrush)); }
        }

        public Brush BackgroundBrush => Forecast?.CurrentWeather?.BackgroundBrush is null
                    ? DefaultBackgroundBrush
                    : (Forecast?.CurrentWeather?.BackgroundBrush);

        // Used in NotifyIconResources.xaml > NotifyIconMenu
        public static Visibility IsDebug
        {
#if DEBUG
#pragma warning disable IDE0025 // Use expression body for properties
            get => Visibility.Visible;
#pragma warning restore IDE0025 // Use expression body for properties
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
                    if (My.Settings.ChangeDesktopBackground) desktopWallpaper.Update(Forecast);
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
