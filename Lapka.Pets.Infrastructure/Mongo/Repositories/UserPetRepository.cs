using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Infrastructure.Mongo.Documents;

namespace Lapka.Pets.Infrastructure.Mongo.Repositories
{
    public class UserPetRepository : IUserPetRepository
    {
        private readonly IMongoRepository<UserPetDocument, Guid> _repository;

        public UserPetRepository(IMongoRepository<UserPetDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<IEnumerable<UserPet>> GetUserPets(Guid userId)
        {
            IReadOnlyList<UserPetDocument> petsFromDb = await _repository.FindAsync(x => x.UserId == userId && !x.IsDeleted);

            return petsFromDb.Select(x => x.AsBusiness());
        }

        public async Task<UserPet> GetByIdAsync(Guid id)
        {
            UserPetDocument petFromDb = await _repository.GetAsync(id);
            if (petFromDb is null || petFromDb.IsDeleted)
            {
                throw new PetNotFoundException(id);
            }

            return petFromDb.AsBusiness();
        }

        public async Task AddAsync(UserPet pet)
        {
            await _repository.AddAsync(pet.AsDocument());
        }

        public async Task DeleteAsync(UserPet pet)
        {
            await _repository.DeleteAsync(pet.AsDocument().Id);
        }

        public async Task UpdateAsync(UserPet pet)
        {
            await _repository.UpdateAsync(pet.AsDocument());
        }
    }
}