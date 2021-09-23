using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Application.Commands.Handlers.ShelterPets;
using Lapka.Pets.Application.Commands.ShelterPets;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using NSubstitute;
using Xunit;

namespace Lapka.Pets.Tests.Unit.Application.Handlers.Shelters
{
    public class AddPetPhotoHandlerTests
    {
        private readonly AddShelterPetPhotoHandler _handler;
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;
        private readonly IShelterRepository _shelterRepository;

        public AddPetPhotoHandlerTests()
        {
            _eventProcessor = Substitute.For<IEventProcessor>();
            _repository = Substitute.For<IShelterPetRepository>();
            _photoService = Substitute.For<IGrpcPhotoService>();
            _shelterRepository = Substitute.For<IShelterRepository>();

            _handler = new AddShelterPetPhotoHandler(_eventProcessor, _repository, _photoService, _shelterRepository);
        }

        private Task Act(AddShelterPetPhoto command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_pet_path_should_add()
        {
            Guid userId = Guid.NewGuid();
            ShelterPet aggregatePet = Extensions.ArrangePet(userId: userId);
            Shelter shelter = Extensions.ArrangeShelter(owners: new List<Guid>
            {
                userId
            });
            
            IList<File> files = new List<File>();
            files.Add(Extensions.ArrangePhotoFile());
            files.Add(Extensions.ArrangePhotoFile());
            

            AddShelterPetPhoto command = new AddShelterPetPhoto(aggregatePet.Id.Value, userId, files);

            _repository.GetByIdAsync(command.PetId).Returns(aggregatePet);
            _shelterRepository.GetAsync(aggregatePet.ShelterId).Returns(shelter);

            await Act(command);

            foreach (File photo in files)
            {
                await _photoService.Received().AddAsync(photo.Name, userId, true, photo.Content, BucketName.PetPhotos);
            }
        }
    }
}