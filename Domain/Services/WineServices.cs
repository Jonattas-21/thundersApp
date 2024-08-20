using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Reflection;


namespace Domain.Services
{
    public class WineServices : IWineService
    {
        private readonly IRepository<Wine> _repository;
        private readonly ILogger<WineServices> _logger;
        private readonly IGrapeService _grapeService;

        public WineServices(IRepository<Wine> repository, IGrapeService grapeService, ILogger<WineServices> log)
        {
            _repository = repository;
            _logger = log;
            _grapeService = grapeService;
        }

        #region public methods

        public (Wine, Dictionary<string, string>) CreateWine(Wine wine)
        {
            var validations = new Dictionary<string, string>();

            try
            {
                validations = this.CheckValidWineFields(wine);

                if (validations.Count > 0)
                {
                    return (wine, validations);
                }

                wine.Id = Guid.NewGuid();
                wine.Ativo = true;

                var grapeData = _grapeService.GetGrapeByName(wine.Grape.Name);
                if (grapeData != null)
                {
                    wine.Grape = grapeData;
                }
                else
                {
                    wine.Grape.Id = Guid.NewGuid();
                    wine.Grape.Ativo = true;
                }

                var savedWine = _repository.Create(wine);

                return (savedWine, validations);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error adding wine {wine}", wine);
                validations.Add("Error", "Internal error while adding wine");
                return (wine, validations);
            }
        }

        public bool DeleteWine(Guid id)
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

        public Wine GetWineById(Guid id)
        {
            return _repository.GetById(id);
        }

        public (Wine, Dictionary<string, string>) UpdateWine(Guid id, Dictionary<string, string> fields)
        {
            var erros = new Dictionary<string, string>();

            try
            {
                var wine = _repository.GetById(id);
                if (wine != null)
                {
                    (erros, wine) = CheckUpdateWineFields(fields, wine);

                    if (erros.Count == 0)
                    {
                        _repository.Update(wine);
                    }
                }
                else
                {
                    erros.Add("Wine", "Wine not found");
                }
                return (wine, erros);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating wine fields {fields}", fields);
                erros.Add("Error", "Internal error while updating wine");
                return (null, erros);
            }


        }

        #endregion

        public Dictionary<string, string> CheckValidWineFields(Wine wine)
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

                if (wine.Harvest <= 1000 || wine.Harvest > DateTime.Now.Year)
                {
                    validations.Add("Harvest", "Harvest must be greater than 0 and minor then curent year");
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
                    var grapeValidations = _grapeService.CheckValidGrapeFields(wine.Grape);

                    foreach (var item in grapeValidations)
                    {
                        validations.Add(item.Key, item.Value);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error validating wine {wine}", wine);
                validations.Add("Error", "Internal error while validating wine");
            }

            return validations;
        }

        public (Dictionary<string, string>, Wine) CheckUpdateWineFields(Dictionary<string, string> fields, Wine wine)
        {
            var validations = new Dictionary<string, string>();

            try
            {
                Type type = wine.GetType();
                PropertyInfo[] wineFields = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (var item in fields)
                {
                    try
                    {
                        var fieldExist = wineFields.ToList().Exists(f => f.Name.ToLower() == item.Key.ToLower());

                        if (fieldExist)
                        {
                            PropertyInfo propertyInfo = type.GetProperty(item.Key);
                            if (propertyInfo != null && propertyInfo.CanWrite)
                            {
                                switch (Type.GetTypeCode(propertyInfo.PropertyType))
                                {
                                    case TypeCode.Int32:
                                        propertyInfo.SetValue(wine, Convert.ToInt32(item.Value));
                                        break;

                                    case TypeCode.Boolean:
                                        propertyInfo.SetValue(wine, Convert.ToBoolean(item.Value));
                                        break;

                                    case TypeCode.DateTime:
                                        propertyInfo.SetValue(wine, Convert.ToDateTime(item.Value));
                                        break;

                                    default:
                                        propertyInfo.SetValue(wine,item.Value);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            validations.Add(item.Key, "Campo não existe");
                        }
                    }
                    catch (Exception)
                    {
                        validations.Add(item.Key, "Erro ao atribuir o valor " + item.Value);
                    }
                }
            }

            catch (Exception e)
            {
                _logger.LogError(e, "Error validating wine update {fields}", fields);
                validations.Add("Error", "Internal error while validating wine");
            }

            return (validations, wine);
        }

        public bool EnableDisableWine(Guid id)
        {
            try
            {
                var wine =_repository.GetById(id);

                if (wine != null)
                {
                    wine.Ativo = !wine.Ativo;
                    _repository.Update(wine);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error disabling wine with id {id}", id);
                throw e;
            }

            return true;
        }
    }
}
