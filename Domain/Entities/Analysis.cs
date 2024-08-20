using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Analysis
    {

        [Key, ForeignKey("Wine")]
        public Guid Id { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Field must be between 1 and 5.")]
        public int Sweet { get; set; }
        [Range(1, 5, ErrorMessage = "Field must be between 1 and 5.")]
        public int Tannin { get; set; }
        [Range(1, 5, ErrorMessage = "Field must be between 1 and 5.")]
        public int Acidity { get; set; }
        [Range(1, 5, ErrorMessage = "Field must be between 1 and 5.")]
        public int Alcohol { get; set; }
        [Range(1, 5, ErrorMessage = "Field must be between 1 and 5.")]
        public int Body { get; set; }
        [Required]
        [MaxLength(500)]
        public string? AdditionNotes { get; set; }

        public virtual Wine Wine { get; set; }
    }
}
