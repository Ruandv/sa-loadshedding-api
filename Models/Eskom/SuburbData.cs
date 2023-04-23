using System.Text.Json.Serialization;

namespace Models.Eskom
{
    public class SuburbData
    {
        [JsonPropertyName("subName")]
        public string SubName { get; set; }
        [JsonPropertyName("blockId")]
        public string BlockId { get; set; }
    }

    public class SuburbSearch
    {
        public int Id { get; set; }
        public string MunicipalityName { get; set; }
        public string Name { get; set; }
        public string ProvinceName { get; set; }
        public int Total { get; set; }
    }
}
