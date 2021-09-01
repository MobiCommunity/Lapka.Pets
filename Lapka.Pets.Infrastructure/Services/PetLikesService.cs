using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Services
{
    public class PetLikesService : IPetLikesService
    {
        private readonly IMongoRepository<LikePetDocument, Guid> _repository;

        public PetLikesService()
        {
            _repository = repository;
        }
        public Task LikePet(Guid petId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task DislikePet(Guid petId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Guid>> GetUserLikedPetIds(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}