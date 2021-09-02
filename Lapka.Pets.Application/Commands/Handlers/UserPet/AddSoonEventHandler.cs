using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class AddSoonEventHandler : ICommandHandler<AddSoonEvent>
    {
        private readonly IUserPetService _petService;
        public AddSoonEventHandler(IUserPetService petService)
        {
            _petService = petService;
        }
        public async Task HandleAsync(AddSoonEvent command)
        {
            UserPet pet = await _petService.GetAsync(command.PetId);
            _petService.ValidIfUserIsOwnerOfPet(pet, command.UserId);
            
            pet.AddSoonEvent(command.SoonEvent);

            await _petService.UpdateAsync(pet);
        }
    }
}