using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class UpdateUserPet : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; set; }
        public string Name { get; }
        public string Race { get; }
        public Species Species { get; }
        public Sex Sex { get; }
        public DateTime DateOfBirth { get; }
        public bool Sterilization { get; }
        public double Weight { get; }
        public string Color { get; }

        public UpdateUserPet(Guid id, Guid userId, string name, string race, Species species, Sex sex,
            DateTime dateOfBirth, bool sterilization, double weight, string color)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Race = race;
            Species = species;
            Sex = sex;
            DateOfBirth = dateOfBirth;
            Sterilization = sterilization;
            Weight = weight;
            Color = color;
        }
    }
}