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
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly DeleteShelterPetPhotoHandler _handler;
        private readonly IPetRepository<ShelterPet> _petRepository;

        public DeletePetPhotoHandlerTests()
        {
            _petRepository = Substitute.For<IPetRepository<ShelterPet>>();
            _grpcPhotoService = Substitute.For<IGrpcPhotoService>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _handler = new DeleteShelterPetPhotoHandler(_eventProcessor, _petRepository, _grpcPhotoService);
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

            _petRepository.GetByIdAsync(command.PetId).Returns(pet);

            await Act(command);

            await _petRepository.Received().UpdateAsync(pet);
            await _grpcPhotoService.Received().DeleteAsync(photoPath, BucketName.PetPhotos);
            await _eventProcessor.Received().ProcessAsync(pet.Events);
        }
    }
}