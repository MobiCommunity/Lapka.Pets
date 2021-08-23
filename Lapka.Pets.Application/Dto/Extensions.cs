using Lapka.Identity.Application.Dto;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Dto
{
    public static class Extensions
    {
        public static AddressDto AsDto(this Address address)
        {
            return new AddressDto
            {
                Street = address.Street,
                City = address.City,
                Name = address.Name,
                GeoLocation = address.GeoLocation.AsDto()
            };
        }
        
        public static LocationDto AsDto(this Location location)
        {
            return new LocationDto
            {
                Latitude = location.Latitude.AsDouble(),
                Longitude = location.Longitude.AsDouble()
            };
        }
        
        public static PetBasicDto AsBasicDto(this Pet pet)
        {
            return new PetBasicDto
            {
                Id = pet.Id.Value,
                Name = pet.Name,
                Sex = pet.Sex,
                Race = pet.Race,
                MainPhotoPath = pet.MainPhotoPath,
                BirthDay = pet.BirthDay,
                ShelterAddress = pet.ShelterAddress.AsDto()
            };
        }
        
        public static PetDetailsDto AsDetailsDto(this Pet pet)
        {
            return new PetDetailsDto
            {
                Id = pet.Id.Value,
                Name = pet.Name,
                Sex = pet.Sex,
                Race = pet.Race,
                Color = pet.Color,
                BirthDay = pet.BirthDay,
                Description = pet.Description,
                ShelterAddress = pet.ShelterAddress.AsDto(),
                MainPhotoPath = pet.MainPhotoPath,
                Sterilization = pet.Sterilization,
                Weight = pet.Weight
            };
        }

        public static string GetFileExtension(this File file) =>
            file.Name.Contains('.') ? file.Name.Split('.')[1] : string.Empty;

    }
}