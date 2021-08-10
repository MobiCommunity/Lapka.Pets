using System;
using Lapka.Identity.Api.Models;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Api.Models.Request
{
    public class CreatePetRequest
    {
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public string Race { get; set; }
        public Species Species { get; set; }
        public byte[] Photo { get; set; }
        public DateTime BirthDay { get; set; }
        public string Color { get; set; }
        public double Weight { get; set; }
        public bool Sterilization { get; set; }
        public AddressModel ShelterAddress { get; set; }
        public string Description { get; set; }
    }
}