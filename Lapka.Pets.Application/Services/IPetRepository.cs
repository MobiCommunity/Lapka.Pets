using System;
using System.Threading.Tasks;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Services
{
    public interface IPetRepository
    {
        Task DeleteAsync(Pet pet);
        Task UpdateAsync(Pet pet);
    }
}