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
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers.ShelterPets
{
    public class CreateShelterPetHandler : ICommandHandler<CreateShelterPet>
    {
        private readonly ILogger<CreateShelterPetHandler> _logger;
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;
        private readonly IShelterRepository _shelterRepository;


        public CreateShelterPetHandler(ILogger<CreateShelterPetHandler> logger, IEventProcessor eventProcessor,
            IShelterPetRepository repository, IGrpcPhotoService photoService, IShelterRepository shelterRepository)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
            _repository = repository;
            _photoService = photoService;
            _shelterRepository = shelterRepository;
        }

        public async Task HandleAsync(CreateShelterPet command)
        {
            Shelter shelter = await GetShelterAsync(command);
            ValidIfUserOwnShelter(command, shelter);

            ShelterPet pet = ShelterPet.Create(command.Id, command.UserId, command.Name, command.Sex, command.Race, command.Species,
                String.Empty, command.BirthDay, command.Color, command.Weight, command.Sterilization,
                 command.ShelterId, shelter.Name, shelter.Address, shelter.GeoLocation, command.Description);

            await AddMainPhotoToMinioAsync(command, pet);
            await AddPhotosToMinioAsync(command, pet);
            
            await _repository.AddAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
            
        }

        private static void ValidIfUserOwnShelter(CreateShelterPet command, Shelter shelter)
        {
            if (shelter.Owners.Any(x => x != command.UserId))
            {
                throw new UserNotOwnerOfShelterException(command.UserId, command.ShelterId);
            }
        }

        private async Task<Shelter> GetShelterAsync(CreateShelterPet command)
        {
            Shelter shelter = await _shelterRepository.GetAsync(command.ShelterId);
            if (shelter is null)
            {
                throw new ShelterDoesNotExistsException(command.ShelterId);
            }

            return shelter;
        }

        private async Task AddPhotosToMinioAsync(CreateShelterPet command, ShelterPet pet)
        {
            if (command.Photos != null)
            {
                ICollection<string> photos = new Collection<string>();

                try
                {
                    foreach (File photo in command.Photos)
                    {
                        photos.Add(await _photoService.AddAsync(photo.Name, command.UserId, true, photo.Content,
                            BucketName.PetPhotos));
                    }

                    pet.SetPhotos(photos);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }

        private async Task AddMainPhotoToMinioAsync(CreateShelterPet command, ShelterPet pet)
        {
            try
            {
                string path = await _photoService.AddAsync(command.MainPhoto.Name, command.UserId, true,
                    command.MainPhoto.Content, BucketName.PetPhotos);
                pet.UpdateMainPhoto(path, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}