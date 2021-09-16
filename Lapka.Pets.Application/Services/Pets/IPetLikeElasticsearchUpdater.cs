using System;
using System.Threading.Tasks;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Services.Pets
{
    public interface IPetLikeElasticsearchUpdater
    {
        Task InsertAndUpdateDataAsync(UserLikedPets pet);
        Task DeleteDataAsync(UserLikedPets likedPets);
    }
}