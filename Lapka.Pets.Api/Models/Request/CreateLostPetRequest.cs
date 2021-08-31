using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lapka.Identity.Api.Models;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Lapka.Pets.Api.Models.Request
{
    public class CreateLostPetRequest
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
        public int Age { get; set; }
        [Required]
        public DateTime LostDate { get; set; }
        [Required]
        public bool Sterilization { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string OwnerName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public AddressModel LostAddress { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile MainPhoto { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}