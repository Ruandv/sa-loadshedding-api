using System.Text.Json.Serialization;

namespace EskomCalendarApi.Models.Calendar
{
    public class SuburbData
    {
        [JsonPropertyName("subName")]
        public string SubName { get; set; }
        [JsonPropertyName("blockId")]
        public string BlockId { get; set; }
    }
}
