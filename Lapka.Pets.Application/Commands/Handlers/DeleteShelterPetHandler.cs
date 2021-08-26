using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Microsoft.AspNetCore.Routing.Matching;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeleteShelterPetHandler : ICommandHandler<DeletePet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<ShelterPet> _petRepository;

        public DeleteShelterPetHandler(IEventProcessor eventProcessor, IPetRepository<ShelterPet> petRepository)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
        }

        public async Task HandleAsync(DeletePet command)
        {
            ShelterPet pet = await _petRepository.GetByIdAsync(command.Id);
            if (pet is null)
            {
                throw new PetNotFoundException(command.Id);
            }

            pet.Delete();

            await _petRepository.DeleteAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}