using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Media;
using weatherfrog.Infrastructure;
using weatherfrog.WeatherApi;

namespace weatherfrog.ViewModels
{
    class OptionsViewModel : WeatherApi.Models.BaseModel
    {
        public OptionsViewModel()
        {
            Location = My.Settings.Location;
            TaskbarIconStyle = My.Settings.TaskbarIconStyle;
            TestPassed = My.Settings.ApiKeyValidated;
            UnitSystem = My.Settings.UnitSystem;
            UpdateDesktop = My.Settings.UpdateDesktop;
            DisableWallpaperCompression = My.Settings.DisableWallpaperCompression;
            WeatherApiKey = My.Settings.WeatherApiKey;
            if (My.Settings.Locations is null)
                Locations = [];
            else
            {
                Locations = new ObservableCollection<string>(My.Settings.Locations);
                SortLocations();
            }
            Locations.CollectionChanged += Locations_CollectionChanged;
        }

        private void Locations_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged(nameof(IsLocationFavorited));
            NotifyPropertyChanged(nameof(IsLocationNotFavorited));
        }

        #region Properties

        #region Setting Properties

        private string location;
        public string Location
        {
            get => location;
            set
            {
                if (SetProperty(ref location, value))
                {
                    TestPassed = false;
                    OnSettingChanged();
                    NotifyPropertyChanged(nameof(IsLocationFavorited));
                    NotifyPropertyChanged(nameof(IsLocationNotFavorited));
                }
            }
        }

        private TaskbarIconStyle taskbarIconStyle = TaskbarIconStyle.Temperature;
        public TaskbarIconStyle TaskbarIconStyle
        {
            get => taskbarIconStyle;
            set { if (SetProperty(ref taskbarIconStyle, value)) OnSettingChanged(); }
        }

        private WeatherApi.Models.UnitSystem unitSystem = WeatherApi.Models.UnitSystem.Imperial;
        public WeatherApi.Models.UnitSystem UnitSystem
        {
            get => unitSystem;
            set { if (SetProperty(ref unitSystem, value)) OnSettingChanged(); }
        }

        private bool updateDesktop;
        public bool UpdateDesktop
        {
            get => updateDesktop;
            set { if (SetProperty(ref updateDesktop, value)) OnSettingChanged(); }
        }

        private bool disableWallpaperCompression;
        public bool DisableWallpaperCompression
        {
            get => disableWallpaperCompression;
            set { if (SetProperty(ref disableWallpaperCompression, value)) OnSettingChanged(); }
        }

        private string weatherApiKey;
        public string WeatherApiKey
        {
            get => weatherApiKey;
            set { if (SetProperty(ref weatherApiKey, value)) { TestPassed = false; OnSettingChanged(); } }
        }

        private ObservableCollection<string> locations;
        public ObservableCollection<string> Locations { get => locations; set => SetProperty(ref locations, value); }

        public bool IsLocationFavorited => Locations.Contains(Location);

        public bool IsLocationNotFavorited => !Locations.Contains(Location);

        #endregion

        #region UI Properties

        private bool testPassed;
        public bool TestPassed
        {
            get => testPassed;
            set { if (SetProperty(ref testPassed, value)) { NotifyPropertyChanged(nameof(TestNotPassed)); } }
        }

        public bool TestNotPassed => !testPassed;

        private ImageSource icon;
        public ImageSource Icon => icon ??= (ImageSource)System.Windows.Application.Current.FindResource("FrogHeadDrawingImage");

        public static Dictionary<WeatherApi.Models.UnitSystem, string> UnitChoices => new()
        {
            { WeatherApi.Models.UnitSystem.Imperial, "Imperial   (Fahrenheit, mph, in)" },
            { WeatherApi.Models.UnitSystem.Metric, "Metric     (Celcius, m/s, mm)" }
        };

        private List<WeatherApi.Models.SearchResult> searchResults;
        public List<WeatherApi.Models.SearchResult> SearchResults { get => searchResults; set => SetProperty(ref searchResults, value); }

        private bool isSearchResultsVisibile = false;
        public bool IsSearchResultsVisibile { get => isSearchResultsVisibile; set => SetProperty(ref isSearchResultsVisibile, value); }

        private string locationSearchMsg;
        public string LocationSearchMsg { get => locationSearchMsg; set => SetProperty(ref locationSearchMsg, value); }

        private string selectedLocation;
        public string SelectedLocation { get => selectedLocation; set => SetProperty(ref selectedLocation, value); }

