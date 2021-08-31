using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.Handlers.Helpers;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeleteShelterPetHandler : ICommandHandler<DeleteShelterPet>
    {
        private readonly ILogger<DeleteShelterPetHandler> _logger;
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly IPetRepository<ShelterPet> _petRepository;


        public DeleteShelterPetHandler(ILogger<DeleteShelterPetHandler> logger, IEventProcessor eventProcessor,
            IPetRepository<ShelterPet> petRepository, IGrpcPhotoService grpcPhotoService)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }

        public async Task HandleAsync(DeleteShelterPet command)
        {
            ShelterPet pet = await PetHelpers.GetPetFromRepositoryAsync(_petRepository, command.Id);
            
            pet.Delete();
            await PetHelpers.DeletePetPhotosAsync(_logger, _grpcPhotoService, pet.MainPhotoId, pet.PhotoIds);
            
            await _petRepository.DeleteAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}