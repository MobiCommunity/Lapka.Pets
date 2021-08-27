using System;
using System.Linq;
using GeoCoordinatePortable;
using Lapka.Identity.Application.Dto;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Exceptions;
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
                Latitude = location.Latitude.AsDouble(),
                Longitude = location.Longitude.AsDouble()
            };
        }
        
        public static PetDocument AsDocument(this Pet pet)
        {
            return new PetUserDocument
            {
                Id = pet.Id.Value,
                Name = pet.Name,
                Sex = pet.Sex,
                Race = pet.Race,
                Color = pet.Color,
                BirthDay = pet.BirthDay,
                MainPhotoPath = pet.MainPhotoPath,
                Sterilization = pet.Sterilization,
                Weight = pet.Weight,
                Species = pet.Species,
            };
        }
        
        public static PetUserDocument AsDocument(this UserPet pet)
        {
            return new PetUserDocument
            {
                Id = pet.Id.Value,
                Name = pet.Name,
                Sex = pet.Sex,
                Race = pet.Race,
                Color = pet.Color,
                BirthDay = pet.BirthDay,
                MainPhotoPath = pet.MainPhotoPath,
                Sterilization = pet.Sterilization,
                Weight = pet.Weight,
                Species = pet.Species,
                SoonEvents = pet.SoonEvents.Select(x => x.AsDocument()),
                Visits = pet.LastVisits.Select(x => x.AsDocument())
            };
        }
        
        public static PetShelterDocument AsDocument(this ShelterPet pet)
        {
            return new PetShelterDocument
            {
                Id = pet.Id.Value,
                Name = pet.Name,
                Sex = pet.Sex,
                Race = pet.Race,
                Color = pet.Color,
                BirthDay = pet.BirthDay,
                MainPhotoPath = pet.MainPhotoPath,
                Sterilization = pet.Sterilization,
                Weight = pet.Weight,
                Species = pet.Species,
                ShelterAddress = pet.ShelterAddress.AsDocument()
            };
        }

        private static PetEventDocument AsDocument(this PetEvent petEvent)
        {
            return new PetEventDocument
            {
                DateOfEvent = petEvent.DateOfEvent,
                DescriptionOfEvent = petEvent.DescriptionOfEvent
            };
        }

        private static VisitDocument AsDocument(this Visit petEvent)
        {
            return new VisitDocument
            {
                Description = petEvent.Description,
                VisitDate = petEvent.VisitDate,
                IsVisitDone = petEvent.IsVisitDone,
                MedicalTreatments = petEvent.MedicalTreatments,
                LocationName = petEvent.LocationName,
                Weight = petEvent.Weight
            };
        }

        private static PetEvent AsBusiness(this PetEventDocument petEvent)
        {
            return new PetEvent(petEvent.DateOfEvent, petEvent.DescriptionOfEvent);
        }

        private static Visit AsBusiness(this VisitDocument petEvent)
        {
            return new Visit(petEvent.LocationName, petEvent.IsVisitDone, petEvent.VisitDate, petEvent.Description,
                petEvent.Weight, petEvent.MedicalTreatments);
        }

        public static UserPet AsBusiness(this PetUserDocument pet)
        {
            return new UserPet(pet.Id, pet.Name, pet.Sex, pet.Race, pet.Species, pet.MainPhotoPath, pet.BirthDay,
                pet.Color, pet.Weight, pet.Sterilization, pet.SoonEvents.Select(x => x.AsBusiness()).ToList(),
                pet.Visits.Select(x => x.AsBusiness()).ToList());
        }
        
        public static ShelterPet AsBusiness(this PetShelterDocument pet)
        {
            return new ShelterPet(pet.Id, pet.Name, pet.Sex, pet.Race, pet.Species, pet.MainPhotoPath, pet.BirthDay,
                pet.Color, pet.Weight, pet.Sterilization, pet.ShelterAddress.AsBusiness(), pet.Description);
        }

        private static Location AsBusiness(this LocationDocument location)
        {
            return new Location(location.Latitude.ToString(), location.Longitude.ToString());
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

        public static PetBasicShelterDto AsBasicDto(this PetShelterDocument pet, string latitude, string longitude)
        {
            double? distance = null;
            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                Location location = new Location(latitude, longitude);
                GeoCoordinate pin1 = new GeoCoordinate(pet.ShelterAddress.GeoLocation.Latitude,
                    pet.ShelterAddress.GeoLocation.Longitude);
                GeoCoordinate pin2 = new GeoCoordinate(location.Latitude.AsDouble(), location.Longitude.AsDouble());
                distance = pin1.GetDistanceTo(pin2);
            }

            return new PetBasicShelterDto
            {
                Id = pet.Id,
                Name = pet.Name,
                Sex = pet.Sex,
                MainPhotoPath = pet.MainPhotoPath,
                Race = pet.Race,
                BirthDay = pet.BirthDay,
                ShelterAddress = pet.ShelterAddress.AsDto(),
                Distance = distance,
            };
        }

        public static PetDetailsShelterDto AsDetailDto(this PetShelterDocument pet, string latitude, string longitude)
        {
            double? distance = null;
            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                Location location = new Location(latitude, longitude);
                GeoCoordinate pin1 = new GeoCoordinate(pet.ShelterAddress.GeoLocation.Latitude,
                    pet.ShelterAddress.GeoLocation.Longitude);
                GeoCoordinate pin2 = new GeoCoordinate(location.Latitude.AsDouble(), location.Longitude.AsDouble());
                distance = pin1.GetDistanceTo(pin2);
            }
        
            return new PetDetailsShelterDto
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
        
        public static UploadPhotoRequest.Types.Bucket AsGrpcUpload(this BucketName bucket)
        {
            return bucket switch
            {
                BucketName.PetPhotos => UploadPhotoRequest.Types.Bucket.PetPhotos,
                BucketName.ShelterPhotos => UploadPhotoRequest.Types.Bucket.ShelterPhotos,
                BucketName.UserPhotos => UploadPhotoRequest.Types.Bucket.UserPhotos,
                _ => throw new InvalidBucketNameException(bucket.ToString())
            };
        }
        
        public static DeletePhotoRequest.Types.Bucket AsGrpcDelete(this BucketName bucket)
        {
            return bucket switch
            {
                BucketName.PetPhotos => DeletePhotoRequest.Types.Bucket.PetPhotos,
                BucketName.ShelterPhotos => DeletePhotoRequest.Types.Bucket.ShelterPhotos,
                BucketName.UserPhotos => DeletePhotoRequest.Types.Bucket.UserPhotos,
                _ => throw new InvalidBucketNameException(bucket.ToString())
            };
        }
    }
}