using System;
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
                command.Species, command.MainPhoto.Id, command.BirthDay, command.Color, command.Weight,
                command.Sterilization, command.Photos.IdsAsGuidList());

            await AddMainPhoto(command, pet);
            await AddPhotos(command, pet);
            
            await _repository.AddAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
            
        }
        private async Task AddPhotos(CreateUserPet command, UserPet pet)
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

        private async Task AddMainPhoto(CreateUserPet command, UserPet pet)
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