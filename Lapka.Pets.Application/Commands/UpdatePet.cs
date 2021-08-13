using System;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class UpdatePet : ICommand
    {
        public Guid Id { get; }
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


        public UpdatePet(Guid id, string name, string race, Species species, Sex sex, DateTime dateOfBirth,
            string description, Address shelterAddress, bool sterilization, double weight, string color)
        {
            Id = id;
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