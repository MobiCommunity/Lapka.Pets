using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Services
{
    public interface IPetPhotoService<TPet> where TPet : AggregatePet
    {
        Task<TPet> AddPetPhotosAsync<THandler>(ILogger<THandler> logger, PhotoFile mainPhoto, IEnumerable<PhotoFile> photos,
            TPet pet);
        Task DeletePetPhotosAsync<THandler>(ILogger<THandler> logger, Guid mainPhotoId, List<Guid> photoIds);
        Task<TPet> AddPetPhotosAsync(IEnumerable<PhotoFile> photos, TPet pet);
        Task<TPet> UpdatePetPhotosAsync(Guid oldPhotoId, PhotoFile newPhoto, TPet pet);
        Task<TPet> DeletePetPhotoAsync(Guid photoId, TPet pet);
    }
}