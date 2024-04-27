using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace weatherfrog.Illustrations
{
    public class Illustration : INotifyPropertyChanged
    {
        private AlignmentX alignmentX;
        private readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        };
        private bool isBelow45;
        private TimeOfDay timeOfDay;
        private WeatherCondition weatherCondition;

        public Illustration() { }
        public Illustration(Illustration illustration)
        {
            AlignmentX = illustration.AlignmentX;
            IsBelow45 = illustration.IsBelow45;
            TimeOfDay = illustration.TimeOfDay;
            WeatherCondition = illustration.WeatherCondition;
        }

        [JsonPropertyName("alignment"), JsonConverter(typeof(JsonStringEnumConverter))]
        public AlignmentX AlignmentX { get => alignmentX; set => SetProperty(ref alignmentX, value); }

        /// <summary>
        /// Boolean value indicating weather the illustration is meant to be for temperatures below 45°F. Maybe
        /// the frog is wearing a knit hat and sweater, for example.
        /// </summary>
        [JsonPropertyName("is_below_45"), DefaultValue(false)]
        public bool IsBelow45 { get => isBelow45; set => SetProperty(ref isBelow45, value); }

        [JsonPropertyName("time_of_day")]
        public TimeOfDay TimeOfDay { get => timeOfDay; set => SetProperty(ref timeOfDay, value); }

        [JsonPropertyName("weather_condition")]
        public WeatherCondition WeatherCondition { get => weatherCondition; set => SetProperty(ref weatherCondition, value); }

        public string FileName { get; set; }

        [JsonIgnore]
        public string Json => JsonSerializer.Serialize(this, jsonSerializerOptions);

        public override bool Equals(object obj) => obj is Illustration illustration && Equals(illustration);

        public bool Equals(Illustration other) =>
            AlignmentX == other?.AlignmentX &&
            isBelow45 == other?.isBelow45 &&
            TimeOfDay == other?.TimeOfDay &&
            WeatherCondition == other?.WeatherCondition;
        public override int GetHashCode() => base.GetHashCode();

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
            NotifyPropertyChanged(nameof(Json));
            return true;
        }

        #endregion 

    }
}
