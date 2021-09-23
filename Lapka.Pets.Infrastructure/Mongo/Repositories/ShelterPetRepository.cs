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
    public class ShelterPetRepository : IShelterPetRepository
    {
        private readonly IMongoRepository<ShelterPetDocument, Guid> _repository;
        public ShelterPetRepository(IMongoRepository<ShelterPetDocument, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<ShelterPet> GetByIdAsync(Guid id)
        {
            ShelterPetDocument shelterPetFromDb = await _repository.GetAsync(id);
            if (shelterPetFromDb is null || shelterPetFromDb.IsDeleted)
            {
                throw new PetNotFoundException(id);
            }

            return shelterPetFromDb.AsBusiness();
        }

        public async Task AddAsync(ShelterPet pet)
        {
            await _repository.AddAsync(pet.AsDocument());
        }

        public async Task DeleteAsync(ShelterPet pet)
        {
            await _repository.DeleteAsync(pet.AsDocument().Id);
        }

        public async Task UpdateAsync(ShelterPet pet)
        {
            await _repository.UpdateAsync(pet.AsDocument());
        }

        public async Task<IEnumerable<ShelterPet>> GetAllAsync()
        {
            IReadOnlyList<ShelterPetDocument> petsFromDb = await _repository.FindAsync(p => !p.IsDeleted);

            return petsFromDb.Select(x => x.AsBusiness());
        }
    }
}