using System;
using System.Collections.Generic;
using System.Linq;
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
    public class DeletePetPhotoHandlerTests
    {
        private readonly DeleteShelterPetPhotoHandler _handler;
        private readonly IShelterPetService _petService;
        private readonly IShelterPetPhotoService _petPhotoService;

        public DeletePetPhotoHandlerTests()
        {
            _petPhotoService = Substitute.For<IShelterPetPhotoService>();
            _petService = Substitute.For<IShelterPetService>();
            _handler = new DeleteShelterPetPhotoHandler(_petService, _petPhotoService);
        }

        private Task Act(DeleteShelterPetPhoto command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_pet_photo_path_should_delete()
        {
            ShelterPet pet = Extensions.ArrangePet(photoIds: new List<Guid>
            {
                Guid.NewGuid()
            });
            
            Guid photoPath = pet.PhotoIds.First();
            DeleteShelterPetPhoto command = new DeleteShelterPetPhoto(pet.Id.Value, photoPath);

            _petService.GetAsync(command.PetId).Returns(pet);

            await Act(command);

            await _petPhotoService.Received().DeletePetPhotoAsync(photoPath, pet);
        }
    }
}