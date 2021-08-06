using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Services
{
    public class PetQueryService : IPetQueryService
    {
        private readonly IMongoRepository<PetDocument, Guid> _repository;

        public PetQueryService(IMongoRepository<PetDocument, Guid> mongoRepository)
        {
            _repository = mongoRepository;
        }
        
        public async Task<Pet> GetPetByIdAsync(Guid id)
        {
            PetDocument petFromDb = await _repository.GetAsync(id);
            if (petFromDb == null) return null;

            return petFromDb.AsBusiness();
        } 
        public async Task<IEnumerable<Pet>> GetAllPetsAsync()
        {
            var petsFromDb = await _repository.FindAsync(_ => true);
        
            return petsFromDb.Select(x => x.AsBusiness());
        }

        public async Task<IEnumerable<Pet>> GetAllPetsByRaceAsync(string race)
        {
            var petsFromDb = await _repository.FindAsync(x => x.Race.ToUpper() == race.ToUpper());

            return petsFromDb.Select(x => x.AsBusiness());
        }
    }
}