using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class CreatePetHandler : ICommandHandler<CreatePet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository _petRepository;

        public CreatePetHandler(IEventProcessor eventProcessor, IPetRepository petRepository)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
        }
        
        public async Task HandleAsync(CreatePet command)
        {
            Pet value = Pet.Create(command.Id, command.Name, command.Sex, command.Species, command.Race, command.BirthDay, command.Color,
                command.Weight, command.Sterilization, command.ShelterAddress, command.Description);
            
            await _petRepository.AddAsync(value);
            
            await _eventProcessor.ProcessAsync(value.Events);
        }
    }
}