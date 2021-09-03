using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class CreateUserPetHandler : ICommandHandler<CreateUserPet>
    {
        private readonly IUserPetService _petService;
        private readonly ILogger<CreateUserPetHandler> _logger;

        public CreateUserPetHandler(ILogger<CreateUserPetHandler> logger, IUserPetService petService)
        {
            _logger = logger;
            _petService = petService;
        }

        public async Task HandleAsync(CreateUserPet command)
        {
            UserPet pet = UserPet.Create(command.Id, command.UserId, command.Name, command.Sex, command.Race,
                command.Species, command.MainPhoto.Id, command.BirthDay, command.Color, command.Weight,
                command.Sterilization, command.Photos.IdsAsGuidList());

            await _petService.AddAsync(_logger, command.MainPhoto, null, pet);
        }
    }
}