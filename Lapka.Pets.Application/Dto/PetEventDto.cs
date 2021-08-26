using System;
using System.ComponentModel.DataAnnotations;

namespace Lapka.Pets.Infrastructure.Documents
{
    public class PetEventDto
    {
        [Required]
        public DateTime DateOfEvent { get; set; }
        [Required]
        public string DescriptionOfEvent { get; set; }
    }
}