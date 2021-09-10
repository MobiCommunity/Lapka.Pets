using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class AddShelterPetPhotoHandler : ICommandHandler<AddShelterPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;
        private readonly IGrpcIdentityService _grpcIdentityService;


        public AddShelterPetPhotoHandler(IEventProcessor eventProcessor, IShelterPetRepository repository,
            IGrpcPhotoService photoService, IGrpcIdentityService grpcIdentityService)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _photoService = photoService;
            _grpcIdentityService = grpcIdentityService;
        }
        public async Task HandleAsync(AddShelterPetPhoto command)
        {
            ShelterPet pet = await _repository.GetByIdAsync(command.PetId);
            if (pet == null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            await ValidIfUserOwnShelterAsync(command, pet);
            
            await AddPhotosAsync(command);
            pet.AddPhotos(command.Photos.IdsAsGuidList());
            
            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        private async Task ValidIfUserOwnShelterAsync(AddShelterPetPhoto command, ShelterPet pet)
        {
            try
            {
                bool isOwner = await _grpcIdentityService.IsUserOwnerOfShelter(pet.ShelterId, command.UserId);
                if (!isOwner)
                {
                    throw new UserNotOwnerOfShelterException(command.UserId, pet.ShelterId);
                }
            }
            catch (Exception ex)
            {
                throw new CannotRequestIdentityMicroserviceException(ex);
            }
        }

        private async Task AddPhotosAsync(AddShelterPetPhoto command)
        {
            try
            {
                foreach (PhotoFile photo in command.Photos)
                {
                    await _photoService.AddAsync(photo.Id, photo.Name, photo.Content, BucketName.PetPhotos);
                }
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }
        }
    }
}