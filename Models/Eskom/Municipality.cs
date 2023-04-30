using System.Text.Json.Serialization;

namespace Models.Eskom
{
    public class Municipality
    {
        [JsonPropertyName("Value")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public int MunicipalityId { get; set; }
        [JsonPropertyName("Text")]
        public string MunicipalityName { get; set; }
    }
}
