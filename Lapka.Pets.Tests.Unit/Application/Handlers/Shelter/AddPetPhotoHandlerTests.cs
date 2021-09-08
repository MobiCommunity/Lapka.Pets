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
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;
        private readonly IGrpcIdentityService _grpcIdentity;

        public AddPetPhotoHandlerTests()
        {
            _eventProcessor = Substitute.For<IEventProcessor>();
            _repository = Substitute.For<IShelterPetRepository>();
            _photoService = Substitute.For<IGrpcPhotoService>();
            _grpcIdentity = Substitute.For<IGrpcIdentityService>();

            _handler = new AddShelterPetPhotoHandler(_eventProcessor, _repository, _photoService, _grpcIdentity);
        }

        private Task Act(AddShelterPetPhoto command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_pet_path_should_add()
        {
            Guid userId = Guid.NewGuid();
            ShelterPet aggregatePet = Extensions.ArrangePet(userId: userId);
            List<PhotoFile> files = new List<PhotoFile>();
            files.Add(Extensions.ArrangePhotoFile());
            files.Add(Extensions.ArrangePhotoFile());

            AddShelterPetPhoto command = new AddShelterPetPhoto(aggregatePet.Id.Value, userId, files);

            _repository.GetByIdAsync(command.PetId).Returns(aggregatePet);
            _grpcIdentity.IsUserOwnerOfShelter(aggregatePet.ShelterId, command.UserId).Returns(true);

            await Act(command);

            foreach (PhotoFile photo in files)
            {
                await _photoService.Received().AddAsync(photo.Id, photo.Name, photo.Content, BucketName.PetPhotos);
            }
        }
    }
}