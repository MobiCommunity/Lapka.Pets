using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers
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
                command.Species, command.MainPhoto.Id,
                command.Photos == null ? new List<Guid>() : command.Photos.IdsAsGuidList(),
                DateTime.Now.Subtract(TimeSpan.FromDays(365 * command.Age)), command.Color, command.Weight,
                command.OwnerName, command.PhoneNumber, command.LostDate, command.LostAddress, command.Description);

            await AddMainPhoto(command, pet);
            await AddPhotos(command, pet);
            
            await _repository.AddAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        private async Task AddPhotos(CreateLostPet command, LostPet pet)
        {
            if (command.Photos != null)
            {
                try
                {
                    foreach (PhotoFile photo in command.Photos)
                    {
                        await _photoService.AddAsync(photo.Id, photo.Name, photo.Content, BucketName.PetPhotos);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    foreach (PhotoFile photo in command.Photos)
                    {
                        pet.RemovePhoto(photo.Id);
                    }
                }
            }
        }

        private async Task AddMainPhoto(CreateLostPet command, LostPet pet)
        {
            try
            {
                await _photoService.AddAsync(command.MainPhoto.Id, command.MainPhoto.Name, command.MainPhoto.Content,
                    BucketName.PetPhotos);
                pet.UpdateMainPhoto(command.MainPhoto.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                pet.UpdateMainPhoto(Guid.Empty);
            }
        }
    }
}