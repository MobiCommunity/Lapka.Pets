using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeleteShelterPetHandler : ICommandHandler<DeleteShelterPet>
    {
        private readonly ILogger<DeleteShelterPetHandler> _logger;
        private readonly IShelterPetService _petService;
        
        public DeleteShelterPetHandler(ILogger<DeleteShelterPetHandler> logger, IShelterPetService petService)
        {
            _logger = logger;
            _petService = petService;
        }

        public async Task HandleAsync(DeleteShelterPet command)
        {
            ShelterPet pet = await _petService.GetAsync(command.Id);
            
            await _petService.DeleteAsync(_logger, pet);
        }
    }
}