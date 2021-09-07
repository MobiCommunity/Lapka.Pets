using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeleteShelterPetPhotoHandler : ICommandHandler<DeleteShelterPetPhoto>
    {
        private readonly IShelterPetRepository _repository;
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _photoService;


        public DeleteShelterPetPhotoHandler(IShelterPetRepository repository, IEventProcessor eventProcessor,
            IGrpcPhotoService photoService)
        {
            _repository = repository;
            _eventProcessor = eventProcessor;
            _photoService = photoService;
        }

        public async Task HandleAsync(DeleteShelterPetPhoto command)
        {
            ShelterPet pet = await _repository.GetByIdAsync(command.PetId);
            if (pet == null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            if (pet.UserId != command.UserId)
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.Value.ToString());
            }

            await DeletePhoto(command, pet);
            pet.RemovePhoto(command.PhotoId);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        private async Task DeletePhoto(DeleteShelterPetPhoto command, ShelterPet pet)
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