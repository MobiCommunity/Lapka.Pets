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
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly DeleteShelterPetHandler _handler;
        private readonly IPetRepository<ShelterPet> _petRepository;
        private readonly ILogger<DeleteShelterPetHandler> _logger;

        public DeletePetHandlerTests()
        {
            _petRepository = Substitute.For<IPetRepository<ShelterPet>>();
            _grpcPhotoService = Substitute.For<IGrpcPhotoService>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _logger = Substitute.For<ILogger<DeleteShelterPetHandler>>();
            _handler = new DeleteShelterPetHandler(_logger, _eventProcessor, _petRepository, _grpcPhotoService);
        }

        private Task Act(DeleteShelterPet command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_pet_should_delete()
        {
            ShelterPet pet = Extensions.ArrangePet();
            DeleteShelterPet command = new DeleteShelterPet(pet.Id.Value);

            _petRepository.GetByIdAsync(command.Id).Returns(pet);

            await Act(command);

            await _petRepository.Received().DeleteAsync(pet);
            await _grpcPhotoService.Received().DeleteAsync(pet.MainPhotoId, BucketName.PetPhotos);
            await _eventProcessor.Received().ProcessAsync(pet.Events);
        }
        
        [Fact]
        public async Task given_valid_pet_with_multiple_photos_should_delete()
        {
            ShelterPet pet = Extensions.ArrangePet();
            DeleteShelterPet command = new DeleteShelterPet(pet.Id.Value);

            _petRepository.GetByIdAsync(command.Id).Returns(pet);

            await Act(command);

            await _petRepository.Received().DeleteAsync(pet);
            await _grpcPhotoService.Received().DeleteAsync(pet.MainPhotoId, BucketName.PetPhotos);
            foreach (Guid photoId in pet.PhotoIds)
            {
                await _grpcPhotoService.Received().DeleteAsync(photoId, BucketName.PetPhotos);
            }
            await _eventProcessor.Received().ProcessAsync(pet.Events);
        }
    }
}