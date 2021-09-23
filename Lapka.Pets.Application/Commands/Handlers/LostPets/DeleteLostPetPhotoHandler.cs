using System;
using System.Collections.Generic;
using System.Linq;
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
    public class DeleteLostPetPhotoHandler : ICommandHandler<DeleteLostPetPhoto>
    {
        private readonly ILostPetRepository _repository;
        private readonly IEventProcessor _eventProcessor;
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainToIntegrationEventMapper _eventMapper;

        public DeleteLostPetPhotoHandler(ILostPetRepository repository, IEventProcessor eventProcessor,
            IMessageBroker messageBroker, IDomainToIntegrationEventMapper eventMapper)
        {
            _repository = repository;
            _eventProcessor = eventProcessor;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(DeleteLostPetPhoto command)
        {
            LostPet pet = await GetLostPetAsync(command);
            ValidIfUserIsOwnerOfPet(command, pet);
            ValidIfPhotosExists(command, pet);

            pet.RemovePhotos(command.PhotoPaths);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
            IEnumerable<IEvent> events = _eventMapper.MapAll(pet.Events);
            await _messageBroker.PublishAsync(events);
        }

        private static void ValidIfPhotosExists(DeleteLostPetPhoto command, LostPet pet)
        {
            foreach (string path in command.PhotoPaths)
            {
                if (!pet.PhotoPaths.Contains(path))
                {
                    throw new PhotoNotFoundException(path);
                }
            }
        }

        private static void ValidIfUserIsOwnerOfPet(DeleteLostPetPhoto command, LostPet pet)
        {
            if (pet.UserId != command.UserId)
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.Value.ToString());
            }
        }

        private async Task<LostPet> GetLostPetAsync(DeleteLostPetPhoto command)
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