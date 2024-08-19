using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Analysis
    {
        public int Sweet { get; set; }
        public int Tannin { get; set; }
        public int Acidity { get; set; }
        public int Alcohol { get; set; }
        public int Body { get; set; }

        public string? AdditionNotes { get; set; }
    }
}
