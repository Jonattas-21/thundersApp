using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace thundersApp.Dtos
{
    public class TaskForceResponseDto
    {
        public string? Name { get; set; }
        public int Status { get; set; }
        public int Priority { get; set; }
        public string? Description { get; set; }
        public string? Assignee { get; set; }
        public string? Origin { get; set; }
        public Guid Id { get; set; }
    }
}
