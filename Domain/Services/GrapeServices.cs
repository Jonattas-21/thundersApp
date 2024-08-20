using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    public class GrapeServices : IGrapeService
    {
        private readonly IRepository<Grape> _repository;
        private readonly ILogger<GrapeServices> _logger;

        public GrapeServices(IRepository<Grape> repository, ILogger<GrapeServices> log)
        {
            _repository = repository;
            _logger = log;
        }

        public (Grape?, bool) CreateGrape(Grape grape)
        {
            try
            {
                grape.Id = Guid.NewGuid();
                grape.Ativo = true;
                Grape result = _repository.Create(grape);
                _logger.LogInformation("Added wine grape {grape}", grape);
                return (result, true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error adding wine grape {grape}", grape);
                return (null, false);
            }
        }

        public bool DeleteGrape(Guid id)
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

        public Grape GetGrapeById(Guid id)
        {
            return _repository.GetById(id);
        }

        public Grape GetGrapeByName(string name)
        {
            return _repository.GetByQuery(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public Dictionary<string, string> CheckValidGrapeFields(Grape grape)
        {
            var validations = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(grape.Name))
                {
                    validations.Add("Grape Name", "Name is required");
                }

                if (string.IsNullOrEmpty(grape.Description))
                {
                    validations.Add("Grape Description", "Description is required");
                }

                if (string.IsNullOrEmpty(grape.Origin))
                {
                    validations.Add("Grape Origin", "Origin is required");
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

