using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdatePetHandler : ICommandHandler<UpdatePet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository _petRepository;

        public UpdatePetHandler(IEventProcessor eventProcessor, IPetRepository petRepository)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
        }

        public async Task HandleAsync(UpdatePet command)
        {
            Pet pet = await _petRepository.GetByIdAsync(command.Id);
            if (pet is null)
            {
                throw new PetNotFoundException(command.Id);
            }
            
            pet.Update(command.Name, command.Race, command.Species, command.Sex, command.DateOfBirth, command.Description,
                command.ShelterAddress, command.Sterilization, command.Weight, command.Color);
            
            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}