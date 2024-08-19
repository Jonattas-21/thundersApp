using Domain.Entities;

namespace Domain.Interfaces
{
    internal interface IGrapeService
    {
        IEnumerable<Grape> GetAllGrapes();
        Grape GetGrapeById(int id);
        (Grape?, bool) AddGrape(Grape grape);
        bool DeleteGrape(int id);

        Dictionary<string, string> GrapeValidate(Grape grape);
    }
}
