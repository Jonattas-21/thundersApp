using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace thundersApp.Dtos
{
    public class WineResponseDto
    {
        public required string Name { get; set; }
        public int Harvest { get; set; }

        public Guid Id { get; set; }
        public required string Region { get; set; }
        public required string Winery { get; set; }
        public GrapeResponseDto Grape { get; set; }
    }
}
