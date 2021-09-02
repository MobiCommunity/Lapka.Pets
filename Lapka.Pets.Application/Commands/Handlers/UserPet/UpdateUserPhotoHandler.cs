using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdateUserPhotoHandler : ICommandHandler<UpdateUserPetPhoto>
    {
        private readonly IUserPetService _petService;
        private readonly IUserPetPhotoService _petPhotoService;

        public UpdateUserPhotoHandler(IUserPetService petService, IUserPetPhotoService petPhotoService)
        {
            _petService = petService;
            _petPhotoService = petPhotoService;
        }

        public async Task HandleAsync(UpdateUserPetPhoto command)
        {
            UserPet pet = await _petService.GetAsync(command.Id);
            _petService.ValidIfUserIsOwnerOfPet(pet, command.UserId);
            
            await _petPhotoService.UpdatePetPhotosAsync(pet.MainPhotoId, command.Photo, pet);
            
            await _petService.UpdateAsync(pet);
        }
    }
}