using System;
using System.ComponentModel.DataAnnotations;

namespace Lapka.Pets.Application.Dto.Pets
{
    public class PetDetailsShelterDto : PetDetailsDto
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public Guid ShelterId { get; set; }
        [Required]
        public AddressDto ShelterAddress { get; set; }
        [Required] 
        public double? Distance { get; set; }
    }
}