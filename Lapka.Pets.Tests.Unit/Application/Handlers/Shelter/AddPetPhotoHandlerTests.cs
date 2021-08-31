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
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly AddShelterPetPhotoHandler _handler;
        private readonly IPetRepository<ShelterPet> _petRepository;

        public AddPetPhotoHandlerTests()
        {
            _petRepository = Substitute.For<IPetRepository<ShelterPet>>();
            _grpcPhotoService = Substitute.For<IGrpcPhotoService>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _handler = new AddShelterPetPhotoHandler(_eventProcessor, _petRepository, _grpcPhotoService);
        }

        private Task Act(AddShelterPetPhoto command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_pet_path_should_delete()
        {
            ShelterPet aggregatePet = Extensions.ArrangePet();
            List<PhotoFile> files = new List<PhotoFile>();
            files.Add(Extensions.ArrangePhotoFile());

            AddShelterPetPhoto command = new AddShelterPetPhoto(aggregatePet.Id.Value, files);

            _petRepository.GetByIdAsync(command.PetId).Returns(aggregatePet);

            await Act(command);

            await _petRepository.Received().UpdateAsync(aggregatePet);

            foreach (PhotoFile file in command.Photos)
            {
                await _grpcPhotoService.Received().AddAsync(Arg.Is<Guid>(x => x == file.Id), file.Name, file.Content,
                    BucketName.PetPhotos);
            }

            await _eventProcessor.Received().ProcessAsync(aggregatePet.Events);
        }
    }
}