using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdatePetHandler : ICommandHandler<UpdatePet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository _petRepository;
        private readonly IPetQueryService _queryService;

        public UpdatePetHandler(IEventProcessor eventProcessor, IPetRepository petRepository, IPetQueryService queryService)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
            _queryService = queryService;
        }

        public async Task HandleAsync(UpdatePet command)
        {
            Pet pet = await _queryService.GetByIdAsync(command.Id);
            if (pet is null) throw new PetNotFoundException();
            
            pet.Update(command.Name, command.Race, command.Sex, command.DateOfBirth, command.Description,
                command.ShelterAddress, command.Sterilization, command.Weight, command.Color);

            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}