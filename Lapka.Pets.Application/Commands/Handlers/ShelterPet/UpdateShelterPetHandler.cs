using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdateShelterPetHandler : ICommandHandler<UpdateShelterPet>
    {
        private readonly IShelterPetService _petService;
        public UpdateShelterPetHandler(IShelterPetService petService)
        {
            _petService = petService;
        }

        public async Task HandleAsync(UpdateShelterPet command)
        {
            ShelterPet pet = await _petService.GetAsync(command.Id);
            _petService.ValidIfUserIsOwnerOfPet(pet, command.UserId);

            pet.Update(command.Name, command.Race, command.Species, command.Sex, command.DateOfBirth,
                command.Sterilization, command.Weight, command.Color, command.ShelterAddress, command.Description);

            await _petService.UpdateAsync(pet);
        }
    }
}