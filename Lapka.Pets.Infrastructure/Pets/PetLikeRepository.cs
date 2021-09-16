using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Pets
{
    public class PetLikeRepository : IPetLikeRepository
    {
        private readonly IMongoRepository<LikePetDocument, Guid> _repository;

        public PetLikeRepository(IMongoRepository<LikePetDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<UserLikedPets> GetLikedPetsAsync(Guid userId)
        {
            LikePetDocument likedPets = await _repository.GetAsync(x => x.Id == userId);

            return likedPets?.AsBusiness();
        }

        public async Task UpdateLikesAsync(UserLikedPets likedPets)
        {
            await _repository.UpdateAsync(likedPets.AsDocument());
        }

        public async Task AddUserPetListAsync(UserLikedPets petList)
        {
            await _repository.AddAsync(petList.AsDocument());
        }
    }
}