namespace Models.Eskom.ResponseDto
{
    public class SuburbSearchResponseDto
    {
        public int BlockId { get; set; }
        public string Name { get; set; }
        public Municipality Municipality { get; set; }
        public Province Province { get; set; }
        public bool IsEskomClient { get; set; }
        public int Total { get; set; }
    }
}
