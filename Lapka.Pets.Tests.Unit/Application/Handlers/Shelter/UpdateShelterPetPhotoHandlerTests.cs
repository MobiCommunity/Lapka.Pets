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
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly UpdateShelterPetPhotoHandler _handler;
        private readonly IPetRepository<ShelterPet> _petRepository;

        public UpdateShelterPetPhotoHandlerTests()
        {
            _petRepository = Substitute.For<IPetRepository<ShelterPet>>();
            _grpcPhotoService = Substitute.For<IGrpcPhotoService>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _handler = new UpdateShelterPetPhotoHandler(_eventProcessor, _petRepository, _grpcPhotoService);
        }

        private Task Act(UpdateShelterPetPhoto command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_pet_photo_should_update()
        {
            ShelterPet pet = Extensions.ArrangePet();
            PhotoFile file = Extensions.ArrangePhotoFile();
            Guid photoIdBeforeUpdated = pet.MainPhotoId;
            UpdateShelterPetPhoto command = new UpdateShelterPetPhoto(pet.Id.Value, file);

            _petRepository.GetByIdAsync(command.PetId).Returns(pet);

            await Act(command);

            await _petRepository.Received().UpdateAsync(pet);
            await _grpcPhotoService.Received().DeleteAsync(photoIdBeforeUpdated, BucketName.PetPhotos);
            await _grpcPhotoService.Received().AddAsync(Arg.Is(file.Id),Arg.Is(file.Name), Arg.Is(file.Content),
                Arg.Is(BucketName.PetPhotos));
            await _eventProcessor.Received().ProcessAsync(pet.Events);
        }
    }
}