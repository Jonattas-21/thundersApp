using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface ITaskForceService
    {
        TaskForce GetTaskById(Guid id);
        (TaskForce?, Dictionary<string, string>) CreateTask(TaskForce taskForce, Guid originId);
        bool Delete(Guid id);
        Dictionary<string, string> CheckValidTaskForceFields(TaskForce taskForce);
        IEnumerable<TaskForce> GetByPriority(int? Priority);
        bool UpdateStatus(Guid id, StatusEnum statusEnum);
        IEnumerable<TaskForce> GetAll();
    }
}
