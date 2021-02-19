using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using weatherfrog.Infrastructure;
using weatherfrog.WeatherApi.Models;

namespace weatherfrog.Illustrations
{
    //TODO: Remove Debug statements.
    public class IllustrationViewModel : INotifyPropertyChanged
    {
        private readonly DesktopWallpaper desktopWallpaper;

        public IllustrationViewModel()
        {
            desktopWallpaper = new(Width, Height, WorkArea) { DrawTaskbar = true };
            Forecast = new()
            {
                Location = new()
                {
                    Name = "Monkeys Eyebrow",
                    Region = "Kentucky"
                },
                CurrentWeather = new()
                {
                    TempF = 67,
                    TempC = 20,
                    FeelsLikeF = 65,
                    FeelsLikeC = 19,
                    IsDay = true,
                    Condition = new() { Code = 1087, Text = "Thundery outbreaks possible" }
                },
                Days = new()
                {
                    Forecastdays = new()
                    {
                        new()
                        {
                            WeatherData = new()
                            {
                                DailyWillItSnow = false,
                                DailyWillItRain = true,
                                DailyChanceOfRain = 88,
                            }
                        }
                    }
                },
            };
            //CreateWallpaper();
        }

        private void CreateWallpaper()
        {
            System.Diagnostics.Debug.WriteLine("CreateWallpaper Called (calling method: " +
                new System.Diagnostics.StackFrame(1).GetMethod().Name + ")");
            Wallpaper = desktopWallpaper.CreateBitmap(Forecast);
        }

        private bool isDirty;
        internal bool IsDirty
        {
            get => isDirty;
            set
            {
                if (Equals(isDirty, value)) return;
                isDirty = value;
                NotifyPropertyChanged(nameof(Title));
            }
        }
        private bool isNewFile;
        private Illustration illustrationClean;

        private Illustration illustration;
        public Illustration Illustration
        {
            get => illustration;
            set
            {
                if (SetProperty(ref illustration, value))
                {
                    Illustration.PropertyChanged += Illustration_PropertyChanged;
                    UpdateIsDirty();
                    CreateWallpaper();
                }
            }
        }

        private string imageFilePath;
        public string ImageFilePath
        {
            get => imageFilePath;
            set
            {
                SetProperty(ref imageFilePath, value);
                FileName = Path.GetFileNameWithoutExtension(value);
                NotifyPropertyChanged(nameof(Title));
                UpdateIsDirty();
            }
        }

        private string fileName;
        public string FileName { get => fileName; set { if (SetProperty(ref fileName, value)) UpdateIsDirty(); } }

        public string Title
        {
            get
            {
                string returnValue = "Weather Frog Illustration Metadata Editor";
                if (IsDirty | !string.IsNullOrEmpty(FileName)) returnValue += " | ";
                if (IsDirty) returnValue += "*";
                if (!string.IsNullOrEmpty(FileName)) returnValue += FileName;
                return returnValue;
            }
        }

        private System.Windows.Media.ImageSource wallpaper;
        public System.Windows.Media.ImageSource Wallpaper { get => wallpaper; set => SetProperty(ref wallpaper, value); }

        private double width = 1600.0d;
        public double Width { get => width; set => SetProperty(ref width, value); }

        private double height = 900.0d;
        public double Height { get => height; set => SetProperty(ref height, value); }

        public Rect WorkArea { get; set; } = new(0, 0, 1600, 860);

        private int taskbarMaxWidthHeight = 450;
        public int TaskbarMaxWidthHeight
        {
            get => taskbarMaxWidthHeight;
            set => SetProperty(ref taskbarMaxWidthHeight, value);
        }

        private Forecast forecast;
        public Forecast Forecast
        {
            get => forecast;
            set
            {
                if (SetProperty(ref forecast, value))
                {
                    Forecast.CurrentWeather.Condition.PropertyChanged += Condition_PropertyChanged;
                    Forecast.CurrentWeather.PropertyChanged += Current_PropertyChanged;
                    CreateWallpaper();
                }
            }
        }

        private void Condition_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text") return;
            if (Forecast.CurrentWeather.Condition.Code == 1000)
                Forecast.CurrentWeather.Condition.Text = Forecast.CurrentWeather.IsDay.Value ? "Sunny" : "Clear";
            else if (WeatherApi.Configuration.Conditions.TryGetValue(Forecast.CurrentWeather.Condition.Code.Value,
                 out string conitionText))
                Forecast.CurrentWeather.Condition.Text = conitionText;
            CreateWallpaper();
        }

