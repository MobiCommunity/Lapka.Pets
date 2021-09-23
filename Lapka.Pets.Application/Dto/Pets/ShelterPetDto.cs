using System.ComponentModel.DataAnnotations;

namespace Lapka.Pets.Application.Dto.Pets
{
    public class ShelterPetDto : PetBasicDto
    {
        [Required]
        public AddressDto ShelterAddress { get; set; }
        [Required] 
        public double? Distance { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public bool Sterilization { get; set; }
    }
}