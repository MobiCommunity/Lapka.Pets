using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class AddVisitHandler : ICommandHandler<AddVisit>
    {
        private readonly IUserPetService _petService;

        public AddVisitHandler(IUserPetService petService)
        {
            _petService = petService;
        }
        public async Task HandleAsync(AddVisit command)
        {
            UserPet pet = await _petService.GetAsync(command.PetId);
            _petService.ValidIfUserIsOwnerOfPet(pet, command.UserId);
            
            pet.AddLastVisit(command.Visit);

            await _petService.UpdateAsync(pet);
        }
    }
}