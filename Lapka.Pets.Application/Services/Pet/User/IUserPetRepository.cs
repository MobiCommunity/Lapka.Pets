using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Services
{
    public interface IUserPetRepository : IPetRepository<UserPet>
    {
        Task<IEnumerable<UserPet>> GetUserPets(Guid userId);
    }
}