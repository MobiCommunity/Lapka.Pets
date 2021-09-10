using System;
using System.Linq;
using GeoCoordinatePortable;
using Lapka.Identity.Application.Dto;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Dto.Pets;
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

        public static UserPetDocument AsDocument(this UserPet pet)
        {
            return new UserPetDocument
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

        public static ShelterPetDocument AsDocument(this ShelterPet pet)
        {
            return new ShelterPetDocument
            {
                Id = pet.Id.Value,
                UserId = pet.UserId,
                Name = pet.Name,
                Sex = pet.Sex,
                Race = pet.Race,
                Color = pet.Color,
                BirthDay = pet.BirthDay,
                MainPhotoId = pet.MainPhotoId,
                Sterilization = pet.Sterilization,
                Weight = pet.Weight,
                Species = pet.Species,
                ShelterId = pet.ShelterId,
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

        public static PetDetailsShelterDto AsDetailDto(this ShelterPetDocument shelterPet, string latitude,
            string longitude)
        {
            double? distance = null;
            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                Location location = new Location(latitude, longitude);
                GeoCoordinate pin1 = new GeoCoordinate(shelterPet.ShelterAddress.GeoLocation.Latitude,
                    shelterPet.ShelterAddress.GeoLocation.Longitude);
                GeoCoordinate pin2 = new GeoCoordinate(location.Latitude.AsDouble(), location.Longitude.AsDouble());
                distance = pin1.GetDistanceTo(pin2);
            }

            return new PetDetailsShelterDto
            {
                Id = shelterPet.Id,
                UserId = shelterPet.UserId,
                Name = shelterPet.Name,
                Sex = shelterPet.Sex,
                Species = shelterPet.Species,
                Race = shelterPet.Race,
                Color = shelterPet.Color,
                BirthDay = shelterPet.BirthDay,
                MainPhotoId = shelterPet.MainPhotoId,
                Description = shelterPet.Description,
                ShelterId = shelterPet.ShelterId,
                ShelterAddress = shelterPet.ShelterAddress.AsDto(),
                Sterilization = shelterPet.Sterilization,
                Weight = shelterPet.Weight,
                Distance = distance,
                PhotoIds = shelterPet.PhotoIds
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
                UserId = pet.UserId,
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

        public static PetBasicShelterDto AsBasicDto(this ShelterPetDocument shelterPet, string latitude,
            string longitude)
        {
            double? distance = null;
            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                Location location = new Location(latitude, longitude);
                GeoCoordinate pin1 = new GeoCoordinate(shelterPet.ShelterAddress.GeoLocation.Latitude,
                    shelterPet.ShelterAddress.GeoLocation.Longitude);
                GeoCoordinate pin2 = new GeoCoordinate(location.Latitude.AsDouble(), location.Longitude.AsDouble());
                distance = pin1.GetDistanceTo(pin2);
            }

            return new PetBasicShelterDto
            {
                Id = shelterPet.Id,
                Name = shelterPet.Name,
                Sex = shelterPet.Sex,
                Species = shelterPet.Species,
                MainPhotoId = shelterPet.MainPhotoId,
                Race = shelterPet.Race,
                BirthDay = shelterPet.BirthDay,
                ShelterAddress = shelterPet.ShelterAddress.AsDto(),
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

        public static PetBasicUserDto AsBasicDto(this UserPetDocument pet)
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

        public static PetDetailsUserDto AsDetailsDto(this UserPetDocument pet)
        {
            return new PetDetailsUserDto
            {
                Id = pet.Id,
                UserId = pet.UserId,
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

        public static UserPet AsBusiness(this UserPetDocument pet)
        {
            return new UserPet(pet.Id, pet.UserId, pet.Name, pet.Sex, pet.Race, pet.Species, pet.MainPhotoId,
                pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization,
                pet.SoonEvents.Select(x => x.AsBusiness()).ToList(),
                pet.Visits.Select(x => x.AsBusiness()).ToList(), pet.PhotoIds);
        }

        public static LostPet AsBusiness(this LostPetDocument pet)
        {
            return new LostPet(pet.Id, pet.UserId, pet.Name, pet.Sex, pet.Race, pet.Species, pet.MainPhotoId,
                pet.PhotoIds, pet.BirthDay, pet.Color, pet.Weight, pet.OwnerName, pet.PhoneNumber, pet.LostDate,
                pet.LostAddress.AsBusiness(), pet.Description);
        }

        public static ShelterPet AsBusiness(this ShelterPetDocument shelterPet)
        {
            return new ShelterPet(shelterPet.Id, shelterPet.UserId, shelterPet.Name, shelterPet.Sex, shelterPet.Race,
                shelterPet.Species, shelterPet.MainPhotoId, shelterPet.BirthDay, shelterPet.Color, shelterPet.Weight,
                shelterPet.Sterilization, shelterPet.ShelterId, shelterPet.ShelterAddress.AsBusiness(),
                shelterPet.Description, shelterPet.PhotoIds);
        }

        public static UserLikedPets AsBusiness(this LikePetDocument likePets)
        {
            return new UserLikedPets(likePets.Id, likePets.LikedPets);
        }

        public static LikePetDocument AsDocument(this UserLikedPets likePets)
        {
            return new LikePetDocument(likePets.UserId, likePets.LikedPets);
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