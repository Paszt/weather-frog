using System.Text.Json.Serialization;

namespace weatherfrog.WeatherApi.Models
{
    public class IpLookup : BaseModel
    {
        private string ip;
        private IPType type;
        private string continentCode;
        private string continentName;
        private string countryCode;
        private string countryName;
        private bool? isEu;
        private int? geonameId;
        private string city;
        private string region;
        private double? lat;
        private double? lon;
        private string tzId;
        private int? localtimeEpoch;
        private string localtime;

        /// <summary>
        /// IP address.
        /// </summary>
        [JsonPropertyName("ip")]
        public string Ip { get => ip; set => SetProperty(ref ip, value); }

        /// <summary>
        /// ipv4 or ipv6.
        /// </summary>
        [JsonPropertyName("type"), JsonConverter(typeof(JsonStringEnumConverter))]
        public IPType Type { get => type; set => SetProperty(ref type, value); }

        /// <summary>
        /// Continent code.
        /// </summary>
        [JsonPropertyName("continent_code")]
        public string ContinentCode { get => continentCode; set => SetProperty(ref continentCode, value); }

        /// <summary>
        /// Continent name.
        /// </summary>
        [JsonPropertyName("continent_name")]
        public string ContinentName { get => continentName; set => SetProperty(ref continentName, value); }

        /// <summary>
        /// Country code.
        /// </summary>
        [JsonPropertyName("country_code")]
        public string CountryCode { get => countryCode; set => SetProperty(ref countryCode, value); }

        /// <summary>
        /// Name of country.
        /// </summary>
        [JsonPropertyName("country_name")]
        public string CountryName { get => countryName; set => SetProperty(ref countryName, value); }

        /// <summary>
        /// true or false.
        /// </summary>
        [JsonPropertyName("is_eu"), JsonConverter(typeof(Utilities.ToBooleanConverter))]
        public bool? IsEu { get => isEu; set => SetProperty(ref isEu, value); }

        /// <summary>
        /// Geoname ID.
        /// </summary>
        [JsonPropertyName("geoname_id")]
        public int? GeonameId { get => geonameId; set => SetProperty(ref geonameId, value); }

        /// <summary>
        /// City name.
        /// </summary>
        [JsonPropertyName("city")]
        public string City { get => city; set => SetProperty(ref city, value); }

        /// <summary>
        /// Region name.
        /// </summary>
        [JsonPropertyName("region")]
        public string Region { get => region; set => SetProperty(ref region, value); }

        /// <summary>
        /// Latitude in decimal degree.
        /// </summary>
        [JsonPropertyName("lat")]
        public double? Lat { get => lat; set => SetProperty(ref lat, value); }

        /// <summary>
        /// Longitude in decimal degree.
        /// </summary>
        [JsonPropertyName("lon")]
        public double? Lon { get => lon; set => SetProperty(ref lon, value); }

        /// <summary>
        /// Time zone.
        /// </summary>
        [JsonPropertyName("tz_id")]
        public string TzId { get => tzId; set => SetProperty(ref tzId, value); }

        /// <summary>
        /// Local time as epoch.
        /// </summary>
        [JsonPropertyName("localtime_epoch")]
        public int? LocaltimeEpoch { get => localtimeEpoch; set => SetProperty(ref localtimeEpoch, value); }

        /// <summary>
        /// Date and time.
        /// </summary>
        [JsonPropertyName("localtime")]
        public string Localtime { get => localtime; set => SetProperty(ref localtime, value); }
    }
}
