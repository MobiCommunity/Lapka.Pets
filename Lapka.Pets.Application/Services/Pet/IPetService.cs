using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Services
{
    public interface IPetService<TPet> where TPet : AggregatePet
    {
        Task<TPet> GetAsync(Guid petId);
        Task AddAsync<THandler>(ILogger<THandler> logger, PhotoFile mainPhoto, List<PhotoFile> photoFiles, TPet pet);
        Task DeleteAsync<THandler>(ILogger<THandler> logger, TPet pet);
        Task UpdateAsync(TPet pet);
        void ValidIfUserIsOwnerOfPet(TPet pet, Guid userId);

    }
}