using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Commands.Handlers;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using NSubstitute;
using Xunit;
using File = Lapka.Pets.Core.ValueObjects.File;

namespace Lapka.Pets.Tests.Unit.Application.Handlers
{
    public class AddPetPhotoHandlerTests
    {
        private readonly AddShelterPetPhotoHandler _handler;
        private readonly IShelterPetService _petService;
        private readonly IShelterPetPhotoService _petPhotoService;

        public AddPetPhotoHandlerTests()
        {
            _petPhotoService = Substitute.For<IShelterPetPhotoService>();
            _petService = Substitute.For<IShelterPetService>();
            _handler = new AddShelterPetPhotoHandler(_petService, _petPhotoService);
        }

        private Task Act(AddShelterPetPhoto command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_pet_path_should_add()
        {
            Guid userId = Guid.NewGuid();
            ShelterPet aggregatePet = Extensions.ArrangePet();
            List<PhotoFile> files = new List<PhotoFile>();
            files.Add(Extensions.ArrangePhotoFile());

            AddShelterPetPhoto command = new AddShelterPetPhoto(aggregatePet.Id.Value, userId, files);

            _petService.GetAsync(command.PetId).Returns(aggregatePet);

            await Act(command);

            await _petPhotoService.Received().AddPetPhotosAsync(files, aggregatePet);
        }
    }
}