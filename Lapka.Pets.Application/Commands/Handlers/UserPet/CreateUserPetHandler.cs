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
    public class CreateUserPetHandler : ICommandHandler<CreateUserPet>
    {
        private readonly ILogger<CreateUserPetHandler> _logger;
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<UserPet> _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly IGrpcPetService _grpcPetService;

        public CreateUserPetHandler(ILogger<CreateUserPetHandler> logger, IEventProcessor eventProcessor,
            IPetRepository<UserPet> petRepository, IGrpcPhotoService grpcPhotoService, IGrpcPetService grpcPetService)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
            _grpcPetService = grpcPetService;
        }

        public async Task HandleAsync(CreateUserPet command)
        {
            UserPet pet = UserPet.Create(command.Id, command.UserId, command.Name, command.Sex, command.Race,
                command.Species, command.MainPhoto.Id, command.BirthDay, command.Color, command.Weight,
                command.Sterilization, command.Photos.IdsAsGuidList());

            await AddPetToUserInIdentityAsync(command.UserId, command.Id);
            await PetHelpers.AddPetPhotosAsync(_logger, _grpcPhotoService, _petRepository, command.MainPhoto,
                command.Photos, pet);


            await _petRepository.AddAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        private async Task AddPetToUserInIdentityAsync(Guid userId, Guid petId)
        {
            try
            {
                await _grpcPetService.AddPetAsync(userId, petId);
            }
            catch (Exception e)
            {
                throw new CannotRequestPetsMicroserviceException(e);
            }
        }
    }
}