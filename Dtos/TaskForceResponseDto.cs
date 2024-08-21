using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace thundersApp.Dtos
{
    public class TaskForceResponseDto
    {
        public required string Name { get; set; }
        public int Status { get; set; }
        public int Priority { get; set; }
        public required string Description { get; set; }
        public required string Assignee { get; set; }
        public required string Origin { get; set; }
    }
}
