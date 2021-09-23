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
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers.LostPets
{
    public class DeleteLostPetHandler : ICommandHandler<DeleteLostPet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly ILostPetRepository _repository;
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainToIntegrationEventMapper _eventMapper;

        public DeleteLostPetHandler(IEventProcessor eventProcessor, ILostPetRepository repository,
            IMessageBroker messageBroker, IDomainToIntegrationEventMapper eventMapper)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(DeleteLostPet command)
        {
            LostPet pet = await GetLostPetAsync(command);
            ValidIfUserIsOwnerOfPet(command, pet);

            pet.Delete();

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
            IEnumerable<IEvent> events = _eventMapper.MapAll(pet.Events);
            await _messageBroker.PublishAsync(events);
        }

        private static void ValidIfUserIsOwnerOfPet(DeleteLostPet command, LostPet pet)
        {
            if (pet.UserId != command.UserId)
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.Value.ToString());
            }
        }

        private async Task<LostPet> GetLostPetAsync(DeleteLostPet command)
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