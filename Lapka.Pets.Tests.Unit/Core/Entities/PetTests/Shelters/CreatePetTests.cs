using System;
using System.Collections.Generic;
using System.Linq;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;
using Lapka.Pets.Core.Events.Concrete.Pets.Shelters;
using Lapka.Pets.Core.Exceptions;
using Lapka.Pets.Core.Exceptions.Location;
using Lapka.Pets.Core.Exceptions.Pet;
using Lapka.Pets.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Lapka.Pets.Tests.Unit.Core.Entities.PetTests.Shelters
{
    public class CreatePetTests
    {
        private ShelterPet Act(AggregateId id, Guid userId, string name, Sex sex, string race, Species species,
            string photoId, DateTime birthDay, string color, double weight, bool sterilization, Guid shelterId,
            string shelterName,
            Address shelterAddress, Location shelterGeoLocation, string description, IEnumerable<string> photoIds) =>
            ShelterPet.Create(id.Value, userId, name, sex, race, species, photoId, birthDay, color, weight,
                sterilization, shelterId, shelterName, shelterAddress, shelterGeoLocation, description, photoIds);

        [Fact]
        public void given_valid_pet_should_be_created()
        {
            Guid userId = Guid.NewGuid();
            ShelterPet arrangePet = Extensions.ArrangePet(userId: userId);

            ShelterPet aggregatePet = Act(arrangePet.Id, arrangePet.UserId, arrangePet.Name, arrangePet.Sex,
                arrangePet.Race, arrangePet.Species, arrangePet.MainPhotoPath, arrangePet.BirthDay, arrangePet.Color,
                arrangePet.Weight, arrangePet.Sterilization, arrangePet.ShelterId, arrangePet.ShelterName,
                arrangePet.ShelterAddress, arrangePet.ShelterGeoLocation, arrangePet.Description, null);

            aggregatePet.ShouldNotBeNull();
            aggregatePet.Id.ShouldBe(arrangePet.Id);
            aggregatePet.UserId.ShouldBe(arrangePet.UserId);
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
            @event.ShouldBeOfType<ShelterPetCreated>();
        }

        [Fact]
        public void given_invalid_pet_name_should_throw_an_exception()
        {
            ShelterPet pet = Extensions.ArrangePet(name: "");
            Guid userId = Guid.NewGuid();

            Exception exception = Record.Exception(() => Act(pet.Id, userId, pet.Name, pet.Sex, pet.Race,
                pet.Species, pet.MainPhotoPath, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization,
                pet.ShelterId, pet.ShelterName, pet.ShelterAddress, pet.ShelterGeoLocation, pet.Description, null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPetNameException>();
        }

        [Fact]
        public void given_invalid_pet_race_should_throw_an_exception()
        {
            ShelterPet pet = Extensions.ArrangePet(race: "");
            Guid userId = Guid.NewGuid();

            Exception exception = Record.Exception(() => Act(pet.Id, userId, pet.Name, pet.Sex, pet.Race,
                pet.Species, pet.MainPhotoPath, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization,
                pet.ShelterId, pet.ShelterName, pet.ShelterAddress, pet.ShelterGeoLocation, pet.Description, null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRaceValueException>();
        }

        [Fact]
        public void given_invalid_pet_birth_date_should_throw_an_exception()
        {
            ShelterPet pet = Extensions.ArrangePet(birthDay: DateTime.UtcNow.Add(TimeSpan.FromMinutes(1)));
            Guid userId = Guid.NewGuid();

            Exception exception = Record.Exception(() => Act(pet.Id, userId, pet.Name, pet.Sex, pet.Race, pet.Species,
                pet.MainPhotoPath, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterId, pet.ShelterName,
                pet.ShelterAddress, pet.ShelterGeoLocation, pet.Description, null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidBirthDayValueException>();
        }

        [Fact]
        public void given_invalid_pet_color_should_throw_an_exception()
        {
            ShelterPet pet = Extensions.ArrangePet(color: "");
            Guid userId = Guid.NewGuid();

            Exception exception = Record.Exception(() => Act(pet.Id, userId, pet.Name, pet.Sex, pet.Race, pet.Species,
                pet.MainPhotoPath, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterId, pet.ShelterName,
                pet.ShelterAddress, pet.ShelterGeoLocation, pet.Description, null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidColorValueException>();
        }

        [Fact]
        public void given_invalid_pet_weight_should_throw_an_exception()
        {
            ShelterPet pet = Extensions.ArrangePet(weight: 0);
            Guid userId = Guid.NewGuid();

            Exception exception = Record.Exception(() => Act(pet.Id, userId, pet.Name, pet.Sex, pet.Race,
                pet.Species, pet.MainPhotoPath, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization,
                pet.ShelterId, pet.ShelterName, pet.ShelterAddress, pet.ShelterGeoLocation, pet.Description, null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<WeightBelowMinimumValueException>();
        }

        [Fact]
        public void given_invalid_shelter_name_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                Extensions.ArrangePet(shelterAddress: Extensions.ArrangeAddress(name: "")));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidAddressNameException>();
        }

        [Fact]
        public void given_invalid_shelter_city_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                Extensions.ArrangePet(shelterAddress: Extensions.ArrangeAddress(city: "")));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidCityValueException>();
        }

        [Fact]
        public void given_invalid_shelter_street_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                Extensions.ArrangePet(shelterAddress: Extensions.ArrangeAddress(street: "")));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidStreetValueException>();
        }

        [Fact]
        public void given_too_big_shelter_address_location_latitude_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                Extensions.ArrangePet(shelterAddress: Extensions.ArrangeAddress(location:
                    Extensions.ArrangeLocation(latitude: "180"))));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LatitudeTooBigException>();
        }

        [Fact]
        public void given_too_low_shelter_address_location_latitude_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                Extensions.ArrangePet(shelterAddress: Extensions.ArrangeAddress(location:
                    Extensions.ArrangeLocation(latitude: "-90"))));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LatitudeTooLowException>();
        }

        [Fact]
        public void given_too_big_shelter_address_location_longitude_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                Extensions.ArrangePet(shelterAddress: Extensions.ArrangeAddress(location:
                    Extensions.ArrangeLocation(longitude: "180"))));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LongitudeTooBigException>();
        }

        [Fact]
        public void given_too_low_shelter_address_location_longitude_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangePet(shelterAddress:
                Extensions.ArrangeAddress(
                    location: Extensions.ArrangeLocation(longitude: "-180"))));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LongitudeTooLowException>();
        }

        [Fact]
        public void given_invalid_pet_description_should_throw_an_exception()
        {
            ShelterPet pet = Extensions.ArrangePet(description: "");
            Guid userId = Guid.NewGuid();

            Exception exception = Record.Exception(() => Act(pet.Id, userId, pet.Name, pet.Sex, pet.Race, pet.Species,
                pet.MainPhotoPath, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterId, pet.ShelterName,
                pet.ShelterAddress, pet.ShelterGeoLocation, pet.Description, null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidDescriptionValueException>();
        }
    }
}