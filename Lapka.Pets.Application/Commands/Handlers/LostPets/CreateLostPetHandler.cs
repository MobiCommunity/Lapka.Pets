using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.LostPets;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace Lapka.Pets.Application.Commands.Handlers.LostPets
{
    public class CreateLostPetHandler : ICommandHandler<CreateLostPet>
    {
        private readonly ILogger<CreateLostPetHandler> _logger;
        private readonly IEventProcessor _eventProcessor;
        private readonly ILostPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;

        public CreateLostPetHandler(ILogger<CreateLostPetHandler> logger, IEventProcessor eventProcessor,
            ILostPetRepository repository, IGrpcPhotoService photoService)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
            _repository = repository;
            _photoService = photoService;
        }

        public async Task HandleAsync(CreateLostPet command)
        {
            LostPet pet = LostPet.Create(command.Id, command.UserId, command.Name, command.Sex, command.Race,
                command.Species, string.Empty, DateTime.UtcNow.Subtract(TimeSpan.FromDays(365 * command.Age)),
                command.Color, command.Weight, command.OwnerName, command.PhoneNumber, command.LostDate,
                command.LostAddress, command.Description);

            await AddMainPhotoToMinioAsync(command, pet);
            await AddPhotosToMinioAsync(command, pet);

            await _repository.AddAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        private async Task AddPhotosToMinioAsync(CreateLostPet command, LostPet pet)
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

        private async Task AddMainPhotoToMinioAsync(CreateLostPet command, LostPet pet)
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