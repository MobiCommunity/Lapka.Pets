using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.ShelterPets;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers.ShelterPets
{
    public class CreateShelterPetHandler : ICommandHandler<CreateShelterPet>
    {
        private readonly ILogger<CreateShelterPetHandler> _logger;
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;
        private readonly IGrpcIdentityService _grpcIdentityService;


        public CreateShelterPetHandler(ILogger<CreateShelterPetHandler> logger, IEventProcessor eventProcessor,
            IShelterPetRepository repository, IGrpcPhotoService photoService, IGrpcIdentityService grpcIdentityService)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
            _repository = repository;
            _photoService = photoService;
            _grpcIdentityService = grpcIdentityService;
        }

        public async Task HandleAsync(CreateShelterPet command)
        {
            await ValidIfUserOwnShelter(command);

            ShelterPet pet = ShelterPet.Create(command.Id, command.UserId, command.Name, command.Sex, command.Race, command.Species,
                command.MainPhoto.Id, command.BirthDay, command.Color, command.Weight, command.Sterilization,
                 command.ShelterId, command.ShelterAddress, command.Description,
                command.Photos == null ? new List<Guid>() : command.Photos.IdsAsGuidList());

            await AddMainPhoto(command, pet);
            await AddPhotos(command, pet);
            
            await _repository.AddAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
            
        }
        
        private async Task ValidIfUserOwnShelter(CreateShelterPet command)
        {
            bool isOwner = false;
            try
            {
                isOwner = await _grpcIdentityService.IsUserOwnerOfShelter(command.ShelterId, command.UserId);
            }
            catch (Exception ex)
            {
                throw new CannotRequestIdentityMicroserviceException(ex);
            }
            
            if (!isOwner)
            {
                throw new UserNotOwnerOfShelterException(command.UserId, command.ShelterId);
            }
        }
        
        private async Task AddPhotos(CreateShelterPet command, ShelterPet pet)
        {
            if (command.Photos != null)
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
                    _logger.LogError(ex, ex.Message);
                    foreach (PhotoFile photo in command.Photos)
                    {
                        pet.RemovePhoto(photo.Id);
                    }
                }
            }
        }

        private async Task AddMainPhoto(CreateShelterPet command, ShelterPet pet)
        {
            try
            {
                await _photoService.AddAsync(command.MainPhoto.Id, command.MainPhoto.Name, command.MainPhoto.Content,
                    BucketName.PetPhotos);
                pet.UpdateMainPhoto(command.MainPhoto.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                pet.UpdateMainPhoto(Guid.Empty);
            }
        }
    }
}