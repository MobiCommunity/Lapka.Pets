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
    public class LostPetRepository : PetRepository<LostPet, LostPetDocument>
    {
        private readonly IMongoRepository<LostPetDocument, Guid> _repository;

        public LostPetRepository(IMongoRepository<LostPetDocument, Guid> repository) : base(repository)
        {
            _repository = repository;
        }

        public override async Task<LostPet> GetByIdAsync(Guid id)
        {
            LostPetDocument petFromDb = await _repository.GetAsync(id);
            if (petFromDb is null)
            {
                throw new PetNotFoundException(id);
            }

            return petFromDb.AsBusiness();
        }

        public override async Task AddAsync(LostPet pet)
        {
            await _repository.AddAsync(pet.AsDocument());
        }

        public override async Task DeleteAsync(LostPet pet)
        {
            await _repository.DeleteAsync(pet.AsDocument().Id);
        }

        public override async Task UpdateAsync(LostPet pet)
        {
            await _repository.UpdateAsync(pet.AsDocument());
        }

        public override async Task<IEnumerable<LostPet>> GetAllAsync()
        {
            IReadOnlyList<LostPetDocument> petsFromDb = await _repository.FindAsync(_ => true);

            return petsFromDb.Select(x => x.AsBusiness());
        }
    }
}