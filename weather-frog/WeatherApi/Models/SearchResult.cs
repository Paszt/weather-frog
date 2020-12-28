using System.Text.Json.Serialization;

namespace weatherfrog.WeatherApi.Models
{
    public class SearchResult : BaseModel
    {
        private long? id;
        private string name;
        private string region;
        private string country;
        private double? latitude;
        private double? longitude;
        private string url;

        [JsonPropertyName("id")]
        public long? Id { get => id; set => SetProperty(ref id, value); }

        [JsonPropertyName("name")]
        public string Name { get => name; set => SetProperty(ref name, value); }

        [JsonPropertyName("region")]
        public string Region { get => region; set => SetProperty(ref region, value); }

        [JsonPropertyName("country")]
        public string Country { get => country; set => SetProperty(ref country, value); }

        [JsonPropertyName("lat")]
        public double? Latitude { get => latitude; set => SetProperty(ref latitude, value); }

        [JsonPropertyName("lon")]
        public double? Longitude { get => longitude; set => SetProperty(ref longitude, value); }

        [JsonPropertyName("url")]
        public string Url { get => url; set => SetProperty(ref url, value); }

        //[JsonIgnore]
        //public string DisplayName => name + ", " + (string.IsNullOrEmpty(region) ? country : region);
    }
}
