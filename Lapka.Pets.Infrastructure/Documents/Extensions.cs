using System;
using GeoCoordinatePortable;
using Lapka.Identity.Application.Dto;
using Lapka.Pets.Application.Dto;
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
                MainPhotoPath = pet.MainPhotoPath,
                Description = pet.Description,
                ShelterAddress = pet.ShelterAddress.AsDocument(),
                Sterilization = pet.Sterilization,
                Weight = pet.Weight
            };
        }
        public static Pet AsBusiness(this PetDocument pet)
        {
            return new Pet(pet.Id, pet.Name, pet.Sex, pet.Race, pet.Species, pet.MainPhotoPath, pet.BirthDay, pet.Color,
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
        
        public static AddressDto AsDto(this AddressDocument address)
        {
            return new AddressDto
            {
                Name = address.Name,
                Street = address.Street,
                City = address.City,
                GeoLocation = address.GeoLocation.AsDto()
            };
        }
        
        public static LocationDto AsDto(this LocationDocument location)
        {
            return new LocationDto
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
        }
        
        public static PetBasicDto AsBasicDto(this PetDocument pet, Location location = null)
        {
            double distance = -1;
            if (location != null)
            {
                GeoCoordinate pin1 = new GeoCoordinate(double.Parse(pet.ShelterAddress.GeoLocation.Latitude),
                    double.Parse(pet.ShelterAddress.GeoLocation.Longitude));
                GeoCoordinate pin2 = new GeoCoordinate(double.Parse(location.Latitude), double.Parse(location.Longitude));
                distance = pin1.GetDistanceTo(pin2);
            }
            
            return new PetBasicDto
            {
                Id = pet.Id,
                Name = pet.Name,
                Sex = pet.Sex,          
                MainPhotoPath = pet.MainPhotoPath,
                Race = pet.Race,
                BirthDay = pet.BirthDay,
                ShelterAddress = pet.ShelterAddress.AsDto(),
                Distance = distance
            };
        }
        
        public static PetDetailsDto AsDetailDto(this PetDocument pet, Location location = null)
        {
            double distance = -1;
            if (location != null)
            {
                GeoCoordinate pin1 = new GeoCoordinate(double.Parse(pet.ShelterAddress.GeoLocation.Latitude),
                    double.Parse(pet.ShelterAddress.GeoLocation.Longitude));
                GeoCoordinate pin2 = new GeoCoordinate(double.Parse(location.Latitude), double.Parse(location.Longitude));
                distance = pin1.GetDistanceTo(pin2);
            }

            return new PetDetailsDto
            {
                Id = pet.Id,
                Name = pet.Name,
                Sex = pet.Sex,
                Race = pet.Race,
                Color = pet.Color,
                BirthDay = pet.BirthDay,
                MainPhotoPath = pet.MainPhotoPath,
                Description = pet.Description,
                ShelterAddress = pet.ShelterAddress.AsDto(),
                Sterilization = pet.Sterilization,
                Weight = pet.Weight,
                Distance = distance
            };
        }
    }
}