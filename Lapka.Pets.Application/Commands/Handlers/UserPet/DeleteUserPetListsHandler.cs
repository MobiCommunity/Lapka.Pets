using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class DeleteUserPetListsHandler : ICommandHandler<DeleteUserPetLists>
    {
        private readonly ILogger<DeleteUserPetListsHandler> _logger;
        private readonly IUserPetService _petService;
        private readonly IPetLikesService _likesService;

        public DeleteUserPetListsHandler(ILogger<DeleteUserPetListsHandler> logger, IUserPetService petService,
            IPetLikesService likesService)
        {
            _logger = logger;
            _petService = petService;
            _likesService = likesService;
        }

        public async Task HandleAsync(DeleteUserPetLists command)
        {
            IEnumerable<UserPet> pets = await _petService.GetAllUserPets(command.UserId);
            foreach (UserPet pet in pets)
            {
                await _petService.DeleteAsync(_logger, pet);
            }

            await _likesService.DeleteUserPetList(command.UserId);
        }
    }
}