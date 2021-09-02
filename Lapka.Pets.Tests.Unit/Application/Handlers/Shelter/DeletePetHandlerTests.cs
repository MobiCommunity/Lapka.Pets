using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Commands.Handlers;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Lapka.Pets.Tests.Unit.Application.Handlers
{
    public class DeletePetHandlerTests
    {
        private readonly DeleteShelterPetHandler _handler;
        private readonly IShelterPetService _petService;
        private readonly ILogger<DeleteShelterPetHandler> _logger;

        public DeletePetHandlerTests()
        {          
            _logger = Substitute.For<ILogger<DeleteShelterPetHandler>>();
            _petService = Substitute.For<IShelterPetService>();
            _handler = new DeleteShelterPetHandler(_logger, _petService);
        }

        private Task Act(DeleteShelterPet command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_pet_should_delete()
        {
            ShelterPet pet = Extensions.ArrangePet();
            DeleteShelterPet command = new DeleteShelterPet(pet.Id.Value);

            _petService.GetAsync(command.Id).Returns(pet);

            await Act(command);

            await _petService.Received().DeleteAsync(_logger, pet);
        }
        
        [Fact]
        public async Task given_valid_pet_with_multiple_photos_should_delete()
        {
            ShelterPet pet = Extensions.ArrangePet();
            DeleteShelterPet command = new DeleteShelterPet(pet.Id.Value);

            _petService.GetAsync(command.Id).Returns(pet);

            await Act(command);

            await _petService.Received().DeleteAsync(_logger, pet);
        }
    }
}