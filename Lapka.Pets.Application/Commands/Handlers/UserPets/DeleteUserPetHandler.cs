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
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers.UserPets
{
    public class DeleteUserPetHandler : ICommandHandler<DeleteUserPet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserPetRepository _repository;
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainToIntegrationEventMapper _eventMapper;


        public DeleteUserPetHandler(IEventProcessor eventProcessor, IUserPetRepository repository,
            IMessageBroker messageBroker, IDomainToIntegrationEventMapper eventMapper)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(DeleteUserPet command)
        {
            UserPet pet = await GetUserPetAsync(command);
            ValidIfUserIsOwnerOfPet(command, pet);

            pet.Delete();

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
            IEnumerable<IEvent> events = _eventMapper.MapAll(pet.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }

        private static void ValidIfUserIsOwnerOfPet(DeleteUserPet command, UserPet pet)
        {
            if (pet.UserId != command.UserId)
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.Value.ToString());
            }
        }

        private async Task<UserPet> GetUserPetAsync(DeleteUserPet command)
        {
            UserPet pet = await _repository.GetByIdAsync(command.PetId);
            if (pet == null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            return pet;
        }
    }
}