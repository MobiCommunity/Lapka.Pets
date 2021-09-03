using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class AddShelterPetPhotoHandler : ICommandHandler<AddShelterPetPhoto>
    {
        private readonly IShelterPetService _petService;
        private readonly IShelterPetPhotoService _petPhotoService;

        public AddShelterPetPhotoHandler(IShelterPetService petService, IShelterPetPhotoService petPhotoService)
        {
            _petService = petService;
            _petPhotoService = petPhotoService;
        }
        public async Task HandleAsync(AddShelterPetPhoto command)
        {
            ShelterPet pet = await _petService.GetAsync(command.PetId);

            await _petPhotoService.AddPetPhotosAsync(command.Photos, pet);
            await _petService.UpdateAsync(pet);
        }
    }
}