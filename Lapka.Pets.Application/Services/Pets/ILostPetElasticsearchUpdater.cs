using System;
using System.Threading.Tasks;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Services.Pets
{
    public interface ILostPetElasticsearchUpdater
    {
        Task InsertAndUpdateDataAsync(LostPet pet);
        Task DeleteDataAsync(Guid petId);
    }
}