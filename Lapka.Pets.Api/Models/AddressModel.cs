using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models
{
    public class AddressModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public LocationModel GeoLocation { get; set; }
    }
}