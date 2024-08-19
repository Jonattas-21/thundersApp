using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    internal class GrapeServices : IGrapeService
    {
        private readonly IGrapeRepository _repository;
        private readonly ILogger _logger;

        public GrapeServices(IGrapeRepository repository, ILogger log)
        {
            _repository = repository;
            _logger = log;
        }

        public (Grape?, bool) AddGrape(Grape grape)
        {
            try
            {
                Grape? result = _repository.Add(grape);
                _logger.LogInformation("Added wine grape {grape}", grape);
                return (result, true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error adding wine grape {grape}", grape);
                return (null, false);
            }
        }

        public bool DeleteGrape(int id)
        {
            try
            {
                _repository.DeleteById(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting wine grape id {id}", id);
                return false;
            }

            return true;
        }

        public IEnumerable<Grape> GetAllGrapes()
        {
            return _repository.GetAll();
        }

        public Grape GetGrapeById(int id)
        {
            return _repository.GetById(id);
        }

        public Dictionary<string, string> GrapeValidate(Grape grape)
        {
            var validations = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(grape.Name))
                {
                    validations.Add("Name", "Name is required");
                }

                if (string.IsNullOrEmpty(grape.Description))
                {
                    validations.Add("Color", "Description is required");
                }

                if (string.IsNullOrEmpty(grape.Origin))
                {
                    validations.Add("Color", "Origin is required");
                }

            }

            catch (Exception e)
            {
                _logger.LogError(e, "Error validating wine {grape}", grape);
                validations.Add("Error", "Internal error while validating wine");
            }

            return validations;
        }
    }
}
}
