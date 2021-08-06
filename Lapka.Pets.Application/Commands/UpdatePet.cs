using System;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObject;

namespace Lapka.Pets.Application.Commands
{
    public class UpdatePet : ICommand
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Race { get; }
        public Sex Sex { get; }
        public DateTime DateOfBirth { get; }
        public string Description { get; }
        public Location Geolocation { get; }
        public ShelterAddress? ShelterAddress { get; }
        public bool? Sterilization { get; }
        public double Weight { get; }
        public string Color { get; }
        public Species Species { get; }

        public UpdatePet(Guid id, string name, string race, Sex sex, DateTime dateOfBirth, string description,
            Location geolocation, ShelterAddress shelterAddress, bool sterilization, double weight, string color,
            Species species)
        {
            Id = id;
            Name = name;
            Race = race;
            Sex = sex;
            DateOfBirth = dateOfBirth;
            Description = description;
            Geolocation = geolocation;
            ShelterAddress = shelterAddress;
            Sterilization = sterilization;
            Weight = weight;
            Color = color;
            Species = species;
        }
    }
}