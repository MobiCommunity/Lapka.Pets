using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Application.Commands;

namespace Lapka.Pets.Application.Services
{
    public interface IPetLikeRepository
    {
        Task<IEnumerable<Guid>> GetLikedPets(Guid userId, Guid petId);
        Task UpdateLike(LikePet likedPets);
        Task AddUserPetList(Guid userId);
        Task RemoveUserPetList(Guid userId);
    }
}