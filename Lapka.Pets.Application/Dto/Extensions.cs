using System;
using System.Collections.Generic;
using System.Linq;
using GeoCoordinatePortable;
using Lapka.Identity.Application.Dto;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Lapka.Pets.Infrastructure.Documents;

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
        
        public static PetBasicUserDto AsBasicDto(this UserPet pet)
        {
            return new PetBasicUserDto
            {
                Id = pet.Id.Value,
                Name = pet.Name,
                Sex = pet.Sex,
                Species = pet.Species,
                Race = pet.Race,
                MainPhotoId = pet.MainPhotoId,
                BirthDay = pet.BirthDay
            };
        }
        
        public static PetDetailsUserDto AsDetailsDto(this UserPet pet)
        {
            return new PetDetailsUserDto
            {
                Id = pet.Id.Value,
                Name = pet.Name,
                Sex = pet.Sex,
                Species = pet.Species,
                Race = pet.Race,
                Color = pet.Color,
                BirthDay = pet.BirthDay,
                MainPhotoId = pet.MainPhotoId,
                Sterilization = pet.Sterilization,
                Weight = pet.Weight,
                PetEvents = pet.SoonEvents.Select(x => x.AsDto()),
                Visits = pet.LastVisits.Select(x => x.AsDto()),
                PhotoIds = pet.PhotoIds
            };
        }

        public static List<Guid> IdsAsGuidList(this List<PhotoFile> photos)
        {
            List<Guid> guids = new List<Guid>();
            photos.ForEach((p) => guids.Add(p.Id));
            return guids;
        }
        
        public static List<Guid> IdsAsGuidList(this IEnumerable<PhotoFile> photos)
        {
            List<Guid> guids = new List<Guid>();
            photos.ToList().ForEach((p) => guids.Add(p.Id));
            return guids;
        }

        public static VisitDto AsDto(this Visit visit)
        {
            return new VisitDto
            {
                Id = visit.Id,
                Description = visit.Description,
                VisitDate = visit.VisitDate,
                IsVisitDone = visit.IsVisitDone,
                MedicalTreatments = visit.MedicalTreatments,
                LocationName = visit.LocationName,
                Weight = visit.Weight
            };
        }
        
        public static PetEventDto AsDto(this PetEvent petEvent)
        {
            return new PetEventDto
            {
                Id = petEvent.Id,
                DateOfEvent = petEvent.DateOfEvent,
                DescriptionOfEvent = petEvent.DescriptionOfEvent
            };
        }
        
        public static PetDetailsShelterDto AsDetailDto(this ShelterPet pet, string latitude, string longitude)
        {
            double? distance = null;
            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                Location location = new Location(latitude, longitude);
                GeoCoordinate pin1 = new GeoCoordinate(pet.ShelterAddress.GeoLocation.Latitude.AsDouble(),
                    pet.ShelterAddress.GeoLocation.Longitude.AsDouble());
                GeoCoordinate pin2 = new GeoCoordinate(location.Latitude.AsDouble(), location.Longitude.AsDouble());
                distance = pin1.GetDistanceTo(pin2);
            }
        
            return new PetDetailsShelterDto
            {
                Id = pet.Id.Value,
                Name = pet.Name,
                Sex = pet.Sex,
                Species = pet.Species,
                Race = pet.Race,
                Color = pet.Color,
                BirthDay = pet.BirthDay,
                MainPhotoId = pet.MainPhotoId,
                Description = pet.Description,
                ShelterAddress = pet.ShelterAddress.AsDto(),
                Sterilization = pet.Sterilization,
                Weight = pet.Weight,
                Distance = distance,
                PhotoIds = pet.PhotoIds
            };
        }
        
        public static PetBasicShelterDto AsBasicDto(this ShelterPet pet, string latitude, string longitude)
        {
            double? distance = null;
            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                Location location = new Location(latitude, longitude);
                GeoCoordinate pin1 = new GeoCoordinate(pet.ShelterAddress.GeoLocation.Latitude.AsDouble(),
                    pet.ShelterAddress.GeoLocation.Longitude.AsDouble());
                GeoCoordinate pin2 = new GeoCoordinate(location.Latitude.AsDouble(), location.Longitude.AsDouble());
                distance = pin1.GetDistanceTo(pin2);
            }

            return new PetBasicShelterDto
            {
                Id = pet.Id.Value,
                Name = pet.Name,
                Sex = pet.Sex,
                Species = pet.Species,
                MainPhotoId = pet.MainPhotoId,
                Race = pet.Race,
                BirthDay = pet.BirthDay,
                ShelterAddress = pet.ShelterAddress.AsDto(),
                Distance = distance
            };
        }

        public static string GetFileExtension(this File file) =>
            file.Name.Contains('.') ? file.Name.Split('.')[1] : string.Empty;

    }
}