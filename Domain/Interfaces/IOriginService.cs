using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOriginService
    {
        Origin GetOriginById(Guid id);
        (Origin?, Dictionary<string, string>) CreateOrigin(Origin origin);
        bool Delete(Guid id);
        IEnumerable<Origin> GetAll();
        Dictionary<string, string> CheckValidOriginFields(Origin origin);
    }
}
