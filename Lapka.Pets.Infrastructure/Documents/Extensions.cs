using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Infrastructure.Documents
{
    public static class Extensions
    {
        public static AddressDocument AsDocument(this Address address)
        {
            return new AddressDocument
            {
                Name = address.Name,
                Street = address.Street,
                City = address.City,
                GeoLocation = address.GeoLocation.AsDocument()
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
                Id = pet.Id.Value,
                Name = pet.Name,
                Sex = pet.Sex,
                Race = pet.Race,
                Color = pet.Color,
                BirthDay = pet.BirthDay,
                Description = pet.Description,
                ShelterAddress = pet.ShelterAddress.AsDocument(),
                Sterilization = pet.Sterilization,
                Species = pet.Species,
                Weight = pet.Weight
            };
        }
        public static Pet AsBusiness(this PetDocument pet)
        {
            return new Pet(pet.Id, pet.Name, pet.Sex, pet.Species, pet.Race, pet.BirthDay, pet.Color,
                pet.Weight, pet.Sterilization, pet.ShelterAddress.AsBusiness(), pet.Description);
        }
        public static Location AsBusiness(this LocationDocument location)
        {
            return new Location(location.Latitude, location.Longitude);
        }
        public static Address AsBusiness(this AddressDocument address)
        {
            return new Address(address.Name, address.City, address.Street, address.GeoLocation.AsBusiness());
        }
    }
}