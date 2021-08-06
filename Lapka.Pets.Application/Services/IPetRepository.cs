using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Services
{
    public interface IPetRepository
    {
        public Task AddAsync(Pet shelter);
    }
}