using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAnalysisService
    {
        Analysis GetAnalysisByWine(Guid id);
        (Analysis, Dictionary<string, string>) CreateAnalysis(Analysis analysis);
        bool DeleteAnalysis(Guid id);
        Dictionary<string, string> CheckValidAnalysisFields(Analysis analysis);
    }
}
