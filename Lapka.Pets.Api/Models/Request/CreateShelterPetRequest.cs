using System;
using Lapka.Identity.Api.Models;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Lapka.Pets.Api.Models.Request
{
    public class CreateShelterPetRequest
    {
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public string Race { get; set; }
        public Species Species { get; set; }
        public IFormFile File { get; set; }
        public DateTime BirthDay { get; set; }
        public string Color { get; set; }
        public double Weight { get; set; }
        public bool Sterilization { get; set; }
        public AddressModel ShelterAddress { get; set; }
        public string Description { get; set; }
    }
}