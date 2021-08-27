using System;
using System.Linq;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Exceptions;
using Lapka.Pets.Core.Exceptions.Location;
using Lapka.Pets.Core.Exceptions.Pet;
using Lapka.Pets.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Lapka.Pets.Tests.Unit.Core.Entities.PetTests
{
    public class CreatePetTests
    {
        private ShelterPet Act(AggregateId id, string name, Sex sex, string race, Species species, string photoPath,
            DateTime birthDay, string color, double weight, bool sterilization, Address shelterAddress,
            string description) => ShelterPet.Create(id.Value, name, sex, race, species, photoPath, birthDay, color, weight,
            sterilization, shelterAddress, description);

        [Fact]
        public void given_valid_pet_should_be_created()
        {
            ShelterPet arrangePet = ArrangePet();
            
            ShelterPet aggregatePet = Act(arrangePet.Id, arrangePet.Name, arrangePet.Sex, arrangePet.Race, arrangePet.Species, arrangePet.MainPhotoPath, arrangePet.BirthDay,
                arrangePet.Color, arrangePet.Weight, arrangePet.Sterilization, arrangePet.ShelterAddress, arrangePet.Description);
            
            aggregatePet.ShouldNotBeNull();
            aggregatePet.Id.ShouldBe(arrangePet.Id);
            aggregatePet.Name.ShouldBe(arrangePet.Name);
            aggregatePet.Sex.ShouldBe(arrangePet.Sex);
            aggregatePet.Race.ShouldBe(arrangePet.Race);
            aggregatePet.Species.ShouldBe(arrangePet.Species);
            aggregatePet.MainPhotoPath.ShouldBe(arrangePet.MainPhotoPath);
            aggregatePet.BirthDay.ShouldBe(arrangePet.BirthDay);
            aggregatePet.Color.ShouldBe(arrangePet.Color);
            aggregatePet.Weight.ShouldBe(arrangePet.Weight);
            aggregatePet.Sterilization.ShouldBe(arrangePet.Sterilization);
            aggregatePet.ShelterAddress.ShouldBe(arrangePet.ShelterAddress);
            aggregatePet.Description.ShouldBe(arrangePet.Description);
            aggregatePet.Events.Count().ShouldBe(1);
            IDomainEvent @event = aggregatePet.Events.Single();
            @event.ShouldBeOfType<PetCreated<ShelterPet>>();
        }
        
        [Fact]
        public void given_invalid_pet_name_should_throw_an_exception()
        {
            ShelterPet pet = ArrangePet(name: "");
            
            Exception exception = Record.Exception(() => Act(pet.Id, pet.Name, pet.Sex, pet.Race, 
                pet.Species, pet.MainPhotoPath, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization,
                pet.ShelterAddress, pet.Description));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPetNameException>();
        }
        
        [Fact]
        public void given_invalid_pet_race_should_throw_an_exception()
        {
            ShelterPet pet = ArrangePet(race: "");

            Exception exception = Record.Exception(() => Act(pet.Id, pet.Name, pet.Sex, pet.Race,
                pet.Species, pet.MainPhotoPath, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization,
                pet.ShelterAddress, pet.Description));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRaceValueException>();
        }
        
        [Fact]
        public void given_invalid_pet_birth_date_should_throw_an_exception()
        {
            ShelterPet pet = ArrangePet(birthDay: DateTime.Now.Add(TimeSpan.FromMinutes(1)));
            
            Exception exception = Record.Exception(() => Act(pet.Id, pet.Name, pet.Sex, pet.Race, pet.Species,
                pet.MainPhotoPath, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterAddress,
                pet.Description));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidBirthDayValueException>();
        }
        
        [Fact]
        public void given_invalid_pet_color_should_throw_an_exception()
        {
            ShelterPet pet = ArrangePet(color: "");
            
            Exception exception = Record.Exception(() => Act(pet.Id, pet.Name, pet.Sex, pet.Race, pet.Species,
                pet.MainPhotoPath, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterAddress,
                pet.Description));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidColorValueException>();
        }
        
        [Fact]
        public void given_invalid_pet_weight_should_throw_an_exception()
        {
            ShelterPet pet = ArrangePet(weight: 0);
            
            Exception exception = Record.Exception(() => Act(pet.Id, pet.Name, pet.Sex, pet.Race,
                pet.Species, pet.MainPhotoPath, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization,
                pet.ShelterAddress, pet.Description));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<WeightBelowMinimumValueException>();
        }
        
        [Fact]
        public void given_invalid_shelter_name_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => 
                ArrangePet(shelterAddress: ArrangeShelterAddress(name: "")));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidAddressNameException>();
        }
        
        [Fact]
        public void given_invalid_shelter_city_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                ArrangePet(shelterAddress: ArrangeShelterAddress(city: "")));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidCityValueException>();
        }
        
        [Fact]
        public void given_invalid_shelter_street_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                ArrangePet(shelterAddress: ArrangeShelterAddress(street: "")));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidStreetValueException>();
        }
        
        [Fact]
        public void given_too_big_shelter_address_location_latitude_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                ArrangePet(shelterAddress: ArrangeShelterAddress(location:
                    ArrangeShelterAddressLocation(latitude: "180"))));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LatitudeTooBigException>();
        }
        
        [Fact]
        public void given_too_low_shelter_address_location_latitude_should_throw_an_exception()
        {            
            Exception exception = Record.Exception(() =>
                ArrangePet(shelterAddress: ArrangeShelterAddress(location: 
                    ArrangeShelterAddressLocation(latitude: "-90"))));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LatitudeTooLowException>();
        }
        
        [Fact]
        public void given_too_big_shelter_address_location_longitude_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                ArrangePet(shelterAddress: ArrangeShelterAddress(location:
                    ArrangeShelterAddressLocation(longitude: "180"))));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LongitudeTooBigException>();
        }
        
        [Fact]
        public void given_too_low_shelter_address_location_longitude_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => ArrangePet(shelterAddress:
                ArrangeShelterAddress(location: ArrangeShelterAddressLocation(longitude: "-180"))));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LongitudeTooLowException>();
        }
        
        [Fact]
        public void given_invalid_pet_description_should_throw_an_exception()
        {
            ShelterPet pet = ArrangePet(description: "");
            
            Exception exception = Record.Exception(() => Act(pet.Id, pet.Name, pet.Sex, pet.Race, pet.Species,
                pet.MainPhotoPath, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterAddress,
                pet.Description));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidDescriptionValueException>();
        }

        private ShelterPet ArrangePet(AggregateId id = null, string name = null, Sex? sex = null, string race = null,
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

            ShelterPet aggregatePet = new ShelterPet(validId.Value, validName, validSex, validRace, validSpecies, validPhotoPath,
                 validBirthDate, validColor, validWeight, validSterilization, validShelterAddress, validDescription);

             return aggregatePet;
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
            string shelterAddressLocationLatitude= latitude ?? "22";
            string shelterAddressLocationLongitude = longitude ?? "33";
            
            Location location = new Location(shelterAddressLocationLatitude, shelterAddressLocationLongitude);

            return location;
        }
    }
}