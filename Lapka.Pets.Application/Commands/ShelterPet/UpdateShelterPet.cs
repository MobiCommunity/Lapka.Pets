using System;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class UpdateShelterPet : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Name { get; }
        public string Race { get; }
        public Species Species { get; }
        public Sex Sex { get; }
        public DateTime DateOfBirth { get; }
        public string Description { get; }
        public Address ShelterAddress { get; }
        public bool Sterilization { get; }
        public double Weight { get; }
        public string Color { get; }

        public UpdateShelterPet(Guid id, Guid userId, string name, string race, Species species, Sex sex, DateTime dateOfBirth,
            string description, Address shelterAddress, bool sterilization, double weight, string color)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Race = race;
            Species = species;
            Sex = sex;
            DateOfBirth = dateOfBirth;
            Description = description;
            ShelterAddress = shelterAddress;
            Sterilization = sterilization;
            Weight = weight;
            Color = color;
        }
    }
}