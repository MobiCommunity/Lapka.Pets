using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Services
{
    public class PetRepository : IPetRepository
    {
        private readonly IMongoRepository<PetDocument, Guid> _repository;

        public PetRepository(IMongoRepository<PetDocument, Guid> mongoRepository)
        {
            _repository = mongoRepository;
        }
        
        public async Task<Pet> GetByIdAsync(Guid id)
        {
            PetDocument petFromDb = await _repository.GetAsync(id);
            if (petFromDb is null)
            {
                throw new PetNotFoundException(id);
            }
            
            return petFromDb.AsBusiness();
        } 
        public async Task<IEnumerable<Pet>> GetAllAsync()
        {
            IReadOnlyList<PetDocument> petsFromDb = await _repository.FindAsync(_ => true);
        
            return petsFromDb.Select(x => x.AsBusiness());
        }

        public async Task<IEnumerable<Pet>> GetAllByRaceAsync(string race)
        {
            IReadOnlyList<PetDocument> petsFromDb = await _repository.FindAsync(x => x.Race.ToUpper() == race.ToUpper());

            return petsFromDb.Select(x => x.AsBusiness());
        }

        public async Task AddAsync(Pet pet)
        {
            await _repository.AddAsync(pet.AsDocument());
        }
        
        public async Task DeleteAsync(Pet pet)
        {
            await _repository.DeleteAsync(pet.AsDocument().Id);
        }

        public async Task UpdateAsync(Pet pet)
        {
            await _repository.UpdateAsync(pet.AsDocument());
        }
        
    }
}