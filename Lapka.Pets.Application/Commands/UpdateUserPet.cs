using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands
{
    public class UpdateUserPet : ICommand
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Race { get; }
        public Species Species { get; }
        public File Photo { get; }
        public Sex Sex { get; }
        public DateTime DateOfBirth { get; }
        public bool Sterilization { get; }
        public double Weight { get; }
        public string Color { get; }
        public IEnumerable<PetEvent> PetEvents { get; }
        public IEnumerable<Visit> Visits { get; }

        public UpdateUserPet(Guid id, string name, string race, Species species, File photo, Sex sex,
            DateTime dateOfBirth, bool sterilization, double weight, string color, IEnumerable<PetEvent> petEvents,
            IEnumerable<Visit> visits)
        {
            Id = id;
            Name = name;
            Race = race;
            Species = species;
            Photo = photo;
            Sex = sex;
            DateOfBirth = dateOfBirth;
            Sterilization = sterilization;
            Weight = weight;
            Color = color;
            PetEvents = petEvents;
            Visits = visits;
        }
    }
}