        private void Current_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsDay" && Forecast.CurrentWeather.Condition.Code == 1000)
            {
                Condition_PropertyChanged(sender, e);
                return;
            }
            CreateWallpaper();
        }

        private System.Windows.Controls.Dock taskbarLocation = System.Windows.Controls.Dock.Bottom;
        public System.Windows.Controls.Dock TaskbarLocation
        {
            get => taskbarLocation;
            set { if (SetProperty(ref taskbarLocation, value)) OnDesktopSettingsChanged(); }
        }

        private int taskbarWidthHeight = 40;
        public int TaskbarWidthHeight
        {
            get => taskbarWidthHeight;
            set { if (SetProperty(ref taskbarWidthHeight, value)) OnDesktopSettingsChanged(); }
        }

        private void OnDesktopSettingsChanged()
        {
            // Limit max taskbar width/height to half of desktop
            switch (taskbarLocation)
            {
                case System.Windows.Controls.Dock.Top:
                case System.Windows.Controls.Dock.Bottom:
                    TaskbarMaxWidthHeight = (int)Math.Floor(height / 2);
                    break;
                case System.Windows.Controls.Dock.Left:
                case System.Windows.Controls.Dock.Right:
                    TaskbarMaxWidthHeight = (int)Math.Floor(width / 2);
                    break;
            }
            if (taskbarWidthHeight > taskbarMaxWidthHeight)
            {
                TaskbarWidthHeight = taskbarMaxWidthHeight;
                return;
            }

            switch (taskbarLocation)
            {
                case System.Windows.Controls.Dock.Top:
                    WorkArea = new(0, taskbarWidthHeight, Width, Height - taskbarWidthHeight);
                    break;
                case System.Windows.Controls.Dock.Bottom:
                    WorkArea = new(0, 0, Width, Height - taskbarWidthHeight);
                    break;
                case System.Windows.Controls.Dock.Left:
                    WorkArea = new(taskbarWidthHeight, 0, Width - taskbarWidthHeight, Height);
                    break;
                case System.Windows.Controls.Dock.Right:
                    WorkArea = new(0, 0, Width - taskbarWidthHeight, Height);
                    break;
            }
            desktopWallpaper.UpdateDimensions(Width, Height, WorkArea);
            CreateWallpaper();
        }

        private void Illustration_PropertyChanged(object sender, PropertyChangedEventArgs e) => UpdateIsDirty();

        private RelayCommand loadImageCommand;
        public RelayCommand LoadImageCommand => loadImageCommand ??= new(async () =>
        {
            if (!await HandleIsDirty()) return;

            OpenFileDialog openDlg = new() { Title = "Open Illustration", DefaultExt = ".png", Filter = "PNG Files|*.png" };
            if (openDlg.ShowDialog() == true)
            {
                string jsonFilePath = openDlg.FileName.Replace(Path.GetExtension(openDlg.FileName), ".json");
                if (File.Exists(@jsonFilePath))
                {
                    await LoadImageAsync(@jsonFilePath);
                }
                else
                {
                    isNewFile = true;
                    Illustration = new();
                    ImageFilePath = openDlg.FileName;
                    IsDirty = true;
                }
            }
        });

        private RelayCommand buildFileNameCommand;
        public RelayCommand BuildFileNameCommand => buildFileNameCommand ??= new(() => new FilenameEditorWindow().Show());


        /// <summary>
        /// If values are dirty, show the user a messagebox asking if they should save the changes.
        /// <para>Return True if values are not dirty, or the values are dirty and the user saves or discards the changes. 
        /// False if the user cancels.</para>
        /// </summary>
        /// <returns></returns>
        public async Task<bool> HandleIsDirty()
        {
            if (IsDirty)
            {
                switch (Windows.SaveMessageBox.Show(FileName))
                {
                    case MessageBoxResult.Yes:
                        await SaveToFileAsync(Path.GetDirectoryName(ImageFilePath));
                        return true;
                    case MessageBoxResult.No:
                        return true;
                    case MessageBoxResult.None:
                    case MessageBoxResult.Cancel:
                        return false;
                }
            }
            return true;
        }

        private async Task LoadImageAsync(string path)
        {
            using FileStream openStream = File.OpenRead(path);
            Illustration = await JsonSerializer.DeserializeAsync<Illustration>(openStream);
            illustrationClean = new(Illustration);
            ImageFilePath = @path;
            IsDirty = false;
            isNewFile = false;
        }

        private RelayCommand saveFileCommand;

        public RelayCommand SaveFileCommand => saveFileCommand ??= new(async () =>
        {
            SaveFileDialog dlg = new() { DefaultExt = ".json", Filter = "JSON (.json)|*.json", FileName = FileName };
            bool? result = dlg.ShowDialog();
            if (result == true) await SaveToFileAsync(Path.GetDirectoryName(dlg.FileName));
        }, () => !string.IsNullOrEmpty(ImageFilePath) && IsDirty);

        public async Task SaveToFileAsync(string path)
        {
            using FileStream createStream = File.Create(Path.Combine(@path, FileName + ".json"));
            await JsonSerializer.SerializeAsync(createStream, Illustration, new() { WriteIndented = true });
        }

        private void UpdateIsDirty() =>
            IsDirty = isNewFile ||
            Path.GetFileNameWithoutExtension(ImageFilePath) != FileName ||
            !Illustration.Equals(illustrationClean);

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

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
