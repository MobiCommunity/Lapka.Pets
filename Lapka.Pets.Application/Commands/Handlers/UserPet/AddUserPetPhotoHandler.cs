using System;
using System.Collections.Generic;
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
    public class AddUserPetPhotoHandler : ICommandHandler<AddUserPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<UserPet> _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;

        public AddUserPetPhotoHandler(IEventProcessor eventProcessor, IPetRepository<UserPet> petRepository,
            IGrpcPhotoService grpcPhotoService)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
        }
        public async Task HandleAsync(AddUserPetPhoto command)
        {
            UserPet pet = await UserPetHelpers.GetUserPetWithValidation(_petRepository, command.PetId, command.UserId);

            foreach (PhotoFile photo in command.Photos)
            {
                await PetHelpers.AddPetPhotoAsync(_grpcPhotoService, photo);
            }
            pet.AddPhotos(command.Photos.IdsAsGuidList());
            
            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        
    }
}