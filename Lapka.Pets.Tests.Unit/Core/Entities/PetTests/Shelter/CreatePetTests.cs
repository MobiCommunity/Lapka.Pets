using System;
using System.Collections.Generic;
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
        private ShelterPet Act(AggregateId id, string name, Sex sex, string race, Species species, Guid photoId,
            DateTime birthDay, string color, double weight, bool sterilization, Address shelterAddress,
            string description, List<Guid> photoIds) => ShelterPet.Create(id.Value, name, sex, race, species, photoId,
            birthDay, color, weight, sterilization, shelterAddress, description, photoIds);

        [Fact]
        public void given_valid_pet_should_be_created()
        {
            ShelterPet arrangePet = Extensions.ArrangePet();

            ShelterPet aggregatePet = Act(arrangePet.Id, arrangePet.Name, arrangePet.Sex, arrangePet.Race,
                arrangePet.Species, arrangePet.MainPhotoId, arrangePet.BirthDay, arrangePet.Color, arrangePet.Weight,
                arrangePet.Sterilization, arrangePet.ShelterAddress, arrangePet.Description, null);

            aggregatePet.ShouldNotBeNull();
            aggregatePet.Id.ShouldBe(arrangePet.Id);
            aggregatePet.Name.ShouldBe(arrangePet.Name);
            aggregatePet.Sex.ShouldBe(arrangePet.Sex);
            aggregatePet.Race.ShouldBe(arrangePet.Race);
            aggregatePet.Species.ShouldBe(arrangePet.Species);
            aggregatePet.MainPhotoId.ShouldBe(arrangePet.MainPhotoId);
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
            ShelterPet pet = Extensions.ArrangePet(name: "");

            Exception exception = Record.Exception(() => Act(pet.Id, pet.Name, pet.Sex, pet.Race,
                pet.Species, pet.MainPhotoId, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization,
                pet.ShelterAddress, pet.Description, null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPetNameException>();
        }

        [Fact]
        public void given_invalid_pet_race_should_throw_an_exception()
        {
            ShelterPet pet = Extensions.ArrangePet(race: "");

            Exception exception = Record.Exception(() => Act(pet.Id, pet.Name, pet.Sex, pet.Race,
                pet.Species, pet.MainPhotoId, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization,
                pet.ShelterAddress, pet.Description, null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRaceValueException>();
        }

        [Fact]
        public void given_invalid_pet_birth_date_should_throw_an_exception()
        {
            ShelterPet pet = Extensions.ArrangePet(birthDay: DateTime.Now.Add(TimeSpan.FromMinutes(1)));

            Exception exception = Record.Exception(() => Act(pet.Id, pet.Name, pet.Sex, pet.Race, pet.Species,
                pet.MainPhotoId, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterAddress,
                pet.Description, null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidBirthDayValueException>();
        }

        [Fact]
        public void given_invalid_pet_color_should_throw_an_exception()
        {
            ShelterPet pet = Extensions.ArrangePet(color: "");

            Exception exception = Record.Exception(() => Act(pet.Id, pet.Name, pet.Sex, pet.Race, pet.Species,
                pet.MainPhotoId, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterAddress,
                pet.Description, null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidColorValueException>();
        }

        [Fact]
        public void given_invalid_pet_weight_should_throw_an_exception()
        {
            ShelterPet pet = Extensions.ArrangePet(weight: 0);

            Exception exception = Record.Exception(() => Act(pet.Id, pet.Name, pet.Sex, pet.Race,
                pet.Species, pet.MainPhotoId, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization,
                pet.ShelterAddress, pet.Description, null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<WeightBelowMinimumValueException>();
        }

        [Fact]
        public void given_invalid_shelter_name_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                Extensions.ArrangePet(shelterAddress: Extensions.ArrangeShelterAddress(name: "")));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidAddressNameException>();
        }

        [Fact]
        public void given_invalid_shelter_city_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                Extensions.ArrangePet(shelterAddress: Extensions.ArrangeShelterAddress(city: "")));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidCityValueException>();
        }

        [Fact]
        public void given_invalid_shelter_street_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                Extensions.ArrangePet(shelterAddress: Extensions.ArrangeShelterAddress(street: "")));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidStreetValueException>();
        }

        [Fact]
        public void given_too_big_shelter_address_location_latitude_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                Extensions.ArrangePet(shelterAddress: Extensions.ArrangeShelterAddress(location:
                    Extensions.ArrangeShelterAddressLocation(latitude: "180"))));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LatitudeTooBigException>();
        }

        [Fact]
        public void given_too_low_shelter_address_location_latitude_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                Extensions.ArrangePet(shelterAddress: Extensions.ArrangeShelterAddress(location:
                    Extensions.ArrangeShelterAddressLocation(latitude: "-90"))));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LatitudeTooLowException>();
        }

        [Fact]
        public void given_too_big_shelter_address_location_longitude_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() =>
                Extensions.ArrangePet(shelterAddress: Extensions.ArrangeShelterAddress(location:
                    Extensions.ArrangeShelterAddressLocation(longitude: "180"))));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LongitudeTooBigException>();
        }

        [Fact]
        public void given_too_low_shelter_address_location_longitude_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangePet(shelterAddress:
                Extensions.ArrangeShelterAddress(location: Extensions.ArrangeShelterAddressLocation(longitude: "-180"))));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LongitudeTooLowException>();
        }

        [Fact]
        public void given_invalid_pet_description_should_throw_an_exception()
        {
            ShelterPet pet = Extensions.ArrangePet(description: "");

            Exception exception = Record.Exception(() => Act(pet.Id, pet.Name, pet.Sex, pet.Race, pet.Species,
                pet.MainPhotoId, pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization, pet.ShelterAddress,
                pet.Description, null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidDescriptionValueException>();
        }
    }
}