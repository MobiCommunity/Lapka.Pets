using System.ComponentModel.DataAnnotations;
using Lapka.Identity.Application.Dto;

namespace Lapka.Pets.Application.Dto
{
    public class PetDetailsShelterDto : PetDetailsDto
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public AddressDto ShelterAddress { get; set; }
        [Required] 
        public double? Distance { get; set; }
    }
}