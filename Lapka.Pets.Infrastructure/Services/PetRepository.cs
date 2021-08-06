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
    public class PetRepository : IPetRepository
    {
        private readonly IMongoRepository<PetDocument, Guid> _repository;

        public PetRepository(IMongoRepository<PetDocument, Guid> mongoRepository)
        {
            _repository = mongoRepository;
        }
        
        public async Task AddAsync(Pet shelter)
        {
            await _repository.AddAsync(shelter.AsDocument());
        }
        

    }
}