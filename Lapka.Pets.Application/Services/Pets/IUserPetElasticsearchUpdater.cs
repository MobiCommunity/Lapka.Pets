using System;
using System.Threading.Tasks;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Services.Pets
{
    public interface IUserPetElasticsearchUpdater
    {
        Task InsertAndUpdateDataAsync(UserPet pet);
        Task DeleteDataAsync(Guid petId);
    }
}