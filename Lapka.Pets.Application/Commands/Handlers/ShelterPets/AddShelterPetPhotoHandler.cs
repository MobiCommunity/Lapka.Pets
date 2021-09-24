using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.ShelterPets;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers.ShelterPets
{
    public class AddShelterPetPhotoHandler : ICommandHandler<AddShelterPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;
        private readonly IShelterRepository _shelterRepository;


        public AddShelterPetPhotoHandler(IEventProcessor eventProcessor, IShelterPetRepository repository,
            IGrpcPhotoService photoService, IShelterRepository shelterRepository)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _photoService = photoService;
            _shelterRepository = shelterRepository;
        }
        public async Task HandleAsync(AddShelterPetPhoto command)
        {
            ShelterPet pet = await GetShelterPetAsync(command);
            await ValidIfUserOwnShelterAsync(command, pet);
            
            ICollection<string> paths = await AddPhotosToMinioAsync(command);
            pet.AddPhotos(paths);
            
            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        private async Task<ShelterPet> GetShelterPetAsync(AddShelterPetPhoto command)
        {
            ShelterPet pet = await _repository.GetByIdAsync(command.PetId);
            if (pet == null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            return pet;
        }

        private async Task ValidIfUserOwnShelterAsync(AddShelterPetPhoto command, ShelterPet pet)
        {
            Shelter shelter = await _shelterRepository.GetAsync(pet.ShelterId);
            if (!shelter.Owners.Contains(command.UserId))
            {
                throw new UserNotOwnerOfShelterException(command.UserId, pet.ShelterId);
            }
        }

        private async Task<ICollection<string>> AddPhotosToMinioAsync(AddShelterPetPhoto command)
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
    }
}