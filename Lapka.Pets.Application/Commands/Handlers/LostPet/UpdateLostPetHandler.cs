using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.Handlers.Helpers;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdateLostPetHandler : ICommandHandler<UpdateLostPet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<LostPet> _petRepository;

        public UpdateLostPetHandler(IEventProcessor eventProcessor, IPetRepository<LostPet> petRepository)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
        }

        public async Task HandleAsync(UpdateLostPet command)
        {
            LostPet pet = await PetHelpers.GetPetFromRepositoryAsync(_petRepository, command.Id);

            pet.Update(command.Name, command.Race, command.Species, command.Sex, command.BirthDate, command.Weight,
                command.Color, command.OwnerName, command.PhoneNumber, command.LostDate, command.LostAddress,
                command.Description);

            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}