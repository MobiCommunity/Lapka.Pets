using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeleteUserPetHandler : ICommandHandler<DeleteUserPet>
    {
        private readonly ILogger<DeleteUserPetHandler> _logger;
        private readonly IUserPetService _petService;

        public DeleteUserPetHandler(ILogger<DeleteUserPetHandler> logger, IUserPetService petService)
        {
            _logger = logger;
            _petService = petService;
        }

        public async Task HandleAsync(DeleteUserPet command)
        {
            UserPet pet = await _petService.GetAsync(command.PetId);
            _petService.ValidIfUserIsOwnerOfPet(pet, command.UserId);
            
            await _petService.DeleteAsync(_logger, pet);
        }
    }
}
