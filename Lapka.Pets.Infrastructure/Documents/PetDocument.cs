using System;
using Convey.Types;
using Lapka.Pets.Core.ValueObject;

namespace Lapka.Pets.Infrastructure.Documents
{
    public class PetDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public string Race { get; set; }
        public Sex Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Description { get; set; }
        public Location Geolocation { get; set; }
        public ShelterAddress? ShelterAddress { get; set; }
        public bool? Sterilization { get; set; }
        public double Weight { get; set; }
        public string Color { get; set; }
        public Species Species { get; }
    }
}