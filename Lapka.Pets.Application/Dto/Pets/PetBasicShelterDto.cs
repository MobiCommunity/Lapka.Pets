using System.ComponentModel.DataAnnotations;

namespace Lapka.Pets.Application.Dto.Pets
{
    public class PetBasicShelterDto : PetBasicDto
    {
        [Required]
        public AddressDto ShelterAddress { get; set; }
        [Required] 
        public double? Distance { get; set; }
        [Required]
        public bool IsLiked { get; set; }
    }
}