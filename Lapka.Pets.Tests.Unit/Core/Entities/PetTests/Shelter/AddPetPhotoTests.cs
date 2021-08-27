using System;
using System.Collections.Generic;
using System.Linq;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Abstract;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Lapka.Pets.Tests.Unit.Core.Entities.PetTests.Shelter
{
    public class AddPetPhotoTests
    {
        [Fact]
        public void given_valid_pet_photos_should_be_added()
        {
            ShelterPet aggregatePet = ArrangePet();
            List<string> photoPaths = new List<string>
            {
                $"{Guid.NewGuid():N}.jpg",
                $"{Guid.NewGuid():N}.jpg"
            };
            
            aggregatePet.AddPhotos(photoPaths);

            aggregatePet.ShouldNotBeNull();
            aggregatePet.Id.ShouldBe(aggregatePet.Id);
            aggregatePet.Name.ShouldBe(aggregatePet.Name);
            aggregatePet.Sex.ShouldBe(aggregatePet.Sex);
            aggregatePet.Race.ShouldBe(aggregatePet.Race);
            aggregatePet.Species.ShouldBe(aggregatePet.Species);
            aggregatePet.MainPhotoPath.ShouldBe(aggregatePet.MainPhotoPath);
            aggregatePet.PhotoPaths.Count().ShouldBe(2);
            aggregatePet.BirthDay.ShouldBe(aggregatePet.BirthDay);
            aggregatePet.Color.ShouldBe(aggregatePet.Color);
            aggregatePet.Weight.ShouldBe(aggregatePet.Weight);
            aggregatePet.Sterilization.ShouldBe(aggregatePet.Sterilization);
            aggregatePet.ShelterAddress.ShouldBe(aggregatePet.ShelterAddress);
            aggregatePet.Description.ShouldBe(aggregatePet.Description);
            foreach (var path in photoPaths)
            {
                aggregatePet.PhotoPaths.ShouldContain(path);
            }
            aggregatePet.Events.Count().ShouldBe(1);
            IDomainEvent @event = aggregatePet.Events.Single();
            @event.ShouldBeOfType<PetPhotosAdded>();
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

        private Location ArrangeShelterAddressLocation(string latitude = null, string longitude = null)
        {
            string shelterAddressLocationLatitude = latitude ?? "22";
            string shelterAddressLocationLongitude = longitude ?? "33";

            Location location = new Location(shelterAddressLocationLatitude, shelterAddressLocationLongitude);

            return location;
        }
    }
}