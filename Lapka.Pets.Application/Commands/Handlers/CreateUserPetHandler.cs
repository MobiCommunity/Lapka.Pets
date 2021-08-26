using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class CreateUserPetHandler : ICommandHandler<CreateUserPet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<UserPet> _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;

        public CreateUserPetHandler(IEventProcessor eventProcessor, IPetRepository<UserPet> petRepository,
            IGrpcPhotoService grpcPhotoService)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }

        public async Task HandleAsync(CreateUserPet command)
        {
            string mainPhotoPath = $"{Guid.NewGuid():N}.{command.Photo.GetFileExtension()}";

            UserPet pet = UserPet.Create(command.Id, command.Name, command.Sex, command.Race, command.Species,
                mainPhotoPath, command.BirthDay, command.Color, command.Weight, command.Sterilization,
                command.PetEvents, command.Visits);

            await _petRepository.AddAsync(pet);
            await _grpcPhotoService.AddAsync(mainPhotoPath, command.Photo.Content);

            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}