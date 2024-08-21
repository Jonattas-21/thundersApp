using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Utils;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    public class TaskForceService : ITaskForceService
    {
        private readonly IRepository<TaskForce> _repository;
        private readonly ILogger<TaskForceService> _logger;
        private readonly IOriginService _originService;

        public TaskForceService(IRepository<TaskForce> repository, IOriginService originService, ILogger<TaskForceService> log)
        {
            _repository = repository;
            _logger = log;
            _originService = originService;
        }

        public Dictionary<string, string> CheckValidTaskForceFields(TaskForce taskForce)
        {
            var validations = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(taskForce.Name))
                {
                    validations.Add("Name", "Name is required");
                }

                if (string.IsNullOrEmpty(taskForce.Description))
                {
                    validations.Add("Description", "Description is required");
                }

                if (!taskForce.Priority.IsBetweenAnalysis())
                {
                    validations.Add("Priority", "HarvPriorityest must be greater than 0 and minor then 5");
                }

                if (string.IsNullOrEmpty(taskForce.Assignee))
                {
                    validations.Add("Assignee", "Assignee is required");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error validating taskForce {taskForce}", taskForce);
                validations.Add("Error", "Internal error while validating taskForce");
            }

            return validations;
        }

        public (TaskForce?, Dictionary<string, string>) CreateTask(TaskForce taskForce, Guid originId)
        {
            var validations = new Dictionary<string, string>();

            try
            {
                var originData = _originService.GetOriginById(originId);
                if (originData != null)
                {
                    validations = this.CheckValidTaskForceFields(taskForce);

                    if (validations.Count > 0)
                    {
                        return (null, validations);
                    }

                    taskForce.Id = Guid.NewGuid();
                    taskForce.Ativo = true;
                    taskForce.Status = (int)StatusEnum.Todo;

                    var savedTask = _repository.Create(taskForce);
                    taskForce.Origin = originData;

                    return (savedTask, validations);
                }
                else
                {
                    validations.Add("Origin", "Origin not found");
                    return (null, validations);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error adding taskForce {taskForce}", taskForce);
                validations.Add("Error", "Internal error while adding taskForce");
                return (null, validations);
            }
        }

        public bool Delete(Guid id)
        {
            try
            {
                _repository.DeleteById(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting taskforce taskForce id {id}", id);
                return false;
            }

            return true;
        }

        public IEnumerable<TaskForce> GetByPriority(int? Priority)
        {
            if (Priority == null)
            {
                return _repository.GetAll();
            }
            else
            {
                return _repository.GetByQuery(x => x.Priority == Priority);
            }
        }

        public TaskForce GetTaskById(Guid id)
        {
            return _repository.GetById(id);
        }

        public bool UpdateStatus(Guid id, StatusEnum statusEnum)
        {
            try
            {
                var task = _repository.GetById(id);

                if (task == null)
                {
                    return false;
                }

                //todo

                task.Status = (int)statusEnum;

                _repository.Update(task);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating taskforce taskForce id {id}", id);
                throw;
            }
        }
    }
}