        public static WeatherApi.Models.Forecast Forecast => App.Current.Forecast;

        #endregion

        #endregion

        #region Commands

        private RelayCommand testCommand;
        public RelayCommand TestCommand => testCommand ??= new RelayCommand(async () =>
        {
            Configuration.ApiKey = WeatherApiKey;
            try
            {
                LocationSearchMsg = string.Empty;
                SearchResults = await Api.LookupLocationAsync(Location);
                IsSearchResultsVisibile = true;
            }
            catch (System.Net.Http.HttpRequestException httpReqEx)
            {
                LocationSearchMsg = (httpReqEx.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    ? "The API Key appears to be incorrect. Please verify."
                    : httpReqEx.Message;
            }
            catch (System.Exception ex)
            {
                LocationSearchMsg = ex.Message;
            }
        }, () => !string.IsNullOrWhiteSpace(WeatherApiKey) && !string.IsNullOrWhiteSpace(Location) && TestNotPassed);

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(() =>
        {
            if (disableWallpaperCompression == true)
                DisableDesktopCompression();
            My.Settings.ApiKeyValidated = true;
            My.Settings.WeatherApiKey = WeatherApiKey;
            My.Settings.UnitSystem = UnitSystem;
            My.Settings.Location = Location;
            My.Settings.TaskbarIconStyle = TaskbarIconStyle;
            My.Settings.UpdateDesktop = UpdateDesktop;
            My.Settings.DisableWallpaperCompression = DisableWallpaperCompression;
            My.Settings.Save();
            Configuration.ApiKey = WeatherApiKey;
            // Weather data is updated in App.ShowOptionsCommand
        }, () => TestPassed | isDirty);

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand => cancelCommand ??= new RelayCommand(() =>
            Configuration.ApiKey = My.Settings.WeatherApiKey);

        private RelayCommand addLocationToFavoritesCommand;
        public RelayCommand AddLocationToFavoritesCommand => addLocationToFavoritesCommand ??= new RelayCommand(() =>
        {
            Locations.Add(Location);
            SortLocations();
            Locations_Changed();
        });

        private RelayCommand removeLocationFromFavoritesCommand;
        public RelayCommand RemoveLocationFromFavoritesCommand => removeLocationFromFavoritesCommand ??= new RelayCommand(() =>
        {
            string currentLocation = Location;
            Locations.Remove(Location);
            Location = currentLocation;
            Locations_Changed();
        });

        private void Locations_Changed()
        {
            My.Settings.Locations = new List<string>(Locations);
            My.Settings.Save();
            App.Current.Locations = My.Settings.Locations;
        }

        private RelayCommand browseToWeatherApiCommand;
        public RelayCommand BrowseToWeatherApiCommand => browseToWeatherApiCommand ??= new RelayCommand(
            () => Process.Start(new ProcessStartInfo("cmd",
                $"/c start {"https://www.weatherapi.com/signup.aspx"}")
            { CreateNoWindow = true }));

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

        private bool isDirty = false;
        private void OnSettingChanged() =>
            isDirty = !(My.Settings.Location == Location &&
                        My.Settings.TaskbarIconStyle == TaskbarIconStyle &&
                        My.Settings.UnitSystem == UnitSystem &&
                        My.Settings.UpdateDesktop == UpdateDesktop &&
                        My.Settings.DisableWallpaperCompression == DisableWallpaperCompression &&
                        My.Settings.WeatherApiKey == WeatherApiKey);

        private void SortLocations()
        {
            List<string> sortableList = new(Locations);
            sortableList.Sort();

            for (int i = 0; i < sortableList.Count; i++)
            {
                int newLocation = Locations.IndexOf(sortableList[i]);
                if (newLocation != i)
                    Locations.Move(Locations.IndexOf(sortableList[i]), i);
            }
        }

        private static void DisableDesktopCompression()
        {
            using RegistryKey deskTopKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            object? jpegImportQualityValue = deskTopKey.GetValue("JPEGImportQuality", null);
            if (jpegImportQualityValue is null or not 0x64)
                deskTopKey.SetValue("JPEGImportQuality", 0x64, RegistryValueKind.DWord);
            //if (Registry.CurrentUser.GetValue(@"Control Panel\Desktop\JPEGImportQuality", null) == null)
            //{
            //    using RegistryKey deskTopKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            //    deskTopKey.SetValue("JPEGImportQuality", 0x64, RegistryValueKind.DWord);
            //}
        }
    }
}
