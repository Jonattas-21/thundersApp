using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Wine : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public int Harvest { get; set; }
        [Required]
        [MaxLength(100)]
        public string Region { get; set; }
        [Required]
        [MaxLength(100)]
        public string Winery { get; set; }
        public virtual Grape Grape { get; set; }
        public virtual Analysis? Analysis { get; set; }
    }
}
