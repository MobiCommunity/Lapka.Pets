using System;
using System.Collections.Generic;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Exceptions.Pet;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Entities
{
    public abstract class AggregatePet : AggregateRoot
    {
        private const double MinimumWeight = 0;

        public string Name { get; private set; }
        public Species Species { get; private set; }
        public Sex Sex { get; private set; }
        public string MainPhotoPath { get; private set; }
        public List<string> PhotoPaths { get; private set; }
        public string Race { get; private set; }
        public DateTime BirthDay { get; private set; }
        public string Color { get; private set; }
        public double Weight { get; private set; }
        public bool Sterilization { get; private set; }
        
        protected AggregatePet(Guid id, string name, Sex sex, string race, Species species, string mainPhotoPath, DateTime birthDay,
            string color, double weight, bool sterilization, List<string> photoPaths = null)
        {
            Id = new AggregateId(id);
            Name = name;
            Sex = sex;
            Race = race;
            Species = species;
            MainPhotoPath = mainPhotoPath;
            PhotoPaths = photoPaths ?? new List<string>();
            BirthDay = birthDay;
            Color = color;
            Weight = weight;
            Sterilization = sterilization;
        }

        public void Update(string name, string race, Species species, string photoPath, Sex sex, DateTime birthDay,
            bool sterilization, double weight, string color)
        {
            Validate(name, race, birthDay, color, weight);

            Name = name;
            Race = race;
            Species = species;
            MainPhotoPath = photoPath;
            Sex = sex;
            BirthDay = birthDay;
            Sterilization = sterilization;
            Weight = weight;
            Color = color;
        }

        public void UpdateMainPhoto(string mainPhotoPath)
        {
            MainPhotoPath = mainPhotoPath;
            AddEvent(new PetPhotoUpdated(MainPhotoPath));
        }
        
        public void AddPhotos(List<string> photoPaths)
        {
            foreach (string path in photoPaths)
            {
                PhotoPaths.Add(path);
            }
            
            AddEvent(new PetPhotosAdded(photoPaths));
        }
        
        public void RemovePhoto(string photoPath)
        {
            PhotoPaths.Remove(photoPath);

            AddEvent(new PetPhotoDeleted(photoPath));
        }

        public abstract void Delete();

        protected static void Validate(string name, string race, DateTime birthDay, string color, double weight)
        {
            ValidateName(name);
            ValidateRace(race);
            ValidateBirthDay(birthDay);
            ValidateColor(color);
            ValidateWeight(weight);
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
    }
}