using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    internal interface IWineService
    {
        IEnumerable<Wine> GetAllWines();
        Wine GetWineById(int id);
        IEnumerable<Wine> GetWineByAnalysis(int tannin, int aciAcidity, int body);
        (Wine, Dictionary<string, string>) AddWine(Wine wine);
        (Wine, Dictionary<string, string>) UpdateWine(Wine wine);
        bool DeleteWine(int id);
    }
}
