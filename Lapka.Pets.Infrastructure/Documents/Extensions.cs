using System;
using System.Linq;
using GeoCoordinatePortable;
using Lapka.Identity.Application.Dto;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Dto.PetDtos;
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

        public static PetUserDocument AsDocument(this UserPet pet)
        {
            return new PetUserDocument
            {
                Id = pet.Id.Value,
                UserId = pet.UserId,
                Name = pet.Name,
                Sex = pet.Sex,
                Race = pet.Race,
                Color = pet.Color,
                BirthDay = pet.BirthDay,
                MainPhotoId = pet.MainPhotoId,
                PhotoIds = pet.PhotoIds,
                Weight = pet.Weight,
                Species = pet.Species,
                SoonEvents = pet.SoonEvents.Select(x => x.AsDocument()),
                Visits = pet.LastVisits.Select(x => x.AsDocument())
            };
        }

        public static LostPetDocument AsDocument(this LostPet pet)
        {
            return new LostPetDocument
            {
                Id = pet.Id.Value,
                UserId = pet.UserId,
                Name = pet.Name,
                Sex = pet.Sex,
                Race = pet.Race,
                Color = pet.Color,
                BirthDay = pet.BirthDay,
                MainPhotoId = pet.MainPhotoId,
                PhotoIds = pet.PhotoIds,
                Weight = pet.Weight,
                Species = pet.Species,
                OwnerName = pet.OwnerName,
                PhoneNumber = pet.PhoneNumber,
                Description = pet.Description,
                LostDate = pet.LostDate,
                LostAddress = pet.LostAddress.AsDocument()
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
                MainPhotoId = pet.MainPhotoId,
                Sterilization = pet.Sterilization,
                Weight = pet.Weight,
                Species = pet.Species,
                ShelterAddress = pet.ShelterAddress.AsDocument(),
                PhotoIds = pet.PhotoIds,
                Description = pet.Description
            };
        }

        private static PetEventDocument AsDocument(this PetEvent petEvent)
        {
            return new PetEventDocument
            {
                Id = petEvent.Id,
                DateOfEvent = petEvent.DateOfEvent,
                DescriptionOfEvent = petEvent.DescriptionOfEvent
            };
        }

        private static VisitDocument AsDocument(this Visit visit)
        {
            return new VisitDocument
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

        public static PetDetailsLostDto AsDetailDto(this LostPetDocument pet, string latitude, string longitude)
        {
            double? distance = null;
            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                Location location = new Location(latitude, longitude);
                GeoCoordinate pin1 = new GeoCoordinate(pet.LostAddress.GeoLocation.Latitude,
                    pet.LostAddress.GeoLocation.Longitude);
                GeoCoordinate pin2 = new GeoCoordinate(location.Latitude.AsDouble(), location.Longitude.AsDouble());
                distance = pin1.GetDistanceTo(pin2);
            }

            return new PetDetailsLostDto
            {
                Id = pet.Id,
                Name = pet.Name,
                Sex = pet.Sex,
                Species = pet.Species,
                Race = pet.Race,
                Color = pet.Color,
                BirthDay = pet.BirthDay,
                MainPhotoId = pet.MainPhotoId,
                Description = pet.Description,
                Sterilization = pet.Sterilization,
                Weight = pet.Weight,
                Distance = distance,
                PhotoIds = pet.PhotoIds,
                LostDate = pet.LostDate,
                OwnerName = pet.OwnerName,
                PhoneNumber = pet.PhoneNumber,
                LostAddress = pet.LostAddress.AsDto(),
            };
        }

        public static PetBasicLostDto AsBasicDto(this LostPetDocument pet, string latitude, string longitude)
        {
            double? distance = null;
            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                Location location = new Location(latitude, longitude);
                GeoCoordinate pin1 = new GeoCoordinate(pet.LostAddress.GeoLocation.Latitude,
                    pet.LostAddress.GeoLocation.Longitude);
                GeoCoordinate pin2 = new GeoCoordinate(location.Latitude.AsDouble(), location.Longitude.AsDouble());
                distance = pin1.GetDistanceTo(pin2);
            }

            return new PetBasicLostDto
            {
                Id = pet.Id,
                Name = pet.Name,
                Sex = pet.Sex,
                Species = pet.Species,
                Race = pet.Race,
                BirthDay = pet.BirthDay,
                MainPhotoId = pet.MainPhotoId,
                Distance = distance
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
                Species = pet.Species,
                MainPhotoId = pet.MainPhotoId,
                Race = pet.Race,
                BirthDay = pet.BirthDay,
                ShelterAddress = pet.ShelterAddress.AsDto(),
                Distance = distance
            };
        }

        public static AddressDto AsDto(this AddressDocument address)
        {
            return new AddressDto
            {
                Street = address.Street,
                City = address.City,
                Name = address.Name,
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

        public static PetBasicUserDto AsBasicDto(this PetUserDocument pet)
        {
            return new PetBasicUserDto
            {
                Id = pet.Id,
                Name = pet.Name,
                Sex = pet.Sex,
                Species = pet.Species,
                Race = pet.Race,
                MainPhotoId = pet.MainPhotoId,
                BirthDay = pet.BirthDay
            };
        }

        public static PetDetailsUserDto AsDetailsDto(this PetUserDocument pet)
        {
            return new PetDetailsUserDto
            {
                Id = pet.Id,
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
                Visits = pet.Visits.Select(x => x.AsDto()),
                PhotoIds = pet.PhotoIds
            };
        }

        public static VisitDto AsDto(this VisitDocument visit)
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

        public static PetEventDto AsDto(this PetEventDocument petEvent)
        {
            return new PetEventDto
            {
                Id = petEvent.Id,
                DateOfEvent = petEvent.DateOfEvent,
                DescriptionOfEvent = petEvent.DescriptionOfEvent
            };
        }

        private static PetEvent AsBusiness(this PetEventDocument petEvent)
        {
            return new PetEvent(petEvent.Id, petEvent.DateOfEvent, petEvent.DescriptionOfEvent);
        }

        private static Visit AsBusiness(this VisitDocument visit)
        {
            return new Visit(visit.Id, visit.LocationName, visit.IsVisitDone, visit.VisitDate, visit.Description,
                visit.Weight, visit.MedicalTreatments);
        }

        public static UserPet AsBusiness(this PetUserDocument pet)
        {
            return new UserPet(pet.Id, pet.UserId, pet.Name, pet.Sex, pet.Race, pet.Species, pet.MainPhotoId,
                pet.BirthDay,
                pet.Color, pet.Weight, pet.Sterilization, pet.SoonEvents.Select(x => x.AsBusiness()).ToList(),
                pet.Visits.Select(x => x.AsBusiness()).ToList(), pet.PhotoIds);
        }

        public static LostPet AsBusiness(this LostPetDocument pet)
        {
            return new LostPet(pet.Id, pet.UserId, pet.Name, pet.Sex, pet.Race, pet.Species, pet.MainPhotoId,
                pet.PhotoIds, pet.BirthDay, pet.Color, pet.Weight, pet.OwnerName, pet.PhoneNumber, pet.LostDate,
                pet.LostAddress.AsBusiness(), pet.Description);
        }

        public static ShelterPet AsBusiness(this PetShelterDocument pet)
        {
            return new ShelterPet(pet.Id, pet.Name, pet.Sex, pet.Race, pet.Species, pet.MainPhotoId, pet.BirthDay,
                pet.Color, pet.Weight, pet.Sterilization, pet.ShelterAddress.AsBusiness(), pet.Description,
                pet.PhotoIds);
        }

        private static Location AsBusiness(this LocationDocument location)
        {
            return new Location(location.Latitude.ToString(), location.Longitude.ToString());
        }

        public static Address AsBusiness(this AddressDocument address)
        {
            return new Address(address.Name, address.City, address.Street, address.GeoLocation.AsBusiness());
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