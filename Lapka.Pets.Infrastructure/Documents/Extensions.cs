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
            return new UserPet(pet.Id, pet.UserId, pet.Name, pet.Sex, pet.Race, pet.Species, pet.MainPhotoId, pet.BirthDay,
                pet.Color, pet.Weight, pet.Sterilization, pet.SoonEvents.Select(x => x.AsBusiness()).ToList(),
                pet.Visits.Select(x => x.AsBusiness()).ToList(), pet.PhotoIds);
        }
        
        public static ShelterPet AsBusiness(this PetShelterDocument pet)
        {
            return new ShelterPet(pet.Id, pet.Name, pet.Sex, pet.Race, pet.Species, pet.MainPhotoId, pet.BirthDay,
                pet.Color, pet.Weight, pet.Sterilization, pet.ShelterAddress.AsBusiness(), pet.Description, pet.PhotoIds);
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