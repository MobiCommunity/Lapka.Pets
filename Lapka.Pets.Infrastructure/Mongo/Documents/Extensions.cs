using System.Linq;
using GeoCoordinatePortable;
using Lapka.Pets.Application.Dto;
using Lapka.Pets.Application.Dto.Pets;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.Exceptions;
using Lapka.Pets.Core.ValueObjects;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static AddressDocument AsDocument(this Address address)
        {
            return new AddressDocument
            {
                Name = address.Name,
                Street = address.Street,
                City = address.City
            };
        }

        public static ShelterDocument AsDocument(this Shelter shelter)
        {
            return new ShelterDocument
            {
                Id = shelter.Id.Value,
                Name = shelter.Name,
                Address = shelter.Address.AsDocument(),
                GeoLocation = shelter.GeoLocation.AsDocument(),
                Owners = shelter.Owners,
                IsDeleted = shelter.IsDeleted
            };
        }

        public static Shelter AsBusiness(this ShelterDocument shelter)
        {
            return new Shelter(shelter.Id, shelter.Name, shelter.Address.AsBusiness(), shelter.GeoLocation.AsBusiness(),
                shelter.IsDeleted, shelter.Owners);
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
                MainPhotoPath = pet.MainPhotoPath,
                PhotoPaths = pet.PhotoPaths,
                Weight = pet.Weight,
                Species = pet.Species,
                SoonEvents = pet.SoonEvents.Select(x => x.AsDocument()),
                Visits = pet.LastVisits.Select(x => x.AsDocument()),
                IsDeleted = pet.IsDeleted
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
                MainPhotoPath = pet.MainPhotoPath,
                PhotoPaths = pet.PhotoPaths,
                Weight = pet.Weight,
                Species = pet.Species,
                OwnerName = pet.OwnerName,
                PhoneNumber = pet.PhoneNumber.Value,
                Description = pet.Description,
                LostDate = pet.LostDate,
                LostAddress = pet.LostAddress.AsDocument(),
                IsDeleted = pet.IsDeleted
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
                MainPhotoPath = pet.MainPhotoPath,
                Sterilization = pet.Sterilization,
                Weight = pet.Weight,
                Species = pet.Species,
                ShelterId = pet.ShelterId,
                ShelterAddress = pet.ShelterAddress.AsDocument(),
                ShelterGeoLocation = pet.ShelterGeoLocation.AsDocument(),
                PhotoPaths = pet.PhotoPaths,
                Description = pet.Description,
                IsDeleted = pet.IsDeleted
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
                GeoCoordinate pin1 = new GeoCoordinate(shelterPet.ShelterGeoLocation.Latitude,
                    shelterPet.ShelterGeoLocation.Longitude);
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
                MainPhotoPath = shelterPet.MainPhotoPath,
                Description = shelterPet.Description,
                ShelterId = shelterPet.ShelterId,
                ShelterAddress = shelterPet.ShelterAddress.AsDto(),
                Sterilization = shelterPet.Sterilization,
                Weight = shelterPet.Weight,
                Distance = distance,
                PhotoPaths = shelterPet.PhotoPaths
            };
        }

        public static PetDetailsLostDto AsDetailDto(this LostPetDocument pet, string latitude, string longitude)
        {
            double? distance = null;
            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                Location location = new Location(latitude, longitude);
                GeoCoordinate pin1 = new GeoCoordinate(pet.LostGeoLocation.Latitude,
                    pet.LostGeoLocation.Longitude);
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
                MainPhotoPath = pet.MainPhotoPath,
                Description = pet.Description,
                Sterilization = pet.Sterilization,
                Weight = pet.Weight,
                Distance = distance,
                PhotoPaths = pet.PhotoPaths,
                LostDate = pet.LostDate,
                OwnerName = pet.OwnerName,
                PhoneNumber = pet.PhoneNumber,
                LostAddress = pet.LostAddress.AsDto()
            };
        }

        public static PetBasicLostDto AsBasicDto(this LostPetDocument pet, string latitude, string longitude)
        {
            double? distance = null;
            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                Location location = new Location(latitude, longitude);
                GeoCoordinate pin1 = new GeoCoordinate(pet.LostGeoLocation.Latitude,
                    pet.LostGeoLocation.Longitude);
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
                MainPhotoPath = pet.MainPhotoPath,
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
                GeoCoordinate pin1 = new GeoCoordinate(shelterPet.ShelterGeoLocation.Latitude,
                    shelterPet.ShelterGeoLocation.Longitude);
                GeoCoordinate pin2 = new GeoCoordinate(location.Latitude.AsDouble(), location.Longitude.AsDouble());
                distance = pin1.GetDistanceTo(pin2);
            }

            return new PetBasicShelterDto
            {
                Id = shelterPet.Id,
                Name = shelterPet.Name,
                Sex = shelterPet.Sex,
                Species = shelterPet.Species,
                MainPhotoPath = shelterPet.MainPhotoPath,
                Race = shelterPet.Race,
                BirthDay = shelterPet.BirthDay,
                ShelterAddress = shelterPet.ShelterAddress.AsDto(),
                Distance = distance
            };
        }

        public static ShelterPetDto AsShelterPetDto(this ShelterPetDocument shelterPet)
        {
            return new ShelterPetDto
            {
                Id = shelterPet.Id,
                Name = shelterPet.Name,
                Sex = shelterPet.Sex,
                Species = shelterPet.Species,
                MainPhotoPath = shelterPet.MainPhotoPath,
                Race = shelterPet.Race,
                BirthDay = shelterPet.BirthDay,
                ShelterAddress = shelterPet.ShelterAddress.AsDto(),
                Color = shelterPet.Color,
                Sterilization = shelterPet.Sterilization
            };
        }

        public static AddressDto AsDto(this AddressDocument address)
        {
            return new AddressDto
            {
                Street = address.Street,
                City = address.City,
                Name = address.Name
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
                MainPhotoPath = pet.MainPhotoPath,
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
                MainPhotoPath = pet.MainPhotoPath,
                Sterilization = pet.Sterilization,
                Weight = pet.Weight,
                PetEvents = pet.SoonEvents.Select(x => x.AsDto()),
                Visits = pet.Visits.Select(x => x.AsDto()),
                PhotoPaths = pet.PhotoPaths
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
            return new UserPet(pet.Id, pet.UserId, pet.Name, pet.Sex, pet.Race, pet.Species, pet.MainPhotoPath,
                pet.BirthDay, pet.Color, pet.Weight, pet.Sterilization,
                pet.SoonEvents.Select(x => x.AsBusiness()).ToList(), pet.Visits.Select(x => x.AsBusiness()).ToList(),
                pet.IsDeleted, pet.PhotoPaths);
        }

        public static LostPet AsBusiness(this LostPetDocument pet)
        {
            return new LostPet(pet.Id, pet.UserId, pet.Name, pet.Sex, pet.Race, pet.Species, pet.MainPhotoPath,
                pet.BirthDay, pet.Color, pet.Weight, pet.OwnerName, new PhoneNumber(pet.PhoneNumber),
                pet.LostDate, pet.LostAddress.AsBusiness(), pet.Description, pet.IsDeleted, pet.PhotoPaths);
        }

        public static ShelterPet AsBusiness(this ShelterPetDocument shelterPet)
        {
            return new ShelterPet(shelterPet.Id, shelterPet.UserId, shelterPet.Name, shelterPet.Sex, shelterPet.Race,
                shelterPet.Species, shelterPet.MainPhotoPath, shelterPet.BirthDay, shelterPet.Color, shelterPet.Weight,
                shelterPet.Sterilization, shelterPet.ShelterId, shelterPet.ShelterName,
                shelterPet.ShelterAddress.AsBusiness(), shelterPet.ShelterGeoLocation?.AsBusiness(),
                shelterPet.Description, shelterPet.IsDeleted, shelterPet.PhotoPaths);
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
            return new Address(address.Name, address.City, address.Street);
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