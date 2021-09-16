using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.LostPets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers.LostPets
{
    public class DeleteLostPetPhotoHandler : ICommandHandler<DeleteLostPetPhoto>
    {
        private readonly ILostPetRepository _repository;
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _photoService;

        public DeleteLostPetPhotoHandler(ILostPetRepository repository, IEventProcessor eventProcessor, IGrpcPhotoService photoService)
        {
            _repository = repository;
            _eventProcessor = eventProcessor;
            _photoService = photoService;
        }
        public async Task HandleAsync(DeleteLostPetPhoto command)
        {
            LostPet pet = await GetLostPetAsync(command);
            ValidIfUserIsOwnerOfPet(command, pet);

            await DeletePhoto(command, pet);
            pet.RemovePhoto(command.PhotoId);
            
            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        private static void ValidIfUserIsOwnerOfPet(DeleteLostPetPhoto command, LostPet pet)
        {
            if (pet.UserId != command.UserId)
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.Value.ToString());
            }
        }

        private async Task<LostPet> GetLostPetAsync(DeleteLostPetPhoto command)
        {
            LostPet pet = await _repository.GetByIdAsync(command.PetId);
            if (pet == null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            return pet;
        }

        private async Task DeletePhoto(DeleteLostPetPhoto command, LostPet pet)
        {
            try
            {
                Guid photoIdFromList = pet.PhotoIds.FirstOrDefault(x => x == command.PhotoId);
                if (photoIdFromList == Guid.Empty)
                {
                    throw new PhotoNotFoundException(command.PhotoId.ToString());
                }

                await _photoService.DeleteAsync(command.PhotoId, BucketName.PetPhotos);
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }
        }
    }
}