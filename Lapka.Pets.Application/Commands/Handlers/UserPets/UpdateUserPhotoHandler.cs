using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Lapka.Pets.Application.Commands.UserPets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers.UserPets
{
    public class UpdateUserPhotoHandler : ICommandHandler<UpdateUserPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainToIntegrationEventMapper _eventMapper;

        public UpdateUserPhotoHandler(IEventProcessor eventProcessor, IUserPetRepository repository,
            IGrpcPhotoService photoService, IMessageBroker messageBroker, IDomainToIntegrationEventMapper eventMapper)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _photoService = photoService;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(UpdateUserPetPhoto command)
        {
            UserPet pet = await GetUserPetAsync(command);
            ValidIfUserIsOwnerOfPet(command, pet);
            string oldPhotoPath = pet.MainPhotoPath;

            string path = await AddPhotoToMinioAsync(command);

            pet.UpdateMainPhoto(path, oldPhotoPath);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
            IEnumerable<IEvent> events = _eventMapper.MapAll(pet.Events);
            await _messageBroker.PublishAsync(events);
        }

        private static void ValidIfUserIsOwnerOfPet(UpdateUserPetPhoto command, UserPet pet)
        {
            if (pet.UserId != command.UserId)
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.Value.ToString());
            }
        }

        private async Task<UserPet> GetUserPetAsync(UpdateUserPetPhoto command)
        {
            UserPet pet = await _repository.GetByIdAsync(command.Id);
            if (pet == null)
            {
                throw new PetNotFoundException(command.Id);
            }

            return pet;
        }

        private async Task<string> AddPhotoToMinioAsync(UpdateUserPetPhoto command)
        {
            string path;
            try
            {
                path = await _photoService.AddAsync(command.Photo.Name, command.UserId, false, command.Photo.Content,
                    BucketName.PetPhotos);
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }

            return path;
        }
    }
}