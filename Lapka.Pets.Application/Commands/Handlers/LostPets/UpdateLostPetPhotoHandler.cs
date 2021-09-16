using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.LostPets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers.LostPets
{
    public class UpdateLostPetPhotoHandler : ICommandHandler<UpdateLostPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly ILostPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;

        public UpdateLostPetPhotoHandler(IEventProcessor eventProcessor, ILostPetRepository repository,
            IGrpcPhotoService photoService)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _photoService = photoService;
        }

        public async Task HandleAsync(UpdateLostPetPhoto command)
        {
            LostPet pet = await GetLostPetAsync(command);

            ValidIfUserIsOwnerOfPet(command, pet);

            await DeleteCurrentPhoto(pet);
            await AddPhoto(command);

            pet.UpdateMainPhoto(command.Photo.Id);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        private static void ValidIfUserIsOwnerOfPet(UpdateLostPetPhoto command, LostPet pet)
        {
            if (pet.UserId != command.UserId)
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.Value.ToString());
            }
        }

        private async Task<LostPet> GetLostPetAsync(UpdateLostPetPhoto command)
        {
            LostPet pet = await _repository.GetByIdAsync(command.Id);
            if (pet == null)
            {
                throw new PetNotFoundException(command.Id);
            }

            return pet;
        }

        private async Task AddPhoto(UpdateLostPetPhoto command)
        {
            try
            {
                await _photoService.AddAsync(command.Photo.Id, command.Photo.Name, command.Photo.Content,
                    BucketName.PetPhotos);
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }
        }

        private async Task DeleteCurrentPhoto(LostPet pet)
        {
            if (pet.MainPhotoId != Guid.Empty)
            {
                try
                {
                    await _photoService.DeleteAsync(pet.MainPhotoId, BucketName.PetPhotos);
                }
                catch (Exception ex)
                {
                    throw new CannotRequestFilesMicroserviceException(ex);
                }
            }
        }
    }
}