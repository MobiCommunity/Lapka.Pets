using System;
using System.ComponentModel.DataAnnotations;
using Lapka.Identity.Application.Dto;

namespace Lapka.Pets.Application.Dto.Pets
{
    public class PetDetailsShelterDto : PetDetailsDto
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public Guid Shelterid { get; set; }
        [Required]
        public AddressDto ShelterAddress { get; set; }
        [Required] 
        public double? Distance { get; set; }
    }
}