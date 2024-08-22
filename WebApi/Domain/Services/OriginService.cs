using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Domain.Services
{
    public class OriginService : IOriginService
    {
        private readonly IRepository<Origin> _repository;
        private readonly ILogger<OriginService> _logger;

        public OriginService(IRepository<Origin> repository, ILogger<OriginService> log)
        {
            _repository = repository;
            _logger = log;
        }

        public Dictionary<string, string> CheckValidOriginFields(Origin origin)
        {
            var validations = new Dictionary<string, string>();

            try
            {
                if (!string.IsNullOrEmpty(origin.Name))
                {
                    if (origin.Name.Length > 100)
                    {
                        validations.Add("origin Name", "Name must have a maximum of 100 characters");
                    }
                }
                else
                {
                    validations.Add("origin Name", "Name is required");
                }
            }

            catch (Exception e)
            {
                _logger.LogError(e, "Error validating origin {origin}", origin);
                validations.Add("Error", "Internal error while validating origin");
            }

            return validations;
        }
    
        public (Origin?, Dictionary<string, string>) CreateOrigin(Origin origin)
        {
            var validations = new Dictionary<string, string>();

            try
            {
                validations = this.CheckValidOriginFields(origin);

                if (validations.Count > 0)
                {
                    return (null, validations);
                }

                origin.Id = Guid.NewGuid();
                origin.Ativo = true;
                Origin result = _repository.Create(origin);
                _logger.LogInformation("Added origin {origin}", origin);
                return (result, validations);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error adding origin {origin}", origin);
                validations.Add("Error", "Internal error while adding origin");
                return (null, validations);
            }
        }

        public bool Delete(Guid id)
        {
            try
            {
                var origin = _repository.GetById(id);

                if (origin == null)
                {
                    return false;
                }

                _repository.DeleteById(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting wine origin id {id}", id);
                throw;
            }

            return true;
        }

        public IEnumerable<Origin> GetAll()
        {
            return _repository.GetAll();
        }

        public Origin GetOriginById(Guid id)
        {
            return _repository.GetById(id);
        }
    }
}
