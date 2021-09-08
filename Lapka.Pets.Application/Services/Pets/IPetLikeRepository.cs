using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Services
{
    public interface IPetLikeRepository
    {
        Task<UserLikedPets> GetLikedPetsAsync(Guid userId);
        Task UpdateLikesAsync(UserLikedPets likedPets);
        Task AddUserPetListAsync(UserLikedPets user);
    }
}