using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Infrastructure.PetServices.User
{
    public class UserPetPhotoService : IUserPetPhotoService
    {
        private readonly IGrpcPhotoService _grpcPhotoService;

        public UserPetPhotoService(IGrpcPhotoService grpcPhotoService)
        {
            _grpcPhotoService = grpcPhotoService;
        }

        public async Task<UserPet> AddPetPhotosAsync<THandler>(ILogger<THandler> logger, PhotoFile mainPhoto,
            IEnumerable<PhotoFile> photos, UserPet pet)
        {
            await AddPetPhotoAsync(logger, mainPhoto, pet);

            if (photos != null)
            {
                await AddPetPhotosAsync(logger, photos, pet);
            }

            return pet;
        }
        
        public async Task DeletePetPhotosAsync<THandler>(ILogger<THandler> logger, Guid mainPhotoId,
            List<Guid> photoIds)
        {
            try
            {
                await _grpcPhotoService.DeleteAsync(mainPhotoId, BucketName.PetPhotos);
                foreach (Guid photoId in photoIds)
                {
                    await _grpcPhotoService.DeleteAsync(photoId, BucketName.PetPhotos);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        public async Task<UserPet> AddPetPhotosAsync(IEnumerable<PhotoFile> photos, UserPet pet)
        {
            IEnumerable<PhotoFile> photoFiles = photos.ToList();

            try
            {
                foreach (PhotoFile photo in photoFiles)
                {
                    await _grpcPhotoService.AddAsync(photo.Id, photo.Name, photo.Content, BucketName.PetPhotos);
                }
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }

            pet.AddPhotos(photoFiles.IdsAsGuidList());
            return pet;
        }

        public async Task<UserPet> UpdatePetPhotosAsync(Guid oldPhotoId, PhotoFile newPhoto, UserPet pet)
        {
            if (oldPhotoId != Guid.Empty)
            {
                await DeletePetPhotoAsync(oldPhotoId);
            }

            await AddPetPhotoAsync(newPhoto);

            pet.UpdateMainPhoto(newPhoto.Id);

            return pet;
        }
        
        public async Task<UserPet> DeletePetPhotoAsync(Guid photoId, UserPet pet)
        {
            try
            {
                await _grpcPhotoService.DeleteAsync(photoId, BucketName.PetPhotos);
                CheckIfPhotoExist(photoId, pet);
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }

            pet.RemovePhoto(photoId);
            return pet;
        }

        private void CheckIfPhotoExist(Guid photoId, UserPet pet)
        {
            Guid photoIdFromList = pet.PhotoIds.FirstOrDefault(x => x == photoId);
            if (photoIdFromList == Guid.Empty)
            {
                throw new PhotoNotFoundException(photoId.ToString());
            }
        }
        
        private async Task AddPetPhotoAsync<THandler>(ILogger<THandler> logger, PhotoFile photo, UserPet pet)
        {
            try
            {
                await _grpcPhotoService.AddAsync(photo.Id, photo.Name, photo.Content, BucketName.PetPhotos);
                pet.UpdateMainPhoto(photo.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                pet.UpdateMainPhoto(Guid.Empty);
            }

        }

        private async Task AddPetPhotosAsync<THandler>(ILogger<THandler> logger, IEnumerable<PhotoFile> photos,
            UserPet pet)
        {
            List<PhotoFile> photoFiles = photos.ToList();

            try
            {
                foreach (PhotoFile photo in photoFiles)
                {
                    await _grpcPhotoService.AddAsync(photo.Id, photo.Name, photo.Content, BucketName.PetPhotos);
                }
                
                pet.AddPhotos(photoFiles.IdsAsGuidList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                foreach (PhotoFile photo in photoFiles)
                {
                    pet.RemovePhoto(photo.Id);
                }
            }
        }
        
        private async Task AddPetPhotoAsync(PhotoFile photo)
        {
            try
            {
                await _grpcPhotoService.AddAsync(photo.Id, photo.Name, photo.Content, BucketName.PetPhotos);
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }
        }
        
        private async Task DeletePetPhotoAsync(Guid photoId)
        {
            try
            {
                await _grpcPhotoService.DeleteAsync(photoId, BucketName.PetPhotos);
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }
        }
    }
}