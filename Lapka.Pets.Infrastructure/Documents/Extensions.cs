using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.ValueObject;

namespace Lapka.Pets.Infrastructure.Documents
{
    public static class Extensions
    {
        public static ShelterAddressDocument AsDocument(this ShelterAddress shelterAddress)
        {
            return new ShelterAddressDocument
            {
                Name = shelterAddress.Name,
                City = shelterAddress.City,
                Street = shelterAddress.Street,
                GeoLocation = shelterAddress.GeoLocation
            };
        }

        public static LocationDocument AsDocument(this Location location)
        {
            return new LocationDocument
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
        }

        public static PetDocument AsDocument(this Pet pet)
        {
            return new PetDocument
            {
                Name = pet.Name,
                Color = pet.Color,
                DateOfBirth = pet.DateOfBirth,
                Description = pet.Description,
                Geolocation = pet.Geolocation,
                Race = pet.Race,
                Sex = pet.Sex,
                Sterilization = pet.Sterilization,
                ShelterAddress = pet.ShelterAddress,
                Weight = pet.Weight
            };
        }
    }
}