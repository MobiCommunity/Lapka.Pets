using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Application.Dto
{
    public class AddressDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public LocationDto GeoLocation { get; set; }
    }
}