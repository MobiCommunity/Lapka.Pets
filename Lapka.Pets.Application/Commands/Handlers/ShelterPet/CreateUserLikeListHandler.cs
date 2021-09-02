using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class CreateUserLikeListHandler : ICommandHandler<CreateUserLikeList>
    {
        private readonly IPetLikesService _likesService;

        public CreateUserLikeListHandler(IPetLikesService likesService)
        {
            _likesService = likesService;
        }

        public async Task HandleAsync(CreateUserLikeList command) => await _likesService.AddUserPetList(command.UserId);
    }
}