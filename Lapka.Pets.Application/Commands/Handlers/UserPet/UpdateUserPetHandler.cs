using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdateUserPetHandler : ICommandHandler<UpdateUserPet>
    {
        private readonly IUserPetService _petService;
        public UpdateUserPetHandler(IUserPetService petService)
        {
            _petService = petService;
        }

        public async Task HandleAsync(UpdateUserPet command)
        {
            UserPet pet = await _petService.GetAsync(command.Id);
            _petService.ValidIfUserIsOwnerOfPet(pet, command.UserId);
            
            pet.Update(command.Name, command.Race, command.Species, command.Sex, command.DateOfBirth,
                command.Sterilization, command.Weight, command.Color);

            await _petService.UpdateAsync(pet);
        }
    }
}