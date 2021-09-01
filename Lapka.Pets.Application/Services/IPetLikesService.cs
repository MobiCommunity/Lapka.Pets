using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lapka.Pets.Application.Services
{
    public interface IPetLikesService
    {
        public Task LikePet(Guid petId, Guid userId);
        public Task DislikePet(Guid petId, Guid userId);
        public Task<IEnumerable<Guid>> GetUserLikedPetIds(Guid userId);
    }
}