using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Wine : BaseEntity
    {
        public required string Name { get; set; }
        public int Harvest { get; set; }
        public required string Region { get; set; }
        public required string Winery { get; set; }

        public required Grape Grape { get; set; }
    }
}
