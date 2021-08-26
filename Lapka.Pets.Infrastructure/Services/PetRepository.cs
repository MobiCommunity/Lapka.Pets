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
    public abstract class PetRepository<TBusinessPet, TPetDocument> : IPetRepository<TBusinessPet>
        where TBusinessPet : Pet where TPetDocument : PetDocument
    {

        protected PetRepository(IMongoRepository<TPetDocument, Guid> repository)
        {
            
        }
        
        public abstract Task<TBusinessPet> GetByIdAsync(Guid id);
        public abstract Task<IEnumerable<TBusinessPet>> GetAllAsync();
        public abstract Task AddAsync(TBusinessPet pet);
        public abstract Task DeleteAsync(TBusinessPet pet);
        public abstract Task UpdateAsync(TBusinessPet pet);
    }
}