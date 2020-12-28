using System.Text.Json.Serialization;

namespace weatherfrog.WeatherApi.Models
{
    public class Location : BaseModel
    {
        private string name;
        private string region;
        private string country;
        private double? latitude;
        private double? longitude;
        private string timeZoneName;
        private int? localtimeEpoch;
        private string localtime;

        /// <summary>
        /// Location name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get => name; set => SetProperty(ref name, value); }

        /// <summary>
        /// Region or state of the location, if available.
        /// </summary>
        [JsonPropertyName("region")]
        public string Region { get => region; set => SetProperty(ref region, value); }

        /// <summary>
        /// Location country.
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get => country; set => SetProperty(ref country, value); }

        /// <summary>
        /// Latitude in decimal degree.
        /// </summary>
        [JsonPropertyName("lat")]
        public double? Latitude { get => latitude; set => SetProperty(ref latitude, value); }

        /// <summary>
        /// Longitude in decimal degree.
        /// </summary>
        [JsonPropertyName("lon")]
        public double? Longitude { get => longitude; set => SetProperty(ref longitude, value); }

        /// <summary>
        /// Time zone name.
        /// </summary>
        [JsonPropertyName("tz_id")]
        public string TimeZoneName { get => timeZoneName; set => SetProperty(ref timeZoneName, value); }

        /// <summary>
        /// Local date and time in unix time.
        /// </summary>
        [JsonPropertyName("localtime_epoch")]
        public int? LocaltimeEpoch { get => localtimeEpoch; set => SetProperty(ref localtimeEpoch, value); }

        /// <summary>
        /// Local date and time.
        /// </summary>
        [JsonPropertyName("localtime")]
        public string Localtime { get => localtime; set => SetProperty(ref localtime, value); }

        [JsonIgnore]
        public string DisplayName => name + ", " + (string.IsNullOrEmpty(region) ? country : region);
    }
}
