using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Lapka.Pets.Application.Commands.LostPets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers.LostPets
{
    public class UpdateLostPetPhotoHandler : ICommandHandler<UpdateLostPetPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly ILostPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainToIntegrationEventMapper _eventMapper;

        public UpdateLostPetPhotoHandler(IEventProcessor eventProcessor, ILostPetRepository repository,
            IGrpcPhotoService photoService, IMessageBroker messageBroker, IDomainToIntegrationEventMapper eventMapper)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _photoService = photoService;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(UpdateLostPetPhoto command)
        {
            LostPet pet = await GetLostPetAsync(command);
            string oldPhotoPath = pet.MainPhotoPath;

            ValidIfUserIsOwnerOfPet(command, pet);

            string path = await AddPhotoToMinioAsync(command);

            pet.UpdateMainPhoto(path, oldPhotoPath);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
            IEnumerable<IEvent> events = _eventMapper.MapAll(pet.Events);
            await _messageBroker.PublishAsync(events);
        }

        private async Task<string> AddPhotoToMinioAsync(UpdateLostPetPhoto command)
        {
            string path;
            try
            {
                path = await _photoService.AddAsync(command.Photo.Name, command.UserId, true, command.Photo.Content,
                    BucketName.PetPhotos);
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }

            return path;
        }
        
        private static void ValidIfUserIsOwnerOfPet(UpdateLostPetPhoto command, LostPet pet)
        {
            if (pet.UserId != command.UserId)
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.Value.ToString());
            }
        }

        private async Task<LostPet> GetLostPetAsync(UpdateLostPetPhoto command)
        {
            LostPet pet = await _repository.GetByIdAsync(command.Id);
            if (pet == null)
            {
                throw new PetNotFoundException(command.Id);
            }

            return pet;
        }
    }
}