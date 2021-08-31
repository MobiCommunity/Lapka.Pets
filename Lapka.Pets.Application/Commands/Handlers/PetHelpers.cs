using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers.Helpers
{
    public static class PetHelpers
    {
        public static async Task AddPetPhotosAsync<THandler, TPet>(ILogger<THandler> logger,
            IGrpcPhotoService grpcPhotoService, IPetRepository<TPet> repository, PhotoFile mainPhoto,
            IEnumerable<PhotoFile> photos, TPet pet) where TPet : AggregatePet
        {
            await AddPetPhotoAtCreationAsync(logger, grpcPhotoService, repository, mainPhoto,
                pet);

            if (photos != null)
            {
                await AddPetPhotosAsync(logger, grpcPhotoService, repository, photos, pet);
            }
        }

        public static async Task<TPet> GetPetFromRepositoryAsync<TPet>(IPetRepository<TPet> petRepository, Guid petId)
        {
            TPet pet = await petRepository.GetByIdAsync(petId);
            if (pet == null)
            {
                throw new PetNotFoundException(petId);
            }

            return pet;
        }

        public static async Task AddPetPhotoAsync(IGrpcPhotoService grpcPhotoService, PhotoFile photo)
        {
            try
            {
                await grpcPhotoService.AddAsync(photo.Id, photo.Name, photo.Content, BucketName.PetPhotos);
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }
        }

        public static async Task AddPetPhotosAsync<THandler, TPet>(ILogger<THandler> logger,
            IGrpcPhotoService grpcPhotoService, IPetRepository<TPet> repository, IEnumerable<PhotoFile> photos,
            TPet pet)
            where TPet : AggregatePet
        {
            List<PhotoFile> photoFiles = photos.ToList();

            try
            {
                foreach (PhotoFile photo in photoFiles)
                {
                    await grpcPhotoService.AddAsync(photo.Id, photo.Name, photo.Content, BucketName.PetPhotos);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                foreach (PhotoFile photo in photoFiles)
                {
                    pet.RemovePhoto(photo.Id);
                }

                await repository.UpdateAsync(pet);
            }
        }

        public static async Task DeletePetPhotoAsync(IGrpcPhotoService grpcPhotoService, Guid photoId)
        {
            try
            {
                await grpcPhotoService.DeleteAsync(photoId, BucketName.PetPhotos);
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }
        }

        public static async Task DeletePetPhotosAsync<THandler>(ILogger<THandler> logger,
            IGrpcPhotoService grpcPhotoService,
            Guid mainPhotoId, List<Guid> photoIds)
        {
            try
            {
                await grpcPhotoService.DeleteAsync(mainPhotoId, BucketName.PetPhotos);
                foreach (Guid photoId in photoIds)
                {
                    await grpcPhotoService.DeleteAsync(photoId, BucketName.PetPhotos);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        public static async Task AddPetPhotoAtCreationAsync<THandler, TPet>(ILogger<THandler> logger,
            IGrpcPhotoService grpcPhotoService, IPetRepository<TPet> petRepository, PhotoFile photo, TPet pet)
            where TPet : AggregatePet
        {
            try
            {
                await grpcPhotoService.AddAsync(photo.Id, photo.Name, photo.Content, BucketName.PetPhotos);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                pet.UpdateMainPhoto(photo.Id);
                await petRepository.UpdateAsync(pet);
            }
        }

        public static void CheckIfPhotoExist<TPet>(Guid photoId, TPet pet) where TPet : AggregatePet
        {
            Guid photoIdFromList = pet.PhotoIds.FirstOrDefault(x => x == photoId);
            if (photoIdFromList == Guid.Empty)
            {
                throw new PhotoNotFoundException(photoId.ToString());
            }
        }
    }
}