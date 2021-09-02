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
    public class CreateShelterPetHandler : ICommandHandler<CreateShelterPet>
    {
        private readonly ILogger<CreateShelterPetHandler> _logger;
        private readonly IShelterPetService _petService;

        public CreateShelterPetHandler(ILogger<CreateShelterPetHandler> logger, IShelterPetService petService)
        {
            _logger = logger;
            _petService = petService;
        }

        public async Task HandleAsync(CreateShelterPet command)
        {
            ShelterPet pet = ShelterPet.Create(command.Id, command.Name, command.Sex, command.Race, command.Species,
                command.MainPhoto.Id, command.BirthDay, command.Color, command.Weight, command.Sterilization,
                command.ShelterAddress, command.Description,
                command.Photos == null ? new List<Guid>() : command.Photos.IdsAsGuidList());

            await _petService.AddAsync(_logger, command.MainPhoto, null, pet);
        }
    }
}