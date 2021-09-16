using System;
using System.Collections.Generic;
using System.IO;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using File = Lapka.Pets.Core.ValueObjects.File;

namespace Lapka.Pets.Tests.Unit
{
    public static class Extensions
    {
        public static ShelterPet ArrangePet(AggregateId id = null, Guid? userId = null, string name = null,
            Sex? sex = null, string race = null, Species? species = null, Guid? photoId = null,
            DateTime? birthDay = null, string color = null, double? weight = null, bool? sterilization = null,
            Guid? shelterId = null, Address shelterAddress = null, string description = null, List<Guid> photoIds = null)
        {
            AggregateId validId = id ?? new AggregateId();
            Guid validUserId = userId ?? Guid.NewGuid();
            string validName = name ?? "Miniok";
            Sex validSex = sex ?? Sex.Male;
            string validRace = race ?? "mops";
            Species validSpecies = species ?? Species.Dog;
            Guid validPhotoId = photoId ?? Guid.NewGuid();
            DateTime validBirthDate = birthDay ?? DateTime.UtcNow.Subtract(TimeSpan.FromDays(180));
            string validColor = color ?? "red";
            double validWeight = weight ?? 152;
            bool validSterilization = sterilization ?? true;
            string validDescription = description ?? "Dlugi opis nie do przeczytania.";
            Guid validShelterId = shelterId ?? Guid.NewGuid();
            Address validShelterAddress = shelterAddress ?? ArrangeShelterAddress();
            List<Guid> validPhotoIds = photoIds ?? new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            ShelterPet aggregatePet = new ShelterPet(validId.Value, validUserId, validName, validSex, validRace,
                validSpecies, validPhotoId, validBirthDate, validColor, validWeight, validSterilization,
                validShelterId, validShelterAddress, validDescription, validPhotoIds);

            return aggregatePet;
        }

        public static Address ArrangeShelterAddress(string name = null, string city = null, string street = null,
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

        public static Location ArrangeShelterAddressLocation(string latitude = null, string longitude = null)
        {
            string shelterAddressLocationLatitude = latitude ?? "22";
            string shelterAddressLocationLongitude = longitude ?? "33";

            Location location = new Location(shelterAddressLocationLatitude, shelterAddressLocationLongitude);

            return location;
        }

        public static PhotoFile ArrangePhotoFile(Guid? photoId = null, string name = null, Stream stream = null,
            string contentType = null)
        {
            Guid validPhotoId = photoId ?? Guid.NewGuid();
            string validName = name ?? $"{Guid.NewGuid()}.jpg";
            Stream validStream = stream ?? new MemoryStream();
            string validContentType = contentType ?? "image/jpg";

            PhotoFile file = new PhotoFile(validPhotoId, validName, validStream, validContentType);

            return file;
        }
    }
}