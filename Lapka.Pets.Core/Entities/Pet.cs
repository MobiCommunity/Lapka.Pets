using System;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Exceptions.Pet;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Entities
{
    public abstract class Pet : AggregateRoot
    {
        private const double MinimumWeight = 0;

        public string Name { get; private set; }
        public Species Species { get; private set; }
        public Sex Sex { get; private set; }
        public string MainPhotoPath { get; private set; }
        public string Race { get; private set; }
        public DateTime BirthDay { get; private set; }
        public string Color { get; private set; }
        public double Weight { get; private set; }
        public bool Sterilization { get; private set; }
        
        public Pet(Guid id, string name, Sex sex, string race, Species species, string photoPath, DateTime birthDay,
            string color, double weight, bool sterilization, Address shelterAddress, string description)
        {
            Id = new AggregateId(id);
            Name = name;
            Sex = sex;
            Race = race;
            Species = species;
            MainPhotoPath = photoPath;
            BirthDay = birthDay;
            Color = color;
            Weight = weight;
            Sterilization = sterilization;
        }

        public abstract T Create<T>(Guid id, string name, Sex sex, string race, Species species, string photoPath,
            DateTime birthDay, string color, double weight, bool sterilization);

        public virtual void Update(string name, string race, Species species, string photoPath, Sex sex, DateTime birthDay, 
            string description, Address shelterAddress, bool sterilization, double weight, string color)
        {
            Validate(name, race, birthDay, color, weight, description);

            Name = name;
            Race = race;
            Species = species;
            MainPhotoPath = photoPath;
            Sex = sex;
            BirthDay = birthDay;
            Sterilization = sterilization;
            Weight = weight;
            Color = color;

            AddEvent(new PetUpdated(this));
        }

        protected virtual void Validate(string name, string race, DateTime birthDay, string color, double weight,
            string description)
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
            if (weight <= MinimumWeight)
                throw new WeightBelowMinimumValueException(weight);
        }

        private static void ValidateDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                throw new InvalidDescriptionValueException(description);
        }

        public void Delete()
        {
            AddEvent(new PetDeleted(this));
        }
    }
}