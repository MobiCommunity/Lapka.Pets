using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.Handlers.Helpers;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdateUserPetHandler : ICommandHandler<UpdateUserPet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<UserPet> _petRepository;

        public UpdateUserPetHandler(IEventProcessor eventProcessor, IPetRepository<UserPet> petRepository)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
        }

        public async Task HandleAsync(UpdateUserPet command)
        {
            UserPet pet = await _petRepository.GetByIdAsync(command.Id);
            UserPetHelpers.ValidateUserAndPet(command.UserId, command.Id, pet);

            pet.Update(command.Name, command.Race, command.Species, command.Sex, command.DateOfBirth,
                command.Sterilization, command.Weight, command.Color);

            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}