using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdateLostPetPhotoHandler : ICommandHandler<UpdateLostPetPhoto>
    {
        private readonly ILostPetService _petService;
        private readonly ILostPetPhotoService _petPhotoService;

        public UpdateLostPetPhotoHandler(ILostPetService petService, ILostPetPhotoService petPhotoService)
        {
            _petService = petService;
            _petPhotoService = petPhotoService;
        }

        public async Task HandleAsync(UpdateLostPetPhoto command)
        {
            LostPet pet = await _petService.GetAsync(command.Id);
            _petService.ValidIfUserIsOwnerOfPet(pet, command.UserId);

            await _petPhotoService.UpdatePetPhotosAsync(pet.MainPhotoId, command.Photo, pet);
            
            await _petService.UpdateAsync(pet);
        }
    }
}