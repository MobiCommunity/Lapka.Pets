using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Application.Services.Pets;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Lapka.Pets.Infrastructure.Mongo.Repositories
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

        public async Task<IEnumerable<UserLikedPets>> GetUsersLikedPetsContainingGivenPetAsync(Guid petId)
        {
            IMongoQueryable<LikePetDocument> query = _repository.Collection.AsQueryable();
            query = query.Where(x => x.LikedPets.Contains(petId));
            ICollection<LikePetDocument> lists = await query.ToListAsync();
            
            return lists.Select(x => x.AsBusiness());
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