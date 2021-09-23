using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    public class DeletePetPhotoHandlerTests
    {
        private readonly DeleteShelterPetPhotoHandler _handler;
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IShelterRepository _shelterRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainToIntegrationEventMapper _mapper;

        public DeletePetPhotoHandlerTests()
        {
            _messageBroker = Substitute.For<IMessageBroker>();
            _mapper = Substitute.For<IDomainToIntegrationEventMapper>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _repository = Substitute.For<IShelterPetRepository>();
            _shelterRepository = Substitute.For<IShelterRepository>();

            _handler = new DeleteShelterPetPhotoHandler(_repository, _eventProcessor, _shelterRepository, _messageBroker, _mapper);
        }

        private Task Act(DeleteShelterPetPhoto command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_pet_photo_path_should_delete()
        {
            Guid userId = Guid.NewGuid();
            ShelterPet pet = Extensions.ArrangePet(photoIds: new HashSet<string>
            {
                "sekretnezdjecie.png"
            }, userId: userId);
            Shelter shelter = Extensions.ArrangeShelter(owners: new HashSet<Guid>
            {
                userId
            });
            string photoPath = pet.PhotoPaths.First();
            DeleteShelterPetPhoto command = new DeleteShelterPetPhoto(pet.Id.Value, userId, new Collection<string>{photoPath});

            _repository.GetByIdAsync(command.PetId).Returns(pet);
            _shelterRepository.GetAsync(pet.ShelterId).Returns(shelter);

            await Act(command);
        }
    }
}