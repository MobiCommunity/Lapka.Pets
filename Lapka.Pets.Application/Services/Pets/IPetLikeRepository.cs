using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Services.Pets
{
    public interface IPetLikeRepository
    {
        Task<UserLikedPets> GetLikedPetsAsync(Guid userId);
        Task<IEnumerable<UserLikedPets>> GetUsersLikedPetsContainingGivenPetAsync(Guid petId);
        Task UpdateLikesAsync(UserLikedPets likedPets);
        Task AddUserPetListAsync(UserLikedPets user);
    }
}