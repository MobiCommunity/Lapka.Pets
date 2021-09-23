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
    public class LostPetRepository : ILostPetRepository
    {
        private readonly IMongoRepository<LostPetDocument, Guid> _repository;

        public LostPetRepository(IMongoRepository<LostPetDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<LostPet> GetByIdAsync(Guid id)
        {
            LostPetDocument petFromDb = await _repository.GetAsync(id);
            if (petFromDb is null || petFromDb.IsDeleted)
            {
                throw new PetNotFoundException(id);
            }

            return petFromDb.AsBusiness();
        }

        public async Task AddAsync(LostPet pet)
        {
            await _repository.AddAsync(pet.AsDocument());
        }

        public async Task DeleteAsync(LostPet pet)
        {
            await _repository.DeleteAsync(pet.AsDocument().Id);
        }

        public async Task UpdateAsync(LostPet pet)
        {
            await _repository.UpdateAsync(pet.AsDocument());
        }

        public async Task<IEnumerable<LostPet>> GetAllAsync()
        {
            IReadOnlyList<LostPetDocument> petsFromDb = await _repository.FindAsync(p => !p.IsDeleted);

            return petsFromDb.Select(x => x.AsBusiness());
        }
    }
}