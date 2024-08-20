﻿using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IGrapeService
    {
        IEnumerable<Grape> GetAllGrapes();
        Grape GetGrapeById(Guid id);
        (Grape?, bool) CreateGrape(Grape grape);
        bool DeleteGrape(Guid id);
        Grape GetGrapeByName(string name);
        Dictionary<string, string> CheckValidGrapeFields(Grape grape);

    }
}
