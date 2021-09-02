using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Services
{
    public interface IPetRepository<TPet>  where TPet : AggregatePet
    {
        Task<TPet> GetByIdAsync(Guid id);
        Task AddAsync(TPet pet);
        Task DeleteAsync(TPet pet);
        Task UpdateAsync(TPet pet);
    }
}