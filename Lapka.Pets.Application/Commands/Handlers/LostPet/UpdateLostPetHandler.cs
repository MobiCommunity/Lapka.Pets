﻿using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdateLostPetHandler : ICommandHandler<UpdateLostPet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly ILostPetRepository _repository;

        public UpdateLostPetHandler(IEventProcessor eventProcessor, ILostPetRepository repository)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
        }

        public async Task HandleAsync(UpdateLostPet command)
        {
            LostPet pet = await _repository.GetByIdAsync(command.Id);
            if (pet == null)
            {
                throw new PetNotFoundException(command.Id);
            }
            if (pet.UserId != command.UserId)
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.Value.ToString());
            }

            pet.Update(command.Name, command.Race, command.Species, command.Sex,
                DateTime.Now.Subtract(TimeSpan.FromDays(365 * command.Age)), command.Weight,
                command.Color, command.OwnerName, command.PhoneNumber, command.LostDate, command.LostAddress,
                command.Description);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}