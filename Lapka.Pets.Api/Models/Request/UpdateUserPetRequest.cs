using System;
using System.ComponentModel.DataAnnotations;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Lapka.Pets.Api.Models.Request
{
    public class UpdateUserPetRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Race { get; set; }
        [Required]
        public Species Species { get; set; }
        [Required]
        public Sex Sex { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public bool Sterilization { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public string Color { get; set;}
    }
}