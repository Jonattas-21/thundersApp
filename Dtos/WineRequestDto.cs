namespace thundersApp.Dtos
{
    public class WineRequestDto
    {
        public required string WineName { get; set; }
        public int Harvest { get; set; }
        public required string Region { get; set; }
        public required string Winery { get; set; }
        public required string GrapeName { get; set; }
        public required string GrapeOrigin { get; set; }
        public required string GrapeDescription { get; set; }
    }
}
