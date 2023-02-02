using System;
using System.Text.Json.Serialization;

namespace EskomCalendarApi.Models.Logging
{
    public class MessageDetails
    {
        public DateTime ActionDate { get; set; }
        public string UserToken { get; set; }
    }

    public class SuburbItem : MessageDetails
    {
        public string SuburbName { get; set; }
    }

    public class InstalledItem : MessageDetails
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
