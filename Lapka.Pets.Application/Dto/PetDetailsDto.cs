using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lapka.Identity.Application.Dto;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Dto
{
    public class PetDetailsDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Sex Sex { get; set; }
        [Required]
        public string Race { get; set; }
        [Required] 
        public string MainPhotoPath { get; set; }
        public List<string> PhotoPaths { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }
        [Required]
        public AddressDto ShelterAddress { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public bool Sterilization { get; set; }
        [Required]
        public string Description { get; set; }
    }
}