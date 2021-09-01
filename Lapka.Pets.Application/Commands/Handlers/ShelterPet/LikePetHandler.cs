using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.Handlers.Helpers;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class LikePetHandler : ICommandHandler<LikePet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<ShelterPet> _petRepository;

        public LikePetHandler(IEventProcessor eventProcessor, IPetRepository<ShelterPet> petRepository)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
        }
        public async Task HandleAsync(LikePet command)
        {
            ShelterPet pet = await PetHelpers.GetPetFromRepositoryAsync(_petRepository, command.PetId);
            

            await _petRepository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}