using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.Handlers.Helpers;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeleteShelterPetPhotoHandler : ICommandHandler<DeleteShelterPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<ShelterPet> _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;

        public DeleteShelterPetPhotoHandler(IEventProcessor eventProcessor, IPetRepository<ShelterPet> petRepository,
            IGrpcPhotoService grpcPhotoService)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }
        public async Task HandleAsync(DeleteShelterPetPhoto command)
        {
            ShelterPet pet = await PetHelpers.GetPetFromRepositoryAsync(_petRepository, command.PetId);
            PetHelpers.CheckIfPhotoExist(command.PhotoId, pet);

            await PetHelpers.DeletePetPhotoAsync(_grpcPhotoService, command.PhotoId);
            pet.RemovePhoto(command.PhotoId);
            
            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}