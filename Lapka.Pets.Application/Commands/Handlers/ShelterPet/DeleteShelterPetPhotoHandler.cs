using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeleteShelterPetPhotoHandler : ICommandHandler<DeleteShelterPetPhoto>
    {
        private readonly IShelterPetService _petService;
        private readonly IShelterPetPhotoService _petPhotoService;

        public DeleteShelterPetPhotoHandler(IShelterPetService petService, IShelterPetPhotoService petPhotoService)
        {
            _petService = petService;
            _petPhotoService = petPhotoService;
        }

        public async Task HandleAsync(DeleteShelterPetPhoto command)
        {
            ShelterPet pet = await _petService.GetAsync(command.PetId);

            await _petPhotoService.DeletePetPhotoAsync(command.PhotoId, pet);
            await _petService.UpdateAsync(pet);
        }
    }
}