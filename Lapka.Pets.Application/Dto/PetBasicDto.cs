using System;
using System.ComponentModel.DataAnnotations;
using Lapka.Identity.Application.Dto;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Dto
{
    public abstract class PetBasicDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Sex Sex { get; set; }
        [Required]
        public string MainPhotoPath { get; set; }
        [Required]
        public string Race { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }
    }
}