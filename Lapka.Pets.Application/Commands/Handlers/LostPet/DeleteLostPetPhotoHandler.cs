using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeleteLostPetPhotoHandler : ICommandHandler<DeleteLostPetPhoto>
    {
        private readonly ILostPetService _petService;
        private readonly ILostPetPhotoService _petPhotoService;

        public DeleteLostPetPhotoHandler(ILostPetService petService, ILostPetPhotoService petPhotoService)
        {
            _petService = petService;
            _petPhotoService = petPhotoService;
        }
        public async Task HandleAsync(DeleteLostPetPhoto command)
        {
            LostPet pet = await _petService.GetAsync(command.PetId);
            
            await _petPhotoService.DeletePetPhotoAsync(command.PhotoId, pet);
            await _petService.UpdateAsync(pet);
        }
    }
}