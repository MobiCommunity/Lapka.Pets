using System;
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
    public class UpdatePetHandlerTests
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly UpdatePetHandler _handler;
        private readonly IPetRepository _petRepository;

        public UpdatePetHandlerTests()
        {
            _petRepository = Substitute.For<IPetRepository>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _handler = new UpdatePetHandler(_eventProcessor, _petRepository);
        }

        private Task Act(UpdatePet command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_pet_should_update()
        {
            Pet arrangePet = ArrangePet();

            Pet pet = Pet.Create(arrangePet.Id.Value, arrangePet.Name, arrangePet.Sex, arrangePet.Race,
                arrangePet.Species, arrangePet.MainPhotoPath, arrangePet.BirthDay, arrangePet.Color,
                arrangePet.Weight, arrangePet.Sterilization, arrangePet.ShelterAddress, arrangePet.Description);

            UpdatePet command = new UpdatePet(arrangePet.Id.Value, arrangePet.Name, arrangePet.Race, arrangePet.Species,
                arrangePet.Sex, arrangePet.BirthDay, arrangePet.Description, arrangePet.ShelterAddress,
                arrangePet.Sterilization, arrangePet.Weight, arrangePet.Color);

            _petRepository.GetByIdAsync(command.Id).Returns(pet);

            await Act(command);

            await _petRepository.Received().UpdateAsync(pet);
            await _eventProcessor.Received().ProcessAsync(pet.Events);
        }

        private Pet ArrangePet(AggregateId id = null, string name = null, Sex? sex = null, string race = null,
            Species? species = null, string photoPath = null, DateTime? birthDay = null, string color = null,
            double? weight = null, bool? sterilization = null, Address shelterAddress = null, string description = null)
        {
            AggregateId validId = id ?? new AggregateId();
            string validName = name ?? "Miniok";
            Sex validSex = sex ?? Sex.Male;
            string validRace = race ?? "mops";
            Species validSpecies = species ?? Species.Dog;
            string validPhotoPath = photoPath ?? $"{Guid.NewGuid()}.jpg";
            DateTime validBirthDate = birthDay ?? DateTime.Now.Subtract(TimeSpan.FromDays(180));
            string validColor = color ?? "red";
            double validWeight = weight ?? 152;
            bool validSterilization = sterilization ?? true;
            string validDescription = description ?? "Dlugi opis nie do przeczytania.";
            Address validShelterAddress = shelterAddress ?? ArrangeShelterAddress();

            Pet pet = new Pet(validId.Value, validName, validSex, validRace, validSpecies, validPhotoPath,
                validBirthDate, validColor, validWeight, validSterilization, validShelterAddress, validDescription);

            return pet;
        }

        private Address ArrangeShelterAddress(string name = null, string city = null, string street = null,
            Location location = null)
        {
            string shelterAddressName = name ?? "Mokotovo";
            string shelterAddressCity = city ?? "Rzeszow";
            string shelterAddressStreet = street ?? "Wojskowa";
            Location validShelterLocation = location ?? ArrangeShelterAddressLocation();

            Address address = new Address(shelterAddressName, shelterAddressCity, shelterAddressStreet,
                validShelterLocation);

            return address;
        }

        private Location ArrangeShelterAddressLocation(string latitude = null, string longitude = null)
        {
            string shelterAddressLocationLatitude = latitude ?? "22";
            string shelterAddressLocationLongitude = longitude ?? "33";

            Location location = new Location(shelterAddressLocationLatitude, shelterAddressLocationLongitude);

            return location;
        }
    }
}