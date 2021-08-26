using System;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public abstract class CreatePet : ICommand
    {
        public Guid Id { get; }
        public string Name { get; }
        public Sex Sex { get; }
        public string Race { get; }
        public Species Species { get; }
        public File Photo { get; }
        public DateTime BirthDay { get; }
        public string Color { get; }
        public double Weight { get; }
        public bool Sterilization { get; }

        public CreatePet(Guid id, string name, Sex sex, string race, Species species, File photo, DateTime birthDay, string color,
            double weight, bool sterilization)
        {
            Id = id;
            Name = name;
            Sex = sex;
            Race = race;
            Species = species;
            Photo = photo;
            BirthDay = birthDay;
            Color = color;
            Weight = weight;
            Sterilization = sterilization;
        }
    }
}