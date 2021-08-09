using System;
using System.Collections.Generic;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Exceptions.Pet;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Entities
{
    public class Pet : AggregateRoot
    {
        private const double MiniumumWeight = 0;
        
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


        public Pet(Guid id, string name, Sex sex, Species species, string race, DateTime birthDay,
            string color, double weight, bool sterilization, Address shelterAddress, string description)
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
        
        public static Pet Create(Guid id, string name, Sex sex, Species species, string race, DateTime birthDay,
            string color, double weight, bool sterilization, Address shelterAddress,  string description)
        {
            ValidateCreation(name, race, birthDay, color, weight, description);
                
            Pet pet = new Pet(id, name, sex, species, race, birthDay, color, weight, sterilization, shelterAddress, description);
            
            pet.AddEvent(new PetCreated(pet));
            return pet;
        }

        private static void ValidateCreation(string name, string race, DateTime birthDay, string color, double weight, string description)
        {
            ValidateName(name);
            ValidateRace(race);
            ValidateBirthDay(birthDay);
            ValidateColor(color);
            ValidateWeight(weight);
            ValidateDescription(description);
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidPetNameException(name);
        }
        private static void ValidateRace(string race)
        {
            if (string.IsNullOrEmpty(race))
                throw new InvalidRaceValueException(race);
        }
        private static void ValidateBirthDay(DateTime birthDate)
        {
            if (birthDate >= DateTime.Now)
                throw new InvalidBirthDayValueException(birthDate);
        }
        private static void ValidateColor(string color)
        {
            if (string.IsNullOrEmpty(color))
                throw new InvalidColorValueException(color);
        }
        private static void ValidateWeight(double weight)
        {
            if (weight <= MiniumumWeight)
                throw new WeightBelowMinimumValueException(weight);
        }
        private static void ValidateDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                throw new InvalidDescriptionValueException(description);
        }
        
    }
}