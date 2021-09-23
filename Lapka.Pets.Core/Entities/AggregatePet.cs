using System;
using System.Collections.Generic;
using System.Linq;
using Lapka.Pets.Core.Events.Concrete;
using Lapka.Pets.Core.Exceptions.Pet;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Core.Entities
{
    public abstract class AggregatePet : AggregateRoot
    {
        private ISet<string> _photoPaths = new HashSet<string>();
        public Guid UserId { get; }
        public string Name { get; private set; }
        public Species Species { get; private set; }
        public Sex Sex { get; private set; }
        public string MainPhotoPath { get; private set; }
        public string Race { get; private set; }
        public DateTime BirthDay { get; private set; }
        public string Color { get; private set; }
        public double Weight { get; private set; }
        public bool IsDeleted { get; private set;}

        
        public IEnumerable<string> PhotoPaths
        {
            get => _photoPaths;
            private set => _photoPaths = new HashSet<string>(value);
        }

        protected AggregatePet(Guid id, Guid userId, string name, Sex sex, string race, Species species,
            string mainPhotoPath, DateTime birthDay, string color, double weight, bool isDeleted = false,
            IEnumerable<string> photoPaths = null)
        {
            Id = new AggregateId(id);
            UserId = userId;
            Name = name;
            Sex = sex;
            Race = race;
            Species = species;
            MainPhotoPath = mainPhotoPath;
            BirthDay = birthDay;
            Color = color;
            Weight = weight;
            PhotoPaths = photoPaths ?? Enumerable.Empty<string>();
            IsDeleted = isDeleted;
        }

        public virtual void Update(string name, string race, Species species, Sex sex, DateTime birthDay, double weight,
            string color)
        {
            Validate(name, race, birthDay, color, weight);

            Name = name;
            Race = race;
            Species = species;
            Sex = sex;
            BirthDay = birthDay;
            Weight = weight;
            Color = color;
        }

        protected virtual void UpdateMainPhoto(string mainPhotoId)
        {
            MainPhotoPath = mainPhotoId;
        }

        public virtual void AddPhotos(IEnumerable<string> photoPaths)
        {
            foreach (string path in photoPaths)
            {
                _photoPaths.Add(path);
            }
        }
        
        public virtual void SetPhotos(IEnumerable<string> photoPaths)
        {
            _photoPaths = photoPaths.ToHashSet();
        }

        public virtual void RemovePhotos(IEnumerable<string> photoPaths)
        {
            foreach (string path in photoPaths)
            {
                _photoPaths.Remove(path);
            }
        }

        public virtual void Delete()
        {
            IsDeleted = true;
        }

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
            if (birthDate >= DateTime.UtcNow)
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
        
        private const double MinimumWeight = 0;

    }
}