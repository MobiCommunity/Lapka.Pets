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
    public class CreateLostPetHandler : ICommandHandler<CreateLostPet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<LostPet> _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly ILogger<CreateLostPetHandler> _logger;

        public CreateLostPetHandler(IEventProcessor eventProcessor, IPetRepository<LostPet> petRepository,
            IGrpcPhotoService grpcPhotoService, ILogger<CreateLostPetHandler> logger)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
            _logger = logger;
        }

        public async Task HandleAsync(CreateLostPet command)
        {
            LostPet pet = LostPet.Create(command.Id, command.UserId, command.Name, command.Sex, command.Race,
                command.Species, command.MainPhoto.Id,
                command.Photos == null ? new List<Guid>() : command.Photos.IdsAsGuidList(),
                DateTime.Now.Subtract(TimeSpan.FromDays(365 * command.Age)), command.Color, command.Weight,
                command.OwnerName, command.PhoneNumber, command.LostDate, command.LostAddress, command.Description);

            await PetHelpers.AddPetPhotosAsync(_logger, _grpcPhotoService, _petRepository, command.MainPhoto, command.Photos, pet);
            
            await _petRepository.AddAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
        
    }
}