using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.UserPets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers.UserPets
{
    public class UpdateUserPetHandler : ICommandHandler<UpdateUserPet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserPetRepository _repository;

        public UpdateUserPetHandler(IEventProcessor eventProcessor, IUserPetRepository repository)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
        }

        public async Task HandleAsync(UpdateUserPet command)
        {
            UserPet pet = await GetUserPetAsync(command);
            ValidIfUserIsOwnerOfPet(command, pet);

            pet.Update(command.Name, command.Race, command.Species, command.Sex, command.DateOfBirth,
                command.Sterilization, command.Weight, command.Color);

            await _repository.UpdateAsync(pet);
            await _eventProcessor.ProcessAsync(pet.Events);
        }

        private static void ValidIfUserIsOwnerOfPet(UpdateUserPet command, UserPet pet)
        {
            if (pet.UserId != command.UserId)
            {
                throw new PetDoesNotBelongToUserException(command.UserId.ToString(), pet.Id.Value.ToString());
            }
        }

        private async Task<UserPet> GetUserPetAsync(UpdateUserPet command)
        {
            UserPet pet = await _repository.GetByIdAsync(command.Id);
            if (pet == null)
            {
                throw new PetNotFoundException(command.Id);
            }

            return pet;
        }
    }
}