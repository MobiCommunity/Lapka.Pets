using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Services
{
    public interface IShelterRepository
    {
        public Task<Shelter> GetAsync(Guid shelterId);
        public Task<IEnumerable<Shelter>> GetAllAsync();
        public Task CreateAsync(Shelter shelter);
        public Task UpdateAsync(Shelter shelter);
        public Task DeleteAsync(Shelter shelter);
    }
}