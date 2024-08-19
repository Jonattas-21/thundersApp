using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Domain.Services
{
    internal class WineServices : IWineService
    {
        private readonly IWineRepository _repository;
        private readonly ILogger _logger;
        private readonly IGrapeService _grapeService;

        public WineServices(IWineRepository repository, IGrapeService grapeService, ILogger log)
        {
            _repository = repository;
            _logger = log;
            _grapeService = grapeService;
        }

        #region public methods

        public (Wine, Dictionary<string, string>) AddWine(Wine wine)
        {
            var validations = new Dictionary<string, string>();

            try
            {
                validations = this.checkValidWine(wine);

                if (validations.Count > 0)
                {
                    return (wine, validations);
                }

                var savedWine = _repository.Save(wine);

                return (savedWine, validations);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error adding wine {wine}", wine);
                validations.Add("Error", "Internal error while adding wine");
                return (wine, validations);
            }
        }

        public bool DeleteWine(int id)
        {
            try
            {
                _repository.DeleteById(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting wine with id {id}", id);
                return false;
            }

            return true;
        }

        public IEnumerable<Wine> GetAllWines()
        {
            return _repository.GetAll();
        }

        public IEnumerable<Wine> GetWineByAnalysis(int tannin, int aciAcidity, int body)
        {
            throw new NotImplementedException();
        }

        public Wine GetWineById(int id)
        {
            return _repository.GetById(id);
        }

        public (Wine, Dictionary<string, string>) UpdateWine(Wine wine)
        {
            var erros = new Dictionary<string, string>();

            try
            {
                _repository.Save(wine);
                return (wine, erros);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating wine {wine}", wine);
                erros.Add("Error", "Internal error while updating wine");
                return (wine, erros);
            }


        }

        #endregion

        #region private methods

        private Dictionary<string, string> checkValidWine(Wine wine)
        {
            var validations = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(wine.Name))
                {
                    validations.Add("Name", "Name is required");
                }

                if (string.IsNullOrEmpty(wine.Winery))
                {
                    validations.Add("Winery", "Winery is required");
                }

                if (wine.Harvest >= 1800 || wine.Harvest <= DateTime.Now.Year)
                {
                    validations.Add("Harvest", "Harvest must be greater than 0");
                }

                if (string.IsNullOrEmpty(wine.Region))
                {
                    validations.Add("Region", "Region is required");
                }

                if (wine.Grape == null)
                {
                    validations.Add("Grape", "Grape is required");
                }
                else
                {
                    validations.Concat(_grapeService.GrapeValidate(wine.Grape));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error validating wine {wine}", wine);
                validations.Add("Error", "Internal error while validating wine");
            }

            return validations;
        }

        #endregion
    }
}
