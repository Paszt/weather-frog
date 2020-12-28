﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;
using weatherfrog.Infrastructure;
using weatherfrog.WeatherApi;

namespace weatherfrog
{
    class OptionsViewModel : WeatherApi.Models.BaseModel
    {
        public OptionsViewModel()
        {
            Icon = (ImageSource)System.Windows.Application.Current.FindResource("FrogHeadDrawingImage");
            WeatherApiKey = My.Settings.WeatherApiKey;
            Location = My.Settings.Location;
            UnitSystem = My.Settings.UnitSystem;
            TaskbarIconStyle = My.Settings.TaskbarIconStyle;
            TestPassed = My.Settings.ApiKeyValidated;
            ChangeDesktopBackground = My.Settings.ChangeDesktopBackground;
        }

        #region Properties

        private bool testPassed;
        public bool TestPassed
        {
            get => testPassed;
            set
            {
                if (SetProperty(ref testPassed, value))
                {
                    NotifyPropertyChanged(nameof(TestNotPassed));
                };
            }
        }

        private System.Windows.Media.ImageSource icon;
        public ImageSource Icon { get => icon; set => SetProperty(ref icon, value); }

        public bool TestNotPassed => !testPassed;

        private string weatherApiKey;
        public string WeatherApiKey
        {
            get => weatherApiKey;
            set { if (SetProperty(ref weatherApiKey, value)) { TestPassed = false; } }
        }

        private WeatherApi.Models.UnitSystem unitSystem = WeatherApi.Models.UnitSystem.Imperial;
        public WeatherApi.Models.UnitSystem UnitSystem { get => unitSystem; set => SetProperty(ref unitSystem, value); }

        private string location;
        public string Location
        {
            get => location;
            set { if (SetProperty(ref location, value)) { TestPassed = false; } }
        }

        private TaskbarIconStyle taskbarIconStyle = TaskbarIconStyle.Temperature;
        public TaskbarIconStyle TaskbarIconStyle { get => taskbarIconStyle; set => SetProperty(ref taskbarIconStyle, value); }

        private bool changeDesktopBackground;
        public bool ChangeDesktopBackground { get => changeDesktopBackground; set => SetProperty(ref changeDesktopBackground, value); }

        public static Dictionary<WeatherApi.Models.UnitSystem, string> UnitChoices => new()
        {
            { WeatherApi.Models.UnitSystem.Imperial, "Imperial   (Fahrenheit, mph, in)" },
            { WeatherApi.Models.UnitSystem.Metric,   "Metric     (Celcius, m/s, mm)" }
        };

        private List<WeatherApi.Models.SearchResult> searchResults;
        public List<WeatherApi.Models.SearchResult> SearchResults { get => searchResults; set => SetProperty(ref searchResults, value); }

        private bool isSearchResultsVisibile = false;
        public bool IsSearchResultsVisibile { get => isSearchResultsVisibile; set => SetProperty(ref isSearchResultsVisibile, value); }

        private string locationSearchMsg;
        public string LocationSearchMsg { get => locationSearchMsg; set => SetProperty(ref locationSearchMsg, value); }

        private string selectedLocation;
        public string SelectedLocation { get => selectedLocation; set => SetProperty(ref selectedLocation, value); }

        #endregion

        #region Commands

        private RelayCommand testCommand;
        public RelayCommand TestCommand => testCommand ??= new RelayCommand(async () =>
        {
            Configuration.ApiKey = WeatherApiKey; 
            try
            {
                LocationSearchMsg = string.Empty;
                SearchResults = await API.LookupLocationAsync(Location);
                IsSearchResultsVisibile = true;
            }
            catch (System.Net.Http.HttpRequestException httpReqEx)
            {
                if (httpReqEx.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    LocationSearchMsg = "The API Key appears to be incorrect. Please verify.";
                }
                else
                {
                    LocationSearchMsg = httpReqEx.Message;
                }
            }
            catch (System.Exception ex)
            {
                LocationSearchMsg = ex.Message;
            }


        }, () => !string.IsNullOrWhiteSpace(WeatherApiKey) && !string.IsNullOrWhiteSpace(Location) && !TestPassed);

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(() =>
        {
            My.Settings.ApiKeyValidated = true;
            My.Settings.WeatherApiKey = weatherApiKey;
            My.Settings.UnitSystem = UnitSystem;
            My.Settings.Location = Location;
            My.Settings.TaskbarIconStyle = TaskbarIconStyle;
            My.Settings.Save();
            Configuration.ApiKey = WeatherApiKey;
            // Weather data is updated in App.ShowOptionsCommand
        }, () => TestPassed);

        private RelayCommand browseToWeatherApiCommand;
        public RelayCommand BrowseToWeatherApiCommand => browseToWeatherApiCommand ??= new RelayCommand(
            () => Process.Start(new ProcessStartInfo("cmd", $"/c start {"https://www.weatherapi.com/signup.aspx"}") { CreateNoWindow = true }));

        private RelayCommand cancelLocationSearchCommand;
        public RelayCommand CancelLocationSearchCommand => cancelLocationSearchCommand ??= new RelayCommand(
            () => IsSearchResultsVisibile = false);

        private RelayCommand selectLocationCommand;
        public RelayCommand SelectLocationCommand => selectLocationCommand ??= new RelayCommand(() =>
        {
            IsSearchResultsVisibile = false;
            Location = SelectedLocation.Replace(", United States of America", string.Empty);
            TestPassed = true;
        }, () => !string.IsNullOrEmpty(SelectedLocation));

        #endregion

    }
}