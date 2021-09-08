using System;
using System.Collections.Generic;
using System.Linq;
using Lapka.Pets.Api.Models.Request;
using Lapka.Pets.Application.Commands;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Lapka.Identity.Api.Models
{
    public static class Extensions
    {
        public static Address AsValueObject(this AddressModel address) => new Address(address.Name, address.City,
            address.Street, address.GeoLocation.AsValueObject());

        public static Location AsValueObject(this LocationModel location) =>
            new Location(location.Latitude, location.Longitude);

        public static Address AsValueObject(this UpdateAddressRequest address) => new Address(address.Name,
            address.City,
            address.Street, address.GeoLocation.AsValueObject());

        public static Location AsValueObject(this UpdateLocationRequest location) =>
            new Location(location.Latitude, location.Longitude);

        public static Visit AsValueObject(this AddVisitRequest visit, Guid id) =>
            new Visit(id, visit.LocationName, visit.IsVisitDone, visit.VisitDate, visit.Description, visit.Weight,
                visit.MedicalTreatments);
        
        public static Visit AsValueObject(this UpdateVisitRequest visit, Guid id) =>
            new Visit(id, visit.LocationName, visit.IsVisitDone, visit.VisitDate, visit.Description, visit.Weight,
                visit.MedicalTreatments);
        
        public static PetEvent AsValueObject(this AddSoonEventRequest soonEvent, Guid id) =>
            new PetEvent(id, soonEvent.DateOfEvent, soonEvent.DescriptionOfEvent);

        public static File AsFile(this IFormFile file) =>
            new File(file.FileName, file.OpenReadStream(), file.ContentType);

        public static PhotoFile AsPhotoFile(this IFormFile file, Guid id) =>
            new PhotoFile(id, file.FileName, file.OpenReadStream(), file.ContentType);
        
        public static List<PhotoFile> CreatePhotoFiles(this List<IFormFile> photos)
        {
            List<PhotoFile> photoFiles = new List<PhotoFile>();

            if (photos == null) return photoFiles;

            photoFiles.AddRange(photos.Select(photo => photo.AsPhotoFile(Guid.NewGuid())));

            return photoFiles;
        }
    }
}