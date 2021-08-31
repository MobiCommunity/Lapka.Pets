using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.Handlers.Helpers;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.AspNetCore.Routing.Matching;
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
            UserPet pet = await _petRepository.GetByIdAsync(command.PetId);
            UserPetHelpers.ValidateUserAndPet(command.UserId, command.PetId, pet);

            pet.Delete();
            
            try
            {
                await _grpcPetService.DeletePetAsync(command.UserId, command.PetId);
            }
            catch (Exception ex)
            {
                throw new CannotRequestPetsMicroserviceException(ex);
            }
            
            try
            {
                await _grpcPhotoService.DeleteAsync(pet.MainPhotoId, BucketName.PetPhotos);
                foreach (Guid photoId in pet.PhotoIds)
                {
                    await _grpcPhotoService.DeleteAsync(photoId, BucketName.PetPhotos);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            await _petRepository.DeleteAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}
