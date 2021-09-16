using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.LostPets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers.LostPets
{
    public class DeleteLostPetHandler : ICommandHandler<DeleteLostPet>
    {
        private readonly ILogger<DeleteLostPetHandler> _logger;
        private readonly IEventProcessor _eventProcessor;
        private readonly ILostPetRepository _repository;
        private readonly IGrpcPhotoService _grpcPhotoService;

        public DeleteLostPetHandler(ILogger<DeleteLostPetHandler> logger, IEventProcessor eventProcessor, ILostPetRepository repository, IGrpcPhotoService grpcPhotoService)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
            _repository = repository;
            _grpcPhotoService = grpcPhotoService;
        }

        public async Task HandleAsync(DeleteLostPet command)
        {
            LostPet pet = await GetLostPetAsync(command);
            ValidIfUserIsOwnerOfPet(command, pet);
            
            await DeletePhotos(pet);
            pet.Delete();
            
            await _repository.DeleteAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        private static void ValidIfUserIsOwnerOfPet(DeleteLostPet command, LostPet pet)
        {
            if (pet.UserId != command.UserId)
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.Value.ToString());
            }
        }

        private async Task<LostPet> GetLostPetAsync(DeleteLostPet command)
        {
            LostPet pet = await _repository.GetByIdAsync(command.PetId);
            if (pet == null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            return pet;
        }

        private async Task DeletePhotos(LostPet pet)
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