using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.Handlers.Helpers;
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

            await PetHelpers.AddPetPhotosAsync(_logger, _grpcPhotoService, _petRepository, command.MainPhoto,
                command.Photos, pet);

            await _petRepository.AddAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}