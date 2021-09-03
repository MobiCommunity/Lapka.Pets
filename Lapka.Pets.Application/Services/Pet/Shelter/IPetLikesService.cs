using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Services
{
    public interface IPetLikesService
    {
        public Task LikePet(Guid petId, Guid userId);
        public Task DislikePet(Guid petId, Guid userId);
        public Task<IEnumerable<Guid>> GetUserLikedPetIdsAsync(Guid userId);
        public Task AddUserPetList(Guid userId);
        public Task DeleteUserPetList(Guid userId);
    }
}