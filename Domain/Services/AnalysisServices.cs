using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    internal class AnalysisServices : IAnalysisService
    {
        private readonly IRepository<Analysis> _repository;
        private readonly ILogger<AnalysisServices> _logger;
        private readonly IGrapeService _grapeService;

        public AnalysisServices(IRepository<Analysis> repository, ILogger<AnalysisServices> log)
        {
            _repository = repository;
            _logger = log;
        }

        public Dictionary<string, string> CheckValidAnalysisFields(Analysis analysis)
        {
            var validations = new Dictionary<string, string>();

            try
            {
                if (!analysis.Sweet.IsBetweenAnalysis())
                {
                    validations.Add("Sweet", "Sweetness must be between 1 and 5");
                }

                if (!analysis.Tannin.IsBetweenAnalysis())
                {
                    validations.Add("Tannin", "Tannin must be between 1 and 5");
                }

                if (!analysis.Acidity.IsBetweenAnalysis())
                {
                    validations.Add("AciAcidity", "Acidity must be between 1 and 5");
                }

                if (!analysis.Body.IsBetweenAnalysis())
                {
                    validations.Add("Body", "Body must be between 1 and 5");
                }

                if (!analysis.Alcohol.IsBetweenAnalysis())
                {
                    validations.Add("Alcohol", "Alcohol must be between 1 and 5");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error validating wine {wine}", analysis);
                validations.Add("Error", "Internal error while validating wine");
            }

            return validations;
        }

        public (Analysis, Dictionary<string, string>) CreateAnalysis(Analysis analysis)
        {
            var validations = new Dictionary<string, string>();

            try
            {
                validations = this.CheckValidAnalysisFields(analysis);

                if (validations.Count > 0)
                {
                    return (analysis, validations);
                }

                var savedAnalysis = _repository.Create(analysis);

                return (savedAnalysis, validations);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error adding analysis {analysis}", analysis);
                validations.Add("Error", "Internal error while adding analysis");
                return (analysis, validations);
            }
        }

        public bool DeleteAnalysis(Guid id)
        {
            try
            {
                _repository.DeleteById(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting analysis with id {id}", id);
                return false;
            }

            return true;
        }

        public Analysis GetAnalysisByWine(Guid id)
        {
            return _repository.GetByQuery(x => x.Wine.Id == id).FirstOrDefault();
        }


    }
    internal static class IntExtensions
    {
        public static bool IsBetweenAnalysis(this int value)
        {
            return value >= 1 && value <= 5;
        }
    }
}
