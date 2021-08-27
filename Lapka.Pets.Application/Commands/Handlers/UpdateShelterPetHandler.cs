using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdatePetHandler : ICommandHandler<UpdateShelterPet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<ShelterPet> _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;

        public UpdatePetHandler(IEventProcessor eventProcessor, IPetRepository<ShelterPet> petRepository,
            IGrpcPhotoService grpcPhotoService)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }

        public async Task HandleAsync(UpdateShelterPet command)
        {
            string mainPhotoPath = $"{Guid.NewGuid():N}.{command.Photo.GetFileExtension()}";

            ShelterPet pet = await _petRepository.GetByIdAsync(command.Id);
            if (pet is null)
            {
                throw new PetNotFoundException(command.Id);
            }

            pet.Update(command.Name, command.Race, command.Species, mainPhotoPath, command.Sex, command.DateOfBirth,
                command.Sterilization, command.Weight, command.Color, command.ShelterAddress, command.Description);

            if (command.Photo != null)
            {
                await _grpcPhotoService.DeleteAsync(pet.MainPhotoPath, BucketName.PetPhotos);
                await _grpcPhotoService.AddAsync(mainPhotoPath, command.Photo.Content, BucketName.PetPhotos);
            }
            
            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}