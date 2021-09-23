using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.UserPets;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers.UserPets
{
    public class AddUserPetPhotoHandler : ICommandHandler<AddUserPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;

        public AddUserPetPhotoHandler(IEventProcessor eventProcessor, IUserPetRepository repository,
            IGrpcPhotoService photoService)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _photoService = photoService;
        }

        public async Task HandleAsync(AddUserPetPhoto command)
        {
            UserPet pet = await GetUserPetAsync(command);
            ValidIfUserIsOwnerOfPet(command, pet);

            ICollection<string> paths = await AddPhotosToMinioAsync(command);
            pet.AddPhotos(paths);
            
            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        private static void ValidIfUserIsOwnerOfPet(AddUserPetPhoto command, UserPet pet)
        {
            if (pet.UserId != command.UserId)
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.Value.ToString());
            }
        }

        private async Task<UserPet> GetUserPetAsync(AddUserPetPhoto command)
        {
            UserPet pet = await _repository.GetByIdAsync(command.PetId);
            if (pet == null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            return pet;
        }

        private async Task<ICollection<string>> AddPhotosToMinioAsync(AddUserPetPhoto command)
        {
            Collection<string> paths = new Collection<string>();

            try
            {
                foreach (File photo in command.Photos)
                {
                    paths.Add(await _photoService.AddAsync(photo.Name, command.UserId, false, photo.Content,
                        BucketName.PetPhotos));
                }
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }

            return paths;
        }
    }
}