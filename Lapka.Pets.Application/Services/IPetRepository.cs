using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Services
{
    public interface IPetRepository
    {
        Task<Pet> GetByIdAsync(Guid id);
        Task<IEnumerable<Pet>> GetAllByRaceAsync(string race);
        Task AddAsync(Pet pet);
        Task DeleteAsync(Pet pet);
        Task UpdateAsync(Pet pet);
    }
}