using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    internal class AlcoholEnum
    {
        public enum Alcohol
        {
            Less10_Percent = 1,
            Between10To11Half_Percent = 2,
            Between11HalfTo13Half_Percent = 3,
            Between13HalfTo15_Percent = 4,
            More15_Percent = 5
        }
    }
}
