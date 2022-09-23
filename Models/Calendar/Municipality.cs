using System.Text.Json.Serialization;

namespace EskomCalendarApi.Models.Calendar
{
    public class Municipality
    {
        [JsonPropertyName("Value")]
        public int MunicipalityId { get; set; }
        [JsonPropertyName("Text")]
        public string MunicipalityName { get; set; }
    }
}
