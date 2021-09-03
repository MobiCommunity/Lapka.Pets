using System;
using System.IO;
using System.Threading.Tasks;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Commands.Handlers;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using File = Lapka.Pets.Core.ValueObjects.File;

namespace Lapka.Pets.Tests.Unit.Application.Handlers
{
    public class UpdateShelterPetPhotoHandlerTests
    {
        private readonly UpdateShelterPetPhotoHandler _handler;
        private readonly IShelterPetService _petService;
        private readonly IShelterPetPhotoService _petPhotoService;

        public UpdateShelterPetPhotoHandlerTests()
        {
            _petPhotoService = Substitute.For<IShelterPetPhotoService>();
            _petService = Substitute.For<IShelterPetService>();
            _handler = new UpdateShelterPetPhotoHandler(_petService, _petPhotoService);
        }

        private Task Act(UpdateShelterPetPhoto command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_pet_photo_should_update()
        {
            ShelterPet pet = Extensions.ArrangePet();
            PhotoFile file = Extensions.ArrangePhotoFile();
            Guid userId = Guid.NewGuid();
            
            UpdateShelterPetPhoto command = new UpdateShelterPetPhoto(pet.Id.Value, userId, file);

            _petService.GetAsync(command.PetId).Returns(pet);

            await Act(command);

            await _petService.Received().UpdateAsync(pet);
        }
    }
}