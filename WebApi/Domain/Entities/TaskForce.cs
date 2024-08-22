using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class TaskForce : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        [Required]
        public int Priority { get; set; }
        [Required]
        public int Status { get; set; }

        [Required]
        [MaxLength(500)]
        public string? Description { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Assignee { get; set; }

        public virtual Origin? Origin { get; set; }
    }
}
