using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class CreateShelterPetHandler : ICommandHandler<CreateShelterPet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<ShelterPet> _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly ILogger<CreateShelterPetHandler> _logger;

        public CreateShelterPetHandler(IEventProcessor eventProcessor, IPetRepository<ShelterPet> petRepository,
            IGrpcPhotoService grpcPhotoService, ILogger<CreateShelterPetHandler> logger)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
            _logger = logger;
        }

        public async Task HandleAsync(CreateShelterPet command)
        {
            ShelterPet pet = ShelterPet.Create(command.Id, command.Name, command.Sex, command.Race, command.Species,
                command.MainPhoto.Id, command.BirthDay, command.Color, command.Weight, command.Sterilization,
                command.ShelterAddress, command.Description,
                command.Photos == null ? new List<Guid>() : command.Photos.IdsAsGuidList());

            try
            {
                await _grpcPhotoService.AddAsync(command.MainPhoto.Id, command.MainPhoto.Name,
                    command.MainPhoto.Content, BucketName.PetPhotos);

                if (command.Photos != null)
                {
                    foreach (PhotoFile photo in command.Photos)
                    {
                        await _grpcPhotoService.AddAsync(photo.Id, photo.Name, photo.Content, BucketName.PetPhotos);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Did not upload shelter pet photo");
                pet.UpdateMainPhoto(Guid.Empty);
                if (command.Photos != null)
                {
                    foreach (PhotoFile photo in command.Photos)
                    {
                        pet.RemovePhoto(photo.Id);
                    }
                }
            }


            await _petRepository.AddAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}