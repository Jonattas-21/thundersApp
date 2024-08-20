
namespace thundersApp.Dtos
{
    public class AnalysisResponseDto
    {
        public int Sweet { get; set; }
        public int Tannin { get; set; }
        public int Acidity { get; set; }
        public int Alcohol { get; set; }
        public int Body { get; set; }
        public string? AdditionNotes { get; set; }
        public WineResponseDto Wine { get; set; }
    }
}
