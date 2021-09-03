using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Microsoft.Extensions.Logging;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class CreateLostPetHandler : ICommandHandler<CreateLostPet>
    {
        private readonly ILogger<CreateLostPetHandler> _logger;
        private readonly ILostPetService _petService;

        public CreateLostPetHandler(ILogger<CreateLostPetHandler> logger, ILostPetService petService)
        {
            _logger = logger;
            _petService = petService;
        }

        public async Task HandleAsync(CreateLostPet command)
        {
            LostPet pet = LostPet.Create(command.Id, command.UserId, command.Name, command.Sex, command.Race,
                command.Species, command.MainPhoto.Id,
                command.Photos == null ? new List<Guid>() : command.Photos.IdsAsGuidList(),
                DateTime.Now.Subtract(TimeSpan.FromDays(365 * command.Age)), command.Color, command.Weight,
                command.OwnerName, command.PhoneNumber, command.LostDate, command.LostAddress, command.Description);
            
            await _petService.AddAsync(_logger, command.MainPhoto, command.Photos, pet);
        }
        
    }
}