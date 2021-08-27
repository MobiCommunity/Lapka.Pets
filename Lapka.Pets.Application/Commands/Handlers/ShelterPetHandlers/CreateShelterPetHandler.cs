using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.AspNetCore.Http;
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
            string mainPhotoPath = $"{Guid.NewGuid():N}.{command.Photo.GetFileExtension()}";

            ShelterPet pet = ShelterPet.Create(command.Id, command.Name, command.Sex, command.Race, command.Species,
                mainPhotoPath, command.BirthDay, command.Color, command.Weight, command.Sterilization,
                command.ShelterAddress, command.Description);

            try
            {
                await _grpcPhotoService.AddAsync(mainPhotoPath, command.Photo.Content, BucketName.PetPhotos);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Did not upload shelter pet photo");
            }

            await _petRepository.AddAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}