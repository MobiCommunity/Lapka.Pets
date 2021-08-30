﻿using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class AddVisitHandler : ICommandHandler<AddVisit>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<UserPet> _repository;

        public AddVisitHandler(IEventProcessor eventProcessor, IPetRepository<UserPet> repository)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
        }
        public async Task HandleAsync(AddVisit command)
        {
            UserPet pet = await _repository.GetByIdAsync(command.PetId);
            if (!pet.UserId.Equals(command.UserId))
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.ToString());
            }
            
            pet.AddLastVisit(command.Visit);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}