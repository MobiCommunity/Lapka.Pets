using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using File = Lapka.Pets.Core.ValueObjects.File;

namespace Lapka.Pets.Tests.Unit
{
    public static class Extensions
    {
        public static ShelterPet ArrangePet(AggregateId id = null, Guid? userId = null, string name = null,
            Sex? sex = null, string race = null, Species? species = null, string photoPath = null,
            DateTime? birthDay = null, string color = null, double? weight = null, bool? sterilization = null,
            Guid? shelterId = null, string shelterName = null, Address shelterAddress = null,
            Location shelterGeoLocation = null, string description = null, IEnumerable<string> photoIds = null)
        {
            AggregateId validId = id ?? new AggregateId();
            Guid validUserId = userId ?? Guid.NewGuid();
            string validName = name ?? "Miniok";
            Sex validSex = sex ?? Sex.Male;
            string validRace = race ?? "mops";
            Species validSpecies = species ?? Species.Dog;
            string validphotoPath = photoPath ?? "mojezdjecie.png";
            DateTime validBirthDate = birthDay ?? DateTime.UtcNow.Subtract(TimeSpan.FromDays(180));
            string validColor = color ?? "red";
            double validWeight = weight ?? 152;
            bool validSterilization = sterilization ?? true;
            string validDescription = description ?? "Dlugi opis nie do przeczytania.";
            Guid validShelterId = shelterId ?? Guid.NewGuid();
            string validShelterName = shelterName ?? "Best shelter name";
            Address validShelterAddress = shelterAddress ?? ArrangeAddress();
            Location validShelterLocation = shelterGeoLocation ?? ArrangeLocation();
            IEnumerable<string> validPhotoIds = photoIds ?? new HashSet<string>
            {
                "zdjecie1.png",
                "afkeofkaeo.jpg"
            };

            ShelterPet aggregatePet = new ShelterPet(validId.Value, validUserId, validName, validSex, validRace,
                validSpecies, validphotoPath, validBirthDate, validColor, validWeight, validSterilization,
                validShelterId, validShelterName, validShelterAddress, validShelterLocation, validDescription,
                false, validPhotoIds);

            return aggregatePet;
        }

        public static Address ArrangeAddress(string name = null, string city = null, string street = null,
            Location location = null)
        {
            string shelterAddressName = name ?? "Mokotovo";
            string shelterAddressCity = city ?? "Rzeszow";
            string shelterAddressStreet = street ?? "Wojskowa";
            Location validShelterLocation = location ?? ArrangeLocation();

            Address address = new Address(shelterAddressName, shelterAddressCity, shelterAddressStreet);

            return address;
        }

        public static Location ArrangeLocation(string latitude = null, string longitude = null)
        {
            string shelterAddressLocationLatitude = latitude ?? "22";
            string shelterAddressLocationLongitude = longitude ?? "33";

            Location location = new Location(shelterAddressLocationLatitude, shelterAddressLocationLongitude);

            return location;
        }

        public static File ArrangePhotoFile(string name = null, Stream stream = null, string contentType = null)
        {
            string validName = name ?? $"{Guid.NewGuid():N}.jpg";
            Stream validStream = stream ?? new MemoryStream();
            string validContentType = contentType ?? "image/jpg";

            File file = new File(validName, validStream, validContentType);

            return file;
        }

        public static Shelter ArrangeShelter(Guid? shelterId = null, string shelterName = null,
            Address shelterAddress = null,
            Location shelterGeoLocation = null, IEnumerable<Guid> owners = null)
        {
            Guid validShelterId = shelterId ?? Guid.NewGuid();
            string validShelterName = shelterName ?? "Best shelter name";
            Address validShelterAddress = shelterAddress ?? ArrangeAddress();
            Location validShelterLocation = shelterGeoLocation ?? ArrangeLocation();
            IEnumerable<Guid> validOwners = owners ?? Enumerable.Empty<Guid>();

            Shelter shelter = new Shelter(validShelterId, validShelterName, validShelterAddress, validShelterLocation,
                false, validOwners);

            return shelter;
        }
    }
}