using System;
using System.Text.Json.Serialization;

namespace weatherfrog.WeatherApi.Models
{
    public class Alert : BaseModel
    {
        private string headline;
        private string messageType;
        private string severity;
        private string urgency;
        private string areas;
        private string category;
        private string certainty;
        private string @event;
        private string note;
        private DateTimeOffset? effective;
        private DateTimeOffset? expires;
        private string desc;
        private string instruction;

        /// <summary>
        /// Headline of the alert.
        /// </summary>
        [JsonPropertyName("headline")]
        public string Headline { get => headline; set => SetProperty(ref headline, value); }

        /// <summary>
        /// Notification type. May be one of Alert, Warning, Update, Cancel.
        /// </summary>
        [JsonPropertyName("msgtype")]
        public string Messagetype { get => messageType; set => SetProperty(ref messageType, value); }

        /// <summary>
        /// Severity of the alert. May be one of Extreme, Severe, Moderate, Minor, Unknown.
        /// </summary>
        [JsonPropertyName("severity")]
        public string Severity { get => severity; set => SetProperty(ref severity, value); }

        /// <summary>
        /// Urgency of the alert. May be one of Immediate, Expected, Future, Unknown.
        /// </summary>
        [JsonPropertyName("urgency")]
        public string Urgency { get => urgency; set => SetProperty(ref urgency, value); }

        /// <summary>
        /// Areas effected by the alert.
        /// </summary>
        [JsonPropertyName("areas")]
        public string Areas { get => areas; set => SetProperty(ref areas, value); }

        /// <summary>
        /// Category of the alert. May be one of Geo, Met, Safety, Security, Rescue, Fire, Health, Env, Transport, Infra, CBRNE, Other.
        /// </summary>
        [JsonPropertyName("category")]
        public string Category { get => category; set => SetProperty(ref category, value); }

        /// <summary>
        /// Certainty of the alert. May be one of Observed, Likely, Possible, Unlikely, Unknown.
        /// </summary>
        [JsonPropertyName("certainty")]
        public string Certainty { get => certainty; set => SetProperty(ref certainty, value); }

        /// <summary>
        /// Alert event name.
        /// </summary>
        [JsonPropertyName("event")]
        public string Event { get => @event; set => SetProperty(ref @event, value); }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("note")]
        public string Note { get => note; set => SetProperty(ref note, value); }

        /// <summary>
        /// Date and time the alert comes into effect.
        /// </summary>
        [JsonPropertyName("effective")]
        public DateTimeOffset? Effective { get => effective; set => SetProperty(ref effective, value); }

        /// <summary>
        /// Date and time the alert is not longer in effect.
        /// </summary>
        [JsonPropertyName("expires")]
        public DateTimeOffset? Expires { get => expires; set => SetProperty(ref expires, value); }

        /// <summary>
        /// Description of the alert.
        /// </summary>
        [JsonPropertyName("desc")]
        public string Description { get => desc; set => SetProperty(ref desc, value); }

        /// <summary>
        /// Instructions from agency about the alert.
        /// </summary>
        [JsonPropertyName("instruction")]
        public string Instruction { get => instruction; set => SetProperty(ref instruction, value); }
    }
}
