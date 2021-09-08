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
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;
        private readonly IGrpcIdentityService _grpcIdentity;
        private readonly ILogger<DeleteShelterPetHandler> _logger;

        public DeletePetHandlerTests()
        {          
            _logger = Substitute.For<ILogger<DeleteShelterPetHandler>>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _repository = Substitute.For<IShelterPetRepository>();
            _photoService = Substitute.For<IGrpcPhotoService>();
            _grpcIdentity = Substitute.For<IGrpcIdentityService>();

            _handler = new DeleteShelterPetHandler(_logger, _eventProcessor, _repository, _photoService, _grpcIdentity);
        }

        private Task Act(DeleteShelterPet command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_pet_should_delete()
        {
            Guid userId = Guid.NewGuid();
            ShelterPet pet = Extensions.ArrangePet(userId: userId);
            
            DeleteShelterPet command = new DeleteShelterPet(pet.Id.Value, userId);

            _repository.GetByIdAsync(command.Id).Returns(pet);
            _grpcIdentity.IsUserOwnerOfShelter(pet.ShelterId, command.UserId).Returns(true);

            await Act(command);

            await _repository.Received().DeleteAsync(pet);
            await _photoService.Received().DeleteAsync(pet.MainPhotoId, BucketName.PetPhotos);
        }
        
        [Fact]
        public async Task given_valid_pet_with_multiple_photos_should_delete()
        {
            Guid userId = Guid.NewGuid();
            ShelterPet pet = Extensions.ArrangePet(userId: userId);       
            
            DeleteShelterPet command = new DeleteShelterPet(pet.Id.Value, userId);

            _repository.GetByIdAsync(command.Id).Returns(pet);
            _grpcIdentity.IsUserOwnerOfShelter(pet.ShelterId, command.UserId).Returns(true);

            await Act(command);

            await _repository.Received().DeleteAsync(pet);
        }
    }
}