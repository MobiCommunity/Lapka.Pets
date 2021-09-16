using System;
using System.IO;
using System.Threading.Tasks;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Commands.Handlers;
using Lapka.Pets.Application.Commands.Handlers.ShelterPets;
using Lapka.Pets.Application.Commands.ShelterPets;
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
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;
        private readonly IGrpcIdentityService _grpcIdentity;

        public UpdateShelterPetPhotoHandlerTests()
        {
            _eventProcessor = Substitute.For<IEventProcessor>();
            _repository = Substitute.For<IShelterPetRepository>();
            _photoService = Substitute.For<IGrpcPhotoService>();
            _grpcIdentity = Substitute.For<IGrpcIdentityService>();

            _handler = new UpdateShelterPetPhotoHandler(_eventProcessor, _repository, _photoService, _grpcIdentity);
        }

        private Task Act(UpdateShelterPetPhoto command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_pet_photo_should_update()
        {
            Guid userId = Guid.NewGuid();
            ShelterPet pet = Extensions.ArrangePet(userId: userId);
            PhotoFile file = Extensions.ArrangePhotoFile();
            Guid oldPhotoId = pet.MainPhotoId;
            
            UpdateShelterPetPhoto command = new UpdateShelterPetPhoto(pet.Id.Value, userId, file);

            _repository.GetByIdAsync(command.PetId).Returns(pet);
            _grpcIdentity.IsUserOwnerOfShelter(pet.ShelterId, command.UserId).Returns(true);

            await Act(command);

            await _repository.Received().UpdateAsync(pet);
            await _photoService.Received().DeleteAsync(oldPhotoId, BucketName.PetPhotos);
            await _photoService.Received().AddAsync(file.Id, file.Name, file.Content, BucketName.PetPhotos);
        }
    }
}