using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Pets.Application.Commands.Handlers.ShelterPets;
using Lapka.Pets.Application.Commands.ShelterPets;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using NSubstitute;
using Xunit;

namespace Lapka.Pets.Tests.Unit.Application.Handlers.Shelters
{
    public class UpdateShelterPetHandlerTests
    {
        private readonly UpdateShelterPetHandler _handler;
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _repository;
        private readonly IShelterRepository _shelterRepository;

        public UpdateShelterPetHandlerTests()
        {
            _eventProcessor = Substitute.For<IEventProcessor>();
            _repository = Substitute.For<IShelterPetRepository>();
            _shelterRepository = Substitute.For<IShelterRepository>();

            _handler = new UpdateShelterPetHandler(_eventProcessor, _repository, _shelterRepository);
        }

        private Task Act(UpdateShelterPet command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_pet_should_update()
        {
            Guid userId = Guid.NewGuid();
            ShelterPet arrangePet = Extensions.ArrangePet(userId: userId);
            Shelter shelter = Extensions.ArrangeShelter(owners: new HashSet<Guid>
            {
                userId
            });
            ShelterPet pet = ShelterPet.Create(arrangePet.Id.Value, arrangePet.UserId, arrangePet.Name, arrangePet.Sex,
                arrangePet.Race, arrangePet.Species, arrangePet.MainPhotoPath, arrangePet.BirthDay, arrangePet.Color,
                arrangePet.Weight, arrangePet.Sterilization, arrangePet.ShelterId, arrangePet.ShelterName,
                arrangePet.ShelterAddress, arrangePet.ShelterGeoLocation, arrangePet.Description, arrangePet.PhotoPaths);

            UpdateShelterPet command = new UpdateShelterPet(arrangePet.Id.Value, userId, arrangePet.Name,
                arrangePet.Race, arrangePet.Species, arrangePet.Sex, arrangePet.BirthDay, arrangePet.Description,
                arrangePet.ShelterAddress, arrangePet.Sterilization, arrangePet.Weight, arrangePet.Color);

            _repository.GetByIdAsync(command.Id).Returns(pet);
            _shelterRepository.GetAsync(pet.ShelterId).Returns(shelter);

            await Act(command);

            await _repository.Received().UpdateAsync(pet);
        }
    }
}