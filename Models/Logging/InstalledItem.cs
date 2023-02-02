using System;
using System.Text.Json.Serialization;

namespace EskomCalendarApi.Models.Logging
{
    public class MessageDetails
    {

        [JsonPropertyName("actionDate")]
        public DateTime ActionDate { get; set; }

        [JsonPropertyName("userToken")]
        public string UserToken { get; set; }
        [JsonPropertyName("appVersion")]
        public string AppVersion { get; set; }
    }

    public class SuburbItem : MessageDetails
    {
        public int Viewed { get; set; }
        [JsonPropertyName("suburbName")]
        public string SuburbName { get; set; }
    }

    public class InstalledItem : MessageDetails
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
