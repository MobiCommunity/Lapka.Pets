using System.ComponentModel.DataAnnotations;

namespace Lapka.Pets.Application.Dto
{
    public class AddressDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
    }
}