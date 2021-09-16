using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.ShelterPets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers.ShelterPets
{
    public class DeleteShelterPetPhotoHandler : ICommandHandler<DeleteShelterPetPhoto>
    {
        private readonly IShelterPetRepository _repository;
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _photoService;
        private readonly IGrpcIdentityService _grpcIdentityService;


        public DeleteShelterPetPhotoHandler(IShelterPetRepository repository, IEventProcessor eventProcessor,
            IGrpcPhotoService photoService, IGrpcIdentityService grpcIdentityService)
        {
            _repository = repository;
            _eventProcessor = eventProcessor;
            _photoService = photoService;
            _grpcIdentityService = grpcIdentityService;
        }

        public async Task HandleAsync(DeleteShelterPetPhoto command)
        {
            ShelterPet pet = await GetShelterPetAsync(command);
            await ValidIfUserOwnShelter(command, pet);

            await DeletePhoto(command, pet);
            pet.RemovePhoto(command.PhotoId);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        private async Task<ShelterPet> GetShelterPetAsync(DeleteShelterPetPhoto command)
        {
            ShelterPet pet = await _repository.GetByIdAsync(command.PetId);
            if (pet == null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            return pet;
        }

        private async Task ValidIfUserOwnShelter(DeleteShelterPetPhoto command, ShelterPet pet)
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