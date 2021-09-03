using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DislikePetHandler : ICommandHandler<DislikePet>
    {
        private readonly IShelterPetService _petService;
        private readonly IPetLikesService _petLikesService;

        public DislikePetHandler(IShelterPetService petService, IPetLikesService petLikesService)
        {
            _petService = petService;
            _petLikesService = petLikesService;
        }
        public async Task HandleAsync(DislikePet command)
        {
            ShelterPet pet = await _petService.GetAsync(command.PetId);
            
            await _petLikesService.DislikePet(pet.Id.Value, command.UserId);
        }
    }
}