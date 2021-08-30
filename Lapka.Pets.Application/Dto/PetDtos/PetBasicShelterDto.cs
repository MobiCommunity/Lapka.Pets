using System.ComponentModel.DataAnnotations;
using Lapka.Identity.Application.Dto;

namespace Lapka.Pets.Application.Dto
{
    public class PetBasicShelterDto : PetBasicDto
    {
        [Required]
        public AddressDto ShelterAddress { get; set; }
        [Required] 
        public double? Distance { get; set; }
    }
}