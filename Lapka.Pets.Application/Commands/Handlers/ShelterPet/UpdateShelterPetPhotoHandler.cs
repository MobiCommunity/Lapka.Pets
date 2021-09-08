using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdateShelterPetPhotoHandler : ICommandHandler<UpdateShelterPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;
        private readonly IGrpcIdentityService _grpcIdentityService;


        public UpdateShelterPetPhotoHandler(IEventProcessor eventProcessor, IShelterPetRepository repository,
            IGrpcPhotoService photoService, IGrpcIdentityService grpcIdentityService)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _photoService = photoService;
            _grpcIdentityService = grpcIdentityService;
        }

        public async Task HandleAsync(UpdateShelterPetPhoto command)
        {
            ShelterPet pet = await _repository.GetByIdAsync(command.PetId);
            if (pet == null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            await ValidIfUserOwnShelter(command, pet);

            await DeleteCurrentPhoto(pet);
            await AddPhoto(command);

            pet.UpdateMainPhoto(command.Photo.Id);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
        
        private async Task ValidIfUserOwnShelter(UpdateShelterPetPhoto command, ShelterPet pet)
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
        
        private async Task AddPhoto(UpdateShelterPetPhoto command)
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

        private async Task DeleteCurrentPhoto(ShelterPet pet)
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