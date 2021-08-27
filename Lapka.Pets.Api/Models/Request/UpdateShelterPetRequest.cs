using System;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Lapka.Pets.Api.Models.Request
{
    public class UpdateShelterPetRequest
    {
        public string Name { get; set; }
        public string Race { get; set; }
        public Species Species { get; set; }
        public IFormFile File { get; set; }
        public Sex Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Description { get; set; }
        public UpdateAddressRequest ShelterAddress { get; set; }
        public bool Sterilization { get; set; }
        public double Weight { get; set; }
        public string Color { get; set;}
    }
}