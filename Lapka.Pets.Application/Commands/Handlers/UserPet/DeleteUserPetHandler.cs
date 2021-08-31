using System;
using System.Collections.Generic;
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
    public class DeleteUserPetHandler : ICommandHandler<DeleteUserPet>
    {
        private readonly ILogger<DeleteUserPetHandler> _logger;
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<UserPet> _petRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly IGrpcPetService _grpcPetService;

        public DeleteUserPetHandler(ILogger<DeleteUserPetHandler> logger, IEventProcessor eventProcessor,
            IPetRepository<UserPet> petRepository, IGrpcPhotoService grpcPhotoService, IGrpcPetService grpcPetService)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _grpcPhotoService = grpcPhotoService;
            _grpcPetService = grpcPetService;
        }

        public async Task HandleAsync(DeleteUserPet command)
        {
            UserPet pet = await UserPetHelpers.GetUserPetWithValidation(_petRepository, command.PetId, command.UserId);
            
            await DeletePetFromIdentityServiceAsync(command.UserId, command.PetId);
            await PetHelpers.DeletePetPhotosAsync(_logger, _grpcPhotoService, pet.MainPhotoId, pet.PhotoIds);
            pet.Delete();

            await _petRepository.DeleteAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        private async Task DeletePetFromIdentityServiceAsync(Guid userId, Guid petId)
        {
            try
            {
                await _grpcPetService.DeletePetAsync(userId, petId);
            }
            catch (Exception ex)
            {
                throw new CannotRequestPetsMicroserviceException(ex);
            }
        }
    }
}
