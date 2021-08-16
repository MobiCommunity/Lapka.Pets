using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.ValueObjects;
using Shouldly;
using Xunit;
using File = Lapka.Pets.Core.ValueObjects.File;

namespace Lapka.Pets.Tests.Unit.Core.Entities.PetTests
{
    public class AddPetPhotoTests
    {
        [Fact]
        public void given_valid_pet_photo_should_be_added()
        {
            Pet pet = ArrangePet();
            string photoPath = $"{Guid.NewGuid():N}.jpg";
            
            pet.AddPhoto(photoPath);

            pet.ShouldNotBeNull();
            pet.Id.ShouldBe(pet.Id);
            pet.Name.ShouldBe(pet.Name);
            pet.Sex.ShouldBe(pet.Sex);
            pet.Race.ShouldBe(pet.Race);
            pet.Species.ShouldBe(pet.Species);
            pet.MainPhotoPath.ShouldBe(pet.MainPhotoPath);
            pet.PhotoPaths.Count().ShouldBe(1);
            pet.PhotoPaths.ShouldContain(photoPath);
            pet.BirthDay.ShouldBe(pet.BirthDay);
            pet.Color.ShouldBe(pet.Color);
            pet.Weight.ShouldBe(pet.Weight);
            pet.Sterilization.ShouldBe(pet.Sterilization);
            pet.ShelterAddress.ShouldBe(pet.ShelterAddress);
            pet.Description.ShouldBe(pet.Description);
            pet.Events.Count().ShouldBe(1);
            IDomainEvent @event = pet.Events.Single();
            @event.ShouldBeOfType<PetPhotoAdded>();
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

        private Location ArrangeShelterAddressLocation(string latitude = null, string longitude = null)
        {
            string shelterAddressLocationLatitude = latitude ?? "22";
            string shelterAddressLocationLongitude = longitude ?? "33";

            Location location = new Location(shelterAddressLocationLatitude, shelterAddressLocationLongitude);

            return location;
        }
    }
}