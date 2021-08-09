using System;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Api.Models.Request
{
    public class UpdatePetRequest
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Race { get; }
        public Sex Sex { get; }
        public DateTime DateOfBirth { get; }
        public string Description { get; }
        public Location Geolocation { get; }
        public Address ShelterAddress { get; }
        public bool Sterilization { get; }
        public double Weight { get; }
        public string Color { get; }
        public Species Species { get; }
    }
}