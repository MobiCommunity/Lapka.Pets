using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Services
{
    public class ShelterPetRepository : PetRepository<ShelterPet, PetShelterDocument>
    {
        private readonly IMongoRepository<PetShelterDocument, Guid> _repository;
        public ShelterPetRepository(IMongoRepository<PetShelterDocument, Guid> repository) : base(repository)
        {
            _repository = repository;
        }
        public override async Task<ShelterPet> GetByIdAsync(Guid id)
        {
            PetShelterDocument petFromDb = await _repository.GetAsync(id);
            if (petFromDb is null)
            {
                throw new PetNotFoundException(id);
            }

            return petFromDb.AsBusiness();
        }

        public override async Task AddAsync(ShelterPet pet)
        {
            await _repository.AddAsync(pet.AsDocument());
        }

        public override async Task DeleteAsync(ShelterPet pet)
        {
            await _repository.DeleteAsync(pet.AsDocument().Id);
        }

        public override async Task UpdateAsync(ShelterPet pet)
        {
            await _repository.UpdateAsync(pet.AsDocument());
        }

        public override async Task<IEnumerable<ShelterPet>> GetAllAsync()
        {
            IReadOnlyList<PetShelterDocument> petsFromDb = await _repository.FindAsync(_ => true);

            return petsFromDb.Select(x => x.AsBusiness());
        }
    }
}