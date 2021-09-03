using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdateShelterPetPhotoHandler : ICommandHandler<UpdateShelterPetPhoto>
    {
        private readonly IShelterPetService _petService;
        private readonly IShelterPetPhotoService _petPhotoService;

        public UpdateShelterPetPhotoHandler(IShelterPetService petService, IShelterPetPhotoService petPhotoService)
        {
            _petService = petService;
            _petPhotoService = petPhotoService;
        }

        public async Task HandleAsync(UpdateShelterPetPhoto command)
        {
            ShelterPet pet = await _petService.GetAsync(command.PetId);

            await _petPhotoService.UpdatePetPhotosAsync(pet.MainPhotoId, command.Photo, pet);
            
            await _petService.UpdateAsync(pet);
        }
    }
}