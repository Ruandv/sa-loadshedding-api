namespace Models.Eskom.ResponseDto
{
    public class SuburbSearchResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Municipality Municipality { get; set; }
        public Province Province { get; set; }
        public int? BlockId { get; set; }
    }
}
