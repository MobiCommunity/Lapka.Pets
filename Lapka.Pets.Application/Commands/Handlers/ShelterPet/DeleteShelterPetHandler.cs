using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
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
        private readonly IShelterPetRepository _repository;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly IGrpcIdentityService _grpcIdentityService;

        public DeleteShelterPetHandler(ILogger<DeleteShelterPetHandler> logger, IEventProcessor eventProcessor,
            IShelterPetRepository repository, IGrpcPhotoService grpcPhotoService, IGrpcIdentityService grpcIdentityService)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
            _repository = repository;
            _grpcPhotoService = grpcPhotoService;
            _grpcIdentityService = grpcIdentityService;
        }

        public async Task HandleAsync(DeleteShelterPet command)
        {
            ShelterPet pet = await _repository.GetByIdAsync(command.Id);
            if (pet == null)
            {
                throw new PetNotFoundException(command.Id);
            }

            await ValidIfUserOwnShelter(command, pet);
            
            await DeletePhotos(pet);
            pet.Delete();

            await _repository.DeleteAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
        
        private async Task ValidIfUserOwnShelter(DeleteShelterPet command, ShelterPet pet)
        {
            try
            {
                bool isOwner = await _grpcIdentityService.IsUserOwnerOfShelter(pet.ShelterId, command.UserId);
                if (!isOwner)
                {
                    throw new UserNotOwnerOfShelterException(command.UserId, pet.ShelterId);
                }
            }
            catch (Exception ex)
            {
                throw new CannotRequestIdentityMicroserviceException(ex);
            }
        }

        private async Task DeletePhotos(ShelterPet pet)
        {
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
        }
    }
}