using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Services
{
    public class PetLikeRepository : IPetLikeRepository
    {
        private readonly IMongoRepository<LikePetDocument, Guid> _repository;

        public PetLikeRepository(IMongoRepository<LikePetDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<IEnumerable<Guid>> GetLikedPets(Guid userId, Guid petId)
        {
            IReadOnlyList<LikePetDocument> likedPets = await _repository.FindAsync(x => x.Id == userId);

            return likedPets.Select(x => x.Id);
        }

        public async Task UpdateLike(LikePetDocument likedPets)
        {
            await _repository.AddAsync(likedPets);
        }

        public async Task AddUserPetList(Guid userId)
        {
            await _repository.AddAsync(new LikePetDocument(userId, new List<Guid>()));
        }

        public async Task RemoveUserPetList(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}