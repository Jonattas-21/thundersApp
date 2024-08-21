using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Origin : BaseEntity
    {

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        public virtual IEnumerable<TaskForce>? Tasks { get; set; }
    }
}
