using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class AddUserPetPhotoHandler : ICommandHandler<AddUserPetPhoto>
    {
        private readonly IUserPetService _petService;
        private readonly IUserPetPhotoService _petPhotoService;
        public AddUserPetPhotoHandler(IUserPetService petService, IUserPetPhotoService petPhotoService)
        {
            _petService = petService;
            _petPhotoService = petPhotoService;
        }

        public async Task HandleAsync(AddUserPetPhoto command)
        {
            UserPet pet = await _petService.GetAsync(command.PetId);
            _petService.ValidIfUserIsOwnerOfPet(pet, command.UserId);
            
            await _petPhotoService.AddPetPhotosAsync(command.Photos, pet);
            await _petService.UpdateAsync(pet);
        }
    }
}