﻿// notifyicon for .Net Core 3.1 and .Net 5 WPF: https://github.com/HavenDV/Hardcodet.NotifyIcon.Wpf.NetCore

// restclient: https://github.com/NimaAra/Easy.Common/blob/master/Easy.Common/RestClient.cs  

using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using weatherfrog.WeatherApi.Models;
using weatherfrog.Infrastructure;

namespace weatherfrog
{
    public partial class App : Application, INotifyPropertyChanged
    {
        private static TaskbarIcon notifyIcon;
        private static OptionsWindow optionsWindow = null;
        private static readonly TimeSpan UpdateWeatherInterval = TimeSpan.FromMinutes(10);
        private Timer updateWeatherTimer;

        // Used for Singleton check
        private static Mutex mutex = null;
        public new static App Current { get { return (App)Application.Current; } }

        private static OptionsWindow OptionsWindowInstance
        {
            get
            {
                if (null == optionsWindow ||
                    (bool)typeof(Window).GetProperty("IsDisposed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(optionsWindow))
                { optionsWindow = new(); }
                if (optionsWindow.IsLoaded)
                { return null; }
                else
                { return optionsWindow; }
            }
        }

        void App_Startup(object sender, StartupEventArgs e)
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
            Popupwindow pu = new();
            pu.Show();

            ForecastWindow fs = new();
            fs.Show();

            if (My.Settings.ApiKeyValidated)
            { Begin(); }
            else
            {
                if (OptionsWindowInstance.ShowDialog() == false)
                { Current?.Shutdown(); }
                else
                { Begin(); }
            }
        }

        private void Begin()
        {
            WeatherApi.Configuration.ApiKey = My.Settings.WeatherApiKey;
            updateWeatherTimer = new((e) => UpdateWeather(), null, TimeSpan.Zero, UpdateWeatherInterval);
            notifyIcon = new()
            {
                Icon = Utilities.CreateIcon(16, 16, (System.Windows.Media.ImageSource)FindResource("FrogHeadDrawingImage")),
                DataContext = Current,
                ContextMenu = (System.Windows.Controls.ContextMenu)FindResource("NotifyIconMenu"),
                TrayPopup = new Resources.TaskbarBalloon(),
                ToolTip = new Resources.TaskbarBalloon(),
            };
        }

        #region Properties

        private Forecast forecast;
        public Forecast Forecast { get => forecast; set => SetProperty(ref forecast, value); }

        #endregion

        private async void UpdateWeather()
        {
            if (My.Settings.ApiKeyValidated)
            {
                Forecast = await WeatherApi.API.GetForecastAsync(My.Settings.Location);
                if (My.Settings.ChangeDesktopBackground) DesktopWallpaper.Update(Forecast);
                notifyIcon.Icon = Utilities.CreateTaskbarIcon(forecast.CurrentWeather);
            }
            else
            {
                throw new Exception("API key has not been validated.");
            }
        }

        #region notify icon commands

        private RelayCommand exitAppCommand;
        public RelayCommand ExitAppCommand => exitAppCommand ??= new RelayCommand(() => { Current.Shutdown(); });

        private RelayCommand showOptionsCommand;
        public RelayCommand ShowOptionsCommand => showOptionsCommand ??= new RelayCommand(() =>
        {
            OptionsWindow ow = OptionsWindowInstance;
            if (ow != null && ow.ShowDialog().Value)
            {
                updateWeatherTimer.Change(TimeSpan.Zero, UpdateWeatherInterval);
            };
        });

        #endregion

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            updateWeatherTimer?.Dispose();
            //TODO: Change desktop background to SomethingWrong.png image with message that Weather Frog is not running. 
            //      Add option to let user select if they want to see somethingwrong.png or revert back to desktop state 
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
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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