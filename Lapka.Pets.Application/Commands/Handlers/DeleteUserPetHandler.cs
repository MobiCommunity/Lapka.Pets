using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Microsoft.AspNetCore.Routing.Matching;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeleteUserPetHandler : ICommandHandler<DeleteUserPet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IPetRepository<UserPet> _petRepository;

        public DeleteUserPetHandler(IEventProcessor eventProcessor, IPetRepository<UserPet> petRepository)
        {
            _eventProcessor = eventProcessor;
            _petRepository = petRepository;
        }

        public async Task HandleAsync(DeleteUserPet command)
        {
            UserPet pet = await _petRepository.GetByIdAsync(command.PetId);
            if (pet is null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            pet.Delete();

            await _petRepository.DeleteAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }
    }
}