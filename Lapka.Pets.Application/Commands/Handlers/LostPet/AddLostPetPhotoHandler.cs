using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class AddLostPetPhotoHandler : ICommandHandler<AddLostPetPhoto>
    {
        private readonly ILostPetService _petService;
        private readonly ILostPetPhotoService _petPhotoService;

        public AddLostPetPhotoHandler(ILostPetService petService, ILostPetPhotoService petPhotoService)
        {
            _petService = petService;
            _petPhotoService = petPhotoService;
        }
        public async Task HandleAsync(AddLostPetPhoto command)
        {
            LostPet pet = await _petService.GetAsync(command.PetId);

            await _petPhotoService.AddPetPhotosAsync(command.Photos, pet);
            await _petService.UpdateAsync(pet);
        }
    }
}