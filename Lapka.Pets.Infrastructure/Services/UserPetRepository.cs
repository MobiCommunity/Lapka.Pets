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
    public class UserPetRepository : PetRepository<UserPet, PetUserDocument>
    {
        private readonly IMongoRepository<PetUserDocument, Guid> _repository;

        public UserPetRepository(IMongoRepository<PetUserDocument, Guid> repository) : base(repository)
        {
            _repository = repository;
        }

        public override async Task<UserPet> GetByIdAsync(Guid id)
        {
            PetUserDocument petFromDb = await _repository.GetAsync(id);
            if (petFromDb is null)
            {
                throw new PetNotFoundException(id);
            }

            return petFromDb.AsBusiness();
        }

        public override async Task AddAsync(UserPet pet)
        {
            await _repository.AddAsync(pet.AsDocument());
        }

        public override async Task DeleteAsync(UserPet pet)
        {
            await _repository.DeleteAsync(pet.AsDocument().Id);
        }

        public override async Task UpdateAsync(UserPet pet)
        {
            await _repository.UpdateAsync(pet.AsDocument());
        }

        public override async Task<IEnumerable<UserPet>> GetAllAsync()
        {
            IReadOnlyList<PetUserDocument> petsFromDb = await _repository.FindAsync(_ => true);

            return petsFromDb.Select(x => x.AsBusiness());
        }
    }
}