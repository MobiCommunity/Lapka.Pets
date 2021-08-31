using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lapka.Identity.Application.Dto;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Dto
{
    public abstract class PetDetailsDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Sex Sex { get; set; }
        [Required]
        public Species Species { get; set; }
        [Required]
        public string Race { get; set; }
        [Required] 
        public Guid MainPhotoId { get; set; }
        public List<Guid> PhotoIds { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public bool Sterilization { get; set; }
    }
}