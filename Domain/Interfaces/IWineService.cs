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
        Wine GetWineById(Guid id);
        (Wine, Dictionary<string, string>) CreateWine(Wine wine);
        (Wine, Dictionary<string, string>) UpdateWine(Guid id, Dictionary<string, string> fields);
        bool DeleteWine(Guid id);
        bool EnableDisableWine(Guid id);
        Dictionary<string, string> CheckValidWineFields(Wine wine);
        (Dictionary<string, string>, Wine) CheckUpdateWineFields(Dictionary<string, string> fields, Wine wine);
    }
}
