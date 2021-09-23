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
    public class UpdateShelterPetPhotoHandlerTests
    {
        private readonly UpdateShelterPetPhotoHandler _handler;
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IGrpcPhotoService _photoService;
        private readonly IShelterRepository _shelterRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainToIntegrationEventMapper _mapper;

        public UpdateShelterPetPhotoHandlerTests()
        {
            _messageBroker = Substitute.For<IMessageBroker>();
            _mapper = Substitute.For<IDomainToIntegrationEventMapper>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _repository = Substitute.For<IShelterPetRepository>();
            _photoService = Substitute.For<IGrpcPhotoService>();
            _shelterRepository = Substitute.For<IShelterRepository>();

            _handler = new UpdateShelterPetPhotoHandler(_eventProcessor, _repository, _photoService, _shelterRepository,
                _messageBroker, _mapper);
        }

        private Task Act(UpdateShelterPetPhoto command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_pet_photo_should_update()
        {
            Guid userId = Guid.NewGuid();
            ShelterPet pet = Extensions.ArrangePet(userId: userId);
            File file = Extensions.ArrangePhotoFile();
            string oldPhotoPath = pet.MainPhotoPath;
            Shelter shelter = Extensions.ArrangeShelter(owners: new HashSet<Guid>
            {
                userId
            });
            UpdateShelterPetPhoto command = new UpdateShelterPetPhoto(pet.Id.Value, userId, file);

            _repository.GetByIdAsync(command.PetId).Returns(pet);
            _shelterRepository.GetAsync(pet.ShelterId).Returns(shelter);

            await Act(command);

            await _repository.Received().UpdateAsync(pet);
            await _photoService.Received()
                .AddAsync(file.Name, command.UserId, true, file.Content, BucketName.PetPhotos);
        }
    }
}