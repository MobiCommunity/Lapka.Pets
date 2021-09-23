using System;
using System.Collections.Generic;
using System.Linq;
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
    public class DeleteUserPetPhotoHandler : ICommandHandler<DeleteUserPetPhoto>
    {
        private readonly IUserPetRepository _repository;
        private readonly IEventProcessor _eventProcessor;
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainToIntegrationEventMapper _eventMapper;


        public DeleteUserPetPhotoHandler(IUserPetRepository repository, IEventProcessor eventProcessor,
            IMessageBroker messageBroker, IDomainToIntegrationEventMapper eventMapper)
        {
            _repository = repository;
            _eventProcessor = eventProcessor;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }
        public async Task HandleAsync(DeleteUserPetPhoto command)
        {
            UserPet pet = await GetUserPetAsync(command);
            ValidIfUserIsOwnerOfPet(command, pet);
            ValidIfPhotosExists(command, pet);
            
            pet.RemovePhotos(command.PhotoPaths);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
            IEnumerable<IEvent> events = _eventMapper.MapAll(pet.Events);
            await _messageBroker.PublishAsync(events);
        }

        private static void ValidIfPhotosExists(DeleteUserPetPhoto command, UserPet pet)
        {
            foreach (string path in command.PhotoPaths)
            {
                if (!pet.PhotoPaths.Contains(path))
                {
                    throw new PhotoNotFoundException(path);
                }
            }
        }

        private static void ValidIfUserIsOwnerOfPet(DeleteUserPetPhoto command, UserPet pet)
        {
            if (pet.UserId != command.UserId)
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.Value.ToString());
            }
        }

        private async Task<UserPet> GetUserPetAsync(DeleteUserPetPhoto command)
        {
            UserPet pet = await _repository.GetByIdAsync(command.PetId);
            if (pet is null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            return pet;
        }
    }
}