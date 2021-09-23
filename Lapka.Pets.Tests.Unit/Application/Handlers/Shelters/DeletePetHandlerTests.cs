using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Application.Commands.Handlers.ShelterPets;
using Lapka.Pets.Application.Commands.ShelterPets;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Lapka.Pets.Tests.Unit.Application.Handlers.Shelters
{
    public class DeletePetHandlerTests
    {
        private readonly DeleteShelterPetHandler _handler;
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IShelterRepository _shelterRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainToIntegrationEventMapper _mapper;

        public DeletePetHandlerTests()
        {          
            _messageBroker = Substitute.For<IMessageBroker>();
            _mapper = Substitute.For<IDomainToIntegrationEventMapper>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _repository = Substitute.For<IShelterPetRepository>();
            _shelterRepository = Substitute.For<IShelterRepository>();

            _handler = new DeleteShelterPetHandler(_eventProcessor, _repository, _shelterRepository, _messageBroker, _mapper);
        }

        private Task Act(DeleteShelterPet command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_valid_pet_should_delete()
        {
            Guid userId = Guid.NewGuid();
            ShelterPet pet = Extensions.ArrangePet(userId: userId);
            Shelter shelter = Extensions.ArrangeShelter(owners: new List<Guid>
            {
                userId
            });
            DeleteShelterPet command = new DeleteShelterPet(pet.Id.Value, userId);

            _repository.GetByIdAsync(command.Id).Returns(pet);
            _shelterRepository.GetAsync(pet.ShelterId).Returns(shelter);

            await Act(command);

            await _repository.Received().UpdateAsync(pet);
        }
        
        [Fact]
        public async Task given_valid_pet_with_multiple_photos_should_delete()
        {
            Guid userId = Guid.NewGuid();
            ShelterPet pet = Extensions.ArrangePet(userId: userId);       
            Shelter shelter = Extensions.ArrangeShelter(owners: new HashSet<Guid>
            {
                userId
            });
            DeleteShelterPet command = new DeleteShelterPet(pet.Id.Value, userId);

            _repository.GetByIdAsync(command.Id).Returns(pet);
            _shelterRepository.GetAsync(pet.ShelterId).Returns(shelter);

            await Act(command);

            await _repository.Received().UpdateAsync(pet);
        }
    }
}