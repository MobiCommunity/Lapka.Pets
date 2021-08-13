using Lapka.Pets.Api.Models.Request;
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
        
        public static Address AsValueObject(this UpdateAddressRequest address) => new Address(address.Name, address.City,
            address.Street, address.GeoLocation.AsValueObject());
        
        public static Location AsValueObject(this UpdateLocationRequest location) =>
            new Location(location.Latitude, location.Longitude);
        
        public static File AsValueObject(this IFormFile file) =>
            new File(file.FileName, file.OpenReadStream(), file.ContentType);
    }
}