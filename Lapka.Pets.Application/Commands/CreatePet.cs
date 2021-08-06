using System;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class CreatePet : ICommand
    {
        public Guid Id { get; }
        public string Name { get; }
        public Sex Sex { get; }
        public Species Species { get; }
        public string Race { get; }
        public DateTime BirthDay { get; }
        public string Color { get; }
        public double Weight { get; }
        public bool Sterilization { get; }
        public Address ShelterAddress { get; }
        public string Description { get; }

        public CreatePet(Guid id, string name, Sex sex, Species species, string race, DateTime birthDay, string color,
            double weight, bool sterilization, Address shelterAddress, string description)
        {
            Id = id;
            Name = name;
            Sex = sex;
            Species = species;
            Race = race;
            BirthDay = birthDay;
            Color = color;
            Weight = weight;
            Sterilization = sterilization;
            ShelterAddress = shelterAddress;
            Description = description;
        }
    }
}