using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdatePetHandler : ICommandHandler<UpdatePet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;

        public UpdatePetHandler(IEventProcessor eventProcessor, IPetRepository petRepository, IGrpcPhotoService grpcPhotoService)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }

        public async Task HandleAsync(UpdatePet command)
        {
            string mainPhotoPath = $"{Guid.NewGuid():N}.{command.Photo.GetFileExtension()}"; 

            Pet pet = await _petRepository.GetByIdAsync(command.Id);
            if (pet is null)
            {
                throw new PetNotFoundException(command.Id);
            }
            
            pet.Update(command.Name, command.Race, command.Species, mainPhotoPath, command.Sex, command.DateOfBirth, command.Description,
                command.ShelterAddress, command.Sterilization, command.Weight, command.Color);

            await _grpcPhotoService.DeleteAsync(pet.MainPhotoPath);
            await _grpcPhotoService.AddAsync(mainPhotoPath, command.Photo.Content);

            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}