using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Services
{
    public interface IPetRepository<T>
    {
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T pet);
        Task DeleteAsync(T pet);
        Task UpdateAsync(T pet);
    }
}