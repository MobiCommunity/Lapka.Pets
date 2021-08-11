using System.ComponentModel.DataAnnotations;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Identity.Application.Dto
{
    public class AddressDto
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public LocationDto GeoLocation { get; set; }
    }
}