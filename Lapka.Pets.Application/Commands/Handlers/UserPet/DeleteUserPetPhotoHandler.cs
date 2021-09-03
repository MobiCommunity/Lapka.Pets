using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeleteUserPetPhotoHandler : ICommandHandler<DeleteUserPetPhoto>
    {
        private readonly IUserPetService _petService;
        private readonly IUserPetPhotoService _petPhotoService;

        public DeleteUserPetPhotoHandler(IUserPetService petService, IUserPetPhotoService petPhotoService)
        {
            _petService = petService;
            _petPhotoService = petPhotoService;
        }
        public async Task HandleAsync(DeleteUserPetPhoto command)
        {
            UserPet pet = await _petService.GetAsync(command.PetId);
            _petService.ValidIfUserIsOwnerOfPet(pet, command.UserId);
            
            await _petPhotoService.DeletePetPhotoAsync(command.PhotoId, pet);
            await _petService.UpdateAsync(pet);
        }
    }
}