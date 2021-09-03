using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeleteLostPetHandler : ICommandHandler<DeleteLostPet>
    {
        private readonly ILogger<DeleteLostPetHandler> _logger;
        private readonly ILostPetService _petService;
        
        public DeleteLostPetHandler(ILogger<DeleteLostPetHandler> logger, ILostPetService petService)
        {
            _logger = logger;
            _petService = petService;
        }

        public async Task HandleAsync(DeleteLostPet command)
        {
            LostPet pet = await _petService.GetAsync(command.PetId);
            _petService.ValidIfUserIsOwnerOfPet(pet, command.UserId);

            await _petService.DeleteAsync(_logger, pet);
        }
    }
}