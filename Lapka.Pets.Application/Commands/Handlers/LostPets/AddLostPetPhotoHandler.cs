using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.LostPets;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers.LostPets
{
    public class AddLostPetPhotoHandler : ICommandHandler<AddLostPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly ILostPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;

        public AddLostPetPhotoHandler(IEventProcessor eventProcessor, ILostPetRepository repository,
            IGrpcPhotoService photoService)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _photoService = photoService;
        }

        public async Task HandleAsync(AddLostPetPhoto command)
        {
            LostPet pet = await GetLostPetAsync(command);
            ValidIfUserIsOwnerOfPet(command, pet);

            ICollection<string> paths = await AddPhotosToMinioAsync(command);
            pet.AddPhotos(paths);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        private async Task<ICollection<string>> AddPhotosToMinioAsync(AddLostPetPhoto command)
        {
            Collection<string> paths = new Collection<string>();

            try
            {
                foreach (File photo in command.Photos)
                {
                    paths.Add(await _photoService.AddAsync(photo.Name, command.UserId, true, photo.Content,
                        BucketName.PetPhotos));
                }
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }

            return paths;
        }

        private static void ValidIfUserIsOwnerOfPet(AddLostPetPhoto command, LostPet pet)
        {
            if (pet.UserId != command.UserId)
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.Value.ToString());
            }
        }

        private async Task<LostPet> GetLostPetAsync(AddLostPetPhoto command)
        {
            LostPet pet = await _repository.GetByIdAsync(command.PetId);
            if (pet == null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            return pet;
        }
    }
}