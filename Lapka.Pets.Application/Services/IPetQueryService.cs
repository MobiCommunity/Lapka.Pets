using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Services
{
    public interface IPetQueryService
    {
        Task<Pet> GetPetByIdAsync(Guid id);
        Task<IEnumerable<Pet>> GetAllPetsAsync();
        Task<IEnumerable<Pet>> GetAllPetsByRaceAsync(string race);
    }
}