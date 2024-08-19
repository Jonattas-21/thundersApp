using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    internal interface IAnalysisService
    {
        Task<Analysis> GetAnalysisByWine(int wineId);
        Task<Analysis> AddAnalysis(Analysis analysis);
        Task<Analysis> UpdateAnalysis(Analysis analysis);
        Task DeleteAnalysis(int id);
    }
}
