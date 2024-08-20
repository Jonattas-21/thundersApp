using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IWineService
    {
        IEnumerable<Wine> GetAllWines();
        Wine GetWineById(Guid id);
        IEnumerable<Wine> GetWineByAnalysis(int tannin, int aciAcidity, int body);
        (Wine, Dictionary<string, string>) CreateWine(Wine wine);
        (Wine, Dictionary<string, string>) UpdateWine(Guid id, Dictionary<string, string> fields);
        bool DeleteWine(Guid id);
        bool EnableDisableWine(Guid id);
        Dictionary<string, string> CheckValidWineFields(Wine wine);
        (Dictionary<string, string>, Wine) CheckUpdateWineFields(Dictionary<string, string> fields, Wine wine);
    }
}
