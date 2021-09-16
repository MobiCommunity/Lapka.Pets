using System;
using System.Threading.Tasks;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Services.Pets
{
    public interface IShelterPetElasticsearchUpdater
    {
        Task InsertAndUpdateDataAsync(ShelterPet pet);
        Task DeleteDataAsync(Guid petId);
    }
}