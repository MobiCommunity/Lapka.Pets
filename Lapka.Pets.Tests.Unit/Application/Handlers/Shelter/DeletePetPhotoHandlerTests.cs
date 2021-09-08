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
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;
        private readonly IGrpcIdentityService _grpcIdentity;

        public DeletePetPhotoHandlerTests()
        {
            _eventProcessor = Substitute.For<IEventProcessor>();
            _repository = Substitute.For<IShelterPetRepository>();
            _photoService = Substitute.For<IGrpcPhotoService>();
            _grpcIdentity = Substitute.For<IGrpcIdentityService>();

            _handler = new DeleteShelterPetPhotoHandler(_repository, _eventProcessor, _photoService, _grpcIdentity);
        }

        private Task Act(DeleteShelterPetPhoto command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_pet_photo_path_should_delete()
        {
            Guid userId = Guid.NewGuid();
            ShelterPet pet = Extensions.ArrangePet(photoIds: new List<Guid>
            {
                Guid.NewGuid()
            }, userId: userId);

            Guid photoPath = pet.PhotoIds.First();
            DeleteShelterPetPhoto command = new DeleteShelterPetPhoto(pet.Id.Value, userId, photoPath);

            _repository.GetByIdAsync(command.PetId).Returns(pet);
            _grpcIdentity.IsUserOwnerOfShelter(pet.ShelterId, command.UserId).Returns(true);

            await Act(command);

            await _photoService.Received().DeleteAsync(photoPath, BucketName.PetPhotos);
        }
    }
}