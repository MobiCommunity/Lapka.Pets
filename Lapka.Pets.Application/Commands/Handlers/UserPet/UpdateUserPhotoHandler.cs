﻿using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdateUserPhotoHandler : ICommandHandler<UpdateUserPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;

        public UpdateUserPhotoHandler(IEventProcessor eventProcessor, IUserPetRepository repository,
            IGrpcPhotoService photoService)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _photoService = photoService;
        }

        public async Task HandleAsync(UpdateUserPetPhoto command)
        {
            UserPet pet = await _repository.GetByIdAsync(command.Id);
            if (pet == null)
            {
                throw new PetNotFoundException(command.Id);
            }

            if (pet.UserId != command.UserId)
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.Value.ToString());
            }

            await DeleteCurrentPhoto(pet);
            await AddPhoto(command);

            pet.UpdateMainPhoto(command.Photo.Id);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
        
        private async Task AddPhoto(UpdateUserPetPhoto command)
        {
            try
            {
                await _photoService.AddAsync(command.Photo.Id, command.Photo.Name, command.Photo.Content,
                    BucketName.PetPhotos);
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }
        }

        private async Task DeleteCurrentPhoto(UserPet pet)
        {
            if (pet.MainPhotoId != Guid.Empty)
            {
                try
                {
                    await _photoService.DeleteAsync(pet.MainPhotoId, BucketName.PetPhotos);
                }
                catch (Exception ex)
                {
                    throw new CannotRequestFilesMicroserviceException(ex);
                }
            }
        }
    }
}