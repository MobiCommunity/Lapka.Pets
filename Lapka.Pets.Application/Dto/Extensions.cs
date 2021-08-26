using System.Linq;
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
                MainPhotoPath = pet.MainPhotoPath,
                Race = pet.Race,
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
                Race = pet.Race,
                Color = pet.Color,
                BirthDay = pet.BirthDay,
                MainPhotoPath = pet.MainPhotoPath,
                Sterilization = pet.Sterilization,
                Weight = pet.Weight,
                PetEvents = pet.SoonEvents.Select(x => x.AsDto()),
                Visits = pet.LastVisits.Select(x => x.AsDto())
            };
        }

        public static VisitDto AsDto(this Visit visit)
        {
            return new VisitDto
            {
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
                DateOfEvent = petEvent.DateOfEvent,
                DescriptionOfEvent = petEvent.DescriptionOfEvent
            };
        }

        public static string GetFileExtension(this File file) =>
            file.Name.Contains('.') ? file.Name.Split('.')[1] : string.Empty;

    }
}