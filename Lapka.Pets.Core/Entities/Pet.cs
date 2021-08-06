using System;
using System.Collections.Generic;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Exceptions.Pet;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Entities
{
    public class Pet : AggregateRoot
    {
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


        public Pet(Guid id, string name, Sex sex, Species species, string race, DateTime birthDay, string color, double weight, bool sterilization, Address shelterAddress, string description)
        {
            Id = new AggregateId(id);
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
        
        public static Pet Create(Guid id, string name, Sex sex, Species species, string race, DateTime birthDay, string color, double weight, bool sterilization, Address shelterAddress, string description)
        {
            if (IsNameValid(name))
                throw new InvalidPetNameException(name);
            
            if (IsRaceValid(race))
                throw new InvalidRaceValueException(race);
            
            if (IsBirthDayValid(birthDay))
                throw new InvalidBirtDayValueException(birthDay);
            
            if (IsColorValid(color))
                throw new InvalidColorValueException(color);

            if (IsDescriptionValid(description))
                throw new InvalidDescriptionValueException(description);
                
            Pet pet = new Pet(id, name, sex, species, race, birthDay, color, weight, sterilization, shelterAddress, description);
            
            pet.AddEvent(new PetCreated(pet));
            return pet;
        }

        private static bool IsDescriptionValid(string description) => string.IsNullOrEmpty(description);
        private static bool IsColorValid(string color) => string.IsNullOrEmpty(color);

        private static bool IsBirthDayValid(DateTime birthDate) => birthDate > DateTime.Now;

        private static bool IsRaceValid(string race) => string.IsNullOrEmpty(race);

        private static bool IsNameValid(string name) => string.IsNullOrEmpty(name);
    }
}