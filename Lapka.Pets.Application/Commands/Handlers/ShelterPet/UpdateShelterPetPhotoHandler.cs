using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.Handlers.Helpers;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdateShelterPetPhotoHandler : ICommandHandler<UpdateShelterPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<ShelterPet> _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;

        public UpdateShelterPetPhotoHandler(IEventProcessor eventProcessor, IPetRepository<ShelterPet> petRepository,
            IGrpcPhotoService grpcPhotoService)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }

        public async Task HandleAsync(UpdateShelterPetPhoto command)
        {
            ShelterPet pet = await PetHelpers.GetPetFromRepositoryAsync(_petRepository, command.PetId);
            
            await PetHelpers.DeletePetPhotoAsync(_grpcPhotoService, pet.MainPhotoId);
            await PetHelpers.AddPetPhotoAsync(_grpcPhotoService, command.Photo);
            pet.UpdateMainPhoto(command.Photo.Id);

            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}