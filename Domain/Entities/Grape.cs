using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Grape : BaseEntity
    {

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Origin { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
    }
}
