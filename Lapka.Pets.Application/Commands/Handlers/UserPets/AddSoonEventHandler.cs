using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.UserPets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers.UserPets
{
    public class AddSoonEventHandler : ICommandHandler<AddSoonEvent>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserPetRepository _repository;

        public AddSoonEventHandler(IEventProcessor eventProcessor, IUserPetRepository repository)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
        }
        public async Task HandleAsync(AddSoonEvent command)
        {
            UserPet pet = await GetUserPetAsync(command);
            ValidIfUserIsOwnerOfPet(command, pet);
            
            pet.AddSoonEvent(command.SoonEvent);
            
            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        private static void ValidIfUserIsOwnerOfPet(AddSoonEvent command, UserPet pet)
        {
            if (pet.UserId != command.UserId)
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.Value.ToString());
            }
        }

        private async Task<UserPet> GetUserPetAsync(AddSoonEvent command)
        {
            UserPet pet = await _repository.GetByIdAsync(command.PetId);
            if (pet == null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            return pet;
        }
    }
}