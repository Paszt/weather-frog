using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Media;
using weatherfrog.Infrastructure;

namespace weatherfrog.Illustrations
{
    public class Illustration : INotifyPropertyChanged
    {
        private string fileName;
        private AlignmentX alignmentX;
        private TimeOfDay timeOfDay;
        private WeatherCondition weatherCondition;

        [JsonPropertyName("file_name")]
        public string FileName { get => fileName; set => SetProperty(ref fileName, value); }

        [JsonPropertyName("alignment"), JsonConverter(typeof(JsonStringEnumConverter))]
        public AlignmentX AlignmentX { get => alignmentX; set => SetProperty(ref alignmentX, value); }

        [JsonPropertyName("time_of_day"), JsonConverter(typeof(JsonStringEnumConverter))]
        public TimeOfDay TimeOfDay { get => timeOfDay; set => SetProperty(ref timeOfDay, value); }

        [JsonPropertyName("weather_condition"), JsonConverter(typeof(JsonStringEnumConverter))]
        public WeatherCondition WeatherCondition { get => weatherCondition; set => SetProperty(ref weatherCondition, value); }

        [JsonIgnore]
        public string Json => JsonSerializer.Serialize(this, new() { WriteIndented = true });

        private RelayCommand saveToFileCommand;
        [JsonIgnore]
        public RelayCommand SavetoFileCommand => saveToFileCommand ??= new RelayCommand(async () =>
        {
            SaveFileDialog dlg = new() { DefaultExt = ".json", Filter = "JSON (.json)|*.json", FileName = FileName };
            bool? result = dlg.ShowDialog();
            if (result == true) await SaveToFile(Path.GetDirectoryName(dlg.FileName));
        }, () => !string.IsNullOrEmpty(FileName));

        public async System.Threading.Tasks.Task SaveToFile(string path)
        {
            using FileStream createStream = File.Create(Path.Combine(path, fileName + ".json"));
            await JsonSerializer.SerializeAsync(createStream, this, new() { WriteIndented = true });
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

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
            NotifyPropertyChanged(nameof(Json));
            return true;
        }

        #endregion 

    }
}
