using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.UserPets;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers.UserPets
{
    public class CreateUserPetHandler : ICommandHandler<CreateUserPet>
    {
        private readonly ILogger<CreateUserPetHandler> _logger;
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;


        public CreateUserPetHandler(ILogger<CreateUserPetHandler> logger, IEventProcessor eventProcessor,
            IUserPetRepository repository, IGrpcPhotoService photoService)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
            _repository = repository;
            _photoService = photoService;
        }

        public async Task HandleAsync(CreateUserPet command)
        {
            UserPet pet = UserPet.Create(command.Id, command.UserId, command.Name, command.Sex, command.Race,
                command.Species, String.Empty, command.BirthDay, command.Color, command.Weight,
                command.Sterilization);

            await AddMainPhotoToMinioAsync(command, pet);
            await AddPhotos(command, pet);
            
            await _repository.AddAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
            
        }
        private async Task AddPhotos(CreateUserPet command, UserPet pet)
        {
            if (command.Photos != null)
            {
                ICollection<string> photos = new Collection<string>();

                try
                {
                    foreach (File photo in command.Photos)
                    {
                        photos.Add(await _photoService.AddAsync(photo.Name, command.UserId, false, photo.Content,
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

        private async Task AddMainPhotoToMinioAsync(CreateUserPet command, UserPet pet)
        {
            try
            {
                string path = await _photoService.AddAsync(command.MainPhoto.Name, command.UserId, false,
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