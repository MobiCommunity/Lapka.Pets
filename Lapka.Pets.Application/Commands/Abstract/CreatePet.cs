using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Abstract
{
    public abstract class CreatePet : ICommand
    {
        public Guid Id { get; }
        public string Name { get; }
        public Sex Sex { get; }
        public string Race { get; }
        public Species Species { get; }
        public PhotoFile MainPhoto { get; }
        public IEnumerable<PhotoFile> Photos { get; }
        public DateTime BirthDay { get; }
        public string Color { get; }
        public double Weight { get; }
        public bool Sterilization { get; }

        protected CreatePet(Guid id, string name, Sex sex, string race, Species species, PhotoFile photo,
            DateTime birthDay, string color, double weight, bool sterilization, IEnumerable<PhotoFile> photoPaths)
        {
            Id = id;
            Name = name;
            Sex = sex;
            Race = race;
            Species = species;
            MainPhoto = photo;
            BirthDay = birthDay;
            Color = color;
            Weight = weight;
            Sterilization = sterilization;
            Photos = photoPaths;
        }
    }
}