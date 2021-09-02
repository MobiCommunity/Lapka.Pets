using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class LikePetHandler : ICommandHandler<LikePet>
    {
        private readonly IShelterPetService _petService;
        private readonly IPetLikesService _petLikesService;

        public LikePetHandler(IShelterPetService petService, IPetLikesService petLikesService)
        {
            _petService = petService;
            _petLikesService = petLikesService;
        }
        public async Task HandleAsync(LikePet command)
        {
            ShelterPet pet = await _petService.GetAsync(command.PetId);
            
            await _petLikesService.LikePet(pet.Id.Value, command.UserId);
        }
    }
